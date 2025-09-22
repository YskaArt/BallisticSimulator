using UnityEngine;
using System.Collections.Generic;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    private List<ObjectivePart> allPieces = new List<ObjectivePart>();
    private int destroyedCount = 0;

    void Awake() => Instance = this;

    public void RegisterPiece(ObjectivePart part)
    {
        allPieces.Add(part);
    }

    public void RegisterDestroyedPiece(ObjectivePart part)
    {
        destroyedCount++;
    }

    public int GetDestroyedPiecesCount() => destroyedCount;

    public void ResetCount()
    {
        destroyedCount = 0;
    }

    // Devuelve la distancia mínima desde 'point' hasta cualquier pieza registrada (o Mathf.Infinity si no hay piezas)
    public float GetClosestPieceDistance(Vector3 point)
    {
        float minDist = Mathf.Infinity;
        foreach (var part in allPieces)
        {
            if (part == null) continue;
            float d = Vector3.Distance(point, part.transform.position);
            if (d < minDist) minDist = d;
        }
        return minDist == Mathf.Infinity ? -1f : minDist;
    }
}
