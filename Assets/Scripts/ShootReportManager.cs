using UnityEngine;
using TMPro;

public class ShootReportManager : MonoBehaviour
{
    public static ShootReportManager Instance;

    [SerializeField] private TMP_Text reportText;

    void Awake() => Instance = this;

    // Datos ampliados del reporte
    public void ShowReport(
        float time,
        Vector3 point,
        float velocity,
        float impulse,
        int destroyed,
        float angleUsed,
        float forceUsed,
        float massUsed,
        bool hit,
        float distanceToObjective)
    {
        int score = CalculateScore(destroyed, impulse);

        string hitResult;
        if (hit)
        {
            hitResult = "Acierto";
            // si hay distancia al objetivo (>=0) mostrarla también
            if (distanceToObjective >= 0f)
                hitResult += $" (distancia: {distanceToObjective:F2} m)";
        }
        else
        {
            if (distanceToObjective >= 0f)
                hitResult = $"No acertó (distancia mínima al objetivo: {distanceToObjective:F2} m)";
            else
                hitResult = "No acertó";
        }

        reportText.text =
            $"📊 REPORTE DE TIRO\n" +
            $"🔺 Ángulo usado: {angleUsed:F1}°\n" +
            $"💪 Fuerza usada: {forceUsed:F2}\n" +
            $"⚖️ Masa del proyectil: {massUsed:F2} kg\n" +
            $"🎯 Resultado del impacto: {hitResult}\n" +
            $"🧩 Piezas derribadas: {destroyed}\n" +
            $"⚡ Velocidad: {velocity:F2} m/s\n" +
            $"⏱ Tiempo de vuelo: {time:F2} s\n" +
            $"🏆 Puntuación: {score}";
    }

    private int CalculateScore(int destroyed, float impulse)
    {
        // Ejemplo de puntuación:
        return destroyed * 100 + Mathf.RoundToInt(impulse * 0.5f);
    }
}
