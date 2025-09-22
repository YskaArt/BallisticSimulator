using System;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class ShotLogger : MonoBehaviour
{
    private DatabaseReference dbReference;

    private void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void LogShot(float angle, float force, float mass, bool hit, float impactDistance, int objectsHit)
    {
        if (dbReference == null)
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        string shotId = dbReference.Child("shots").Push().Key;

        var shotData = new Dictionary<string, object>
        {
            { "angle", angle },
            { "force", force },
            { "mass", mass },
            { "hit", hit },
            { "impactDistance", impactDistance },
            { "objectsHit", objectsHit },
            { "timestamp", DateTime.UtcNow.ToString("o") }
        };

        dbReference.Child("shots").Child(shotId).SetValueAsync(shotData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                Debug.Log($"[ShotLogger] Disparo registrado con ID: {shotId}");
            else
                Debug.LogError("[ShotLogger] Error guardando el disparo: " + task.Exception);
        });
    }

    // Recupera todos los disparos y ejecuta el callback con la lista de diccionarios (ejecuta callbacks en el hilo principal)
    public void FetchAllShots(Action<List<Dictionary<string, object>>> onComplete, Action<Exception> onError = null)
    {
        if (dbReference == null)
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        dbReference.Child("shots").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            try
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("[ShotLogger] Error al leer disparos: " + task.Exception);
                    onError?.Invoke(task.Exception);
                    return;
                }

                if (task.IsCanceled)
                {
                    var ex = new Exception("Firebase GetValueAsync fue cancelado");
                    Debug.LogError("[ShotLogger] Operación cancelada.");
                    onError?.Invoke(ex);
                    return;
                }

                DataSnapshot snapshot = task.Result;
                var list = new List<Dictionary<string, object>>();

                if (snapshot != null && snapshot.Exists)
                {
                    foreach (var child in snapshot.Children)
                    {
                        var dict = new Dictionary<string, object>();

                        // Iterate child children to build dictionary safely
                        foreach (var kvChild in child.Children)
                        {
                            try
                            {
                                dict[kvChild.Key] = kvChild.Value;
                            }
                            catch (Exception ex)
                            {
                                Debug.LogWarning($"[ShotLogger] Error leyendo clave '{kvChild.Key}': {ex}");
                            }
                        }

                        list.Add(dict);
                    }
                }

                onComplete?.Invoke(list);
            }
            catch (Exception ex)
            {
                Debug.LogError("[ShotLogger] Excepción procesando snapshot: " + ex);
                onError?.Invoke(ex);
            }
        });
    }
}
