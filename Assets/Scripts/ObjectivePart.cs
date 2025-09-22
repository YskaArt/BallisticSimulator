using UnityEngine;

public class ObjectivePart : MonoBehaviour
{
    private FixedJoint joint;
    private bool alreadyCounted = false;
    private bool hadJointAtStart = false; // indica si la pieza era suministrada por una unión al inicio

    void Start()
    {
        joint = GetComponent<FixedJoint>();
        hadJointAtStart = joint != null;
        ObjectiveManager.Instance.RegisterPiece(this);
    }

    void Update()
    {
        // Solo contar como derribada si al inicio tenía un FixedJoint y después quedó sin él
        if (!alreadyCounted && hadJointAtStart && joint == null)
        {
            alreadyCounted = true;
            ObjectiveManager.Instance.RegisterDestroyedPiece(this);
        }
    }
}
