using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileReport : MonoBehaviour
{
    private float startTime;
    private Rigidbody rb;
    private bool hasImpacted = false;

    // Datos del disparo que serán rellenados por el Canon al instanciar
    public float initialAngle;
    public float forceUsed;
    public float massUsed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startTime = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasImpacted) return; // Solo registrar el primer impacto
        hasImpacted = true;

        // Datos del impacto
        float flightTime = Time.time - startTime;
        Vector3 impactPoint = collision.contacts[0].point;
        float impactSpeed = rb.linearVelocity.magnitude;
        float impactImpulse = collision.impulse.magnitude;

        // Determinar si el impacto fue sobre una pieza del objetivo
        ObjectivePart hitPart = collision.collider.GetComponentInParent<ObjectivePart>();
        bool hit = hitPart != null;

        float distanceToObjective;
        if (hit)
        {
            distanceToObjective = Vector3.Distance(impactPoint, hitPart.transform.position);
        }
        else
        {
            // Distancia mínima al conjunto de piezas registradas (o -1 si no hay piezas)
            distanceToObjective = ObjectiveManager.Instance.GetClosestPieceDistance(impactPoint);
        }

        // Contar piezas derribadas
        int piecesDestroyed = ObjectiveManager.Instance.GetDestroyedPiecesCount();

        // Pasar datos al gestor, incluyendo parámetros del disparo y resultado
        ShootReportManager.Instance.ShowReport(
            flightTime,
            impactPoint,
            impactSpeed,
            impactImpulse,
            piecesDestroyed,
            initialAngle,
            forceUsed,
            massUsed,
            hit,
            distanceToObjective
        );

        // También registrar en Firebase si existe un ShotLogger en la escena
        var logger = FindObjectOfType<ShotLogger>();
        if (logger != null)
        {
            // objectsHit: usar piecesDestroyed
            logger.LogShot(initialAngle, forceUsed, massUsed, hit, distanceToObjective, piecesDestroyed);
        }
        else
        {
            Debug.LogWarning("[ProjectileReport] No ShotLogger encontrado en la escena. El disparo no se guardará en Firebase.");
        }
    }
}
