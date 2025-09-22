using Firebase;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour
{
    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
                Debug.Log("Firebase inicializado correctamente.");
            else
                Debug.LogError($"Error inicializando Firebase: {task.Result}");
        });
    }
}
