using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseLogUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText;

    public void AddLogEntry(string message)
    {
        if (logText == null) return;

        logText.text += message + "\n";

        // Forzar actualización de canvas/layout
        Canvas.ForceUpdateCanvases();

        // Redimensionar el rectTransform según el texto preferido
        var rt = logText.rectTransform;
        float width = rt.rect.width;
        Vector2 preferred = logText.GetPreferredValues(logText.text, width, 0);
        float newHeight = preferred.y + 8f; // padding pequeño
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

        // Si está dentro de un ScrollRect, desplazar al final
        var scroll = GetComponentInParent<ScrollRect>();
        if (scroll != null)
        {
            Canvas.ForceUpdateCanvases();
            scroll.verticalNormalizedPosition = 0f;
        }
    }

    public void Clear()
    {
        if (logText == null) return;
        logText.text = string.Empty;
        Canvas.ForceUpdateCanvases();
    }
}
