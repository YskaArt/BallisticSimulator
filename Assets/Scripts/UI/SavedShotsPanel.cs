using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Small UI controller to display saved shots in a scrollable panel
public class SavedShotsPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot; // panel root to enable/disable
    [SerializeField] private Button refreshButton;
    [SerializeField] private FirebaseLogUI firebaseLogUI; // Referencia al log visual

    private ShotLogger logger;

    private void Awake()
    {
        logger = FindAnyObjectByType<ShotLogger>();
        if (refreshButton != null)
            refreshButton.onClick.AddListener(Refresh);
    }

    public void Refresh()
    {
        if (firebaseLogUI != null)
            firebaseLogUI.Clear();

        if (logger == null)
        {
            firebaseLogUI?.AddLogEntry("No se encontró ShotLogger en la escena.");
            return;
        }

        firebaseLogUI?.AddLogEntry("Cargando...");

        logger.FetchAllShots(list =>
        {
            firebaseLogUI?.AddLogEntry("\n--- Lista de disparos guardados ---");
            if (list == null || list.Count == 0)
            {
                firebaseLogUI?.AddLogEntry("No hay disparos guardados.");
                return;
            }
            int idx = 1;
            foreach (var dict in list)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine($"Disparo #{idx}");
                if (dict.TryGetValue("timestamp", out var ts)) sb.AppendLine($"  Timestamp: {ts}");
                if (dict.TryGetValue("angle", out var a)) sb.AppendLine($"  Ángulo: {a}");
                if (dict.TryGetValue("force", out var f)) sb.AppendLine($"  Fuerza: {f}");
                if (dict.TryGetValue("mass", out var m)) sb.AppendLine($"  Masa: {m}");
                if (dict.TryGetValue("hit", out var h)) sb.AppendLine($"  Acierto: {h}");
                if (dict.TryGetValue("impactDistance", out var d)) sb.AppendLine($"  Distancia: {d}");
                if (dict.TryGetValue("objectsHit", out var o)) sb.AppendLine($"  Objetos afectados: {o}");
                firebaseLogUI.AddLogEntry(sb.ToString());
                idx++;
            }
        }, err =>
        {
            firebaseLogUI?.AddLogEntry("Error cargando disparos: " + err.Message);
        });
    }

    // Alterna la visibilidad del panel
    public void ShowPanel()
    {
        if (panelRoot != null)
        {
            bool newState = !panelRoot.activeSelf;
            panelRoot.SetActive(newState);
            if (newState) Refresh();
        }
    }
}
