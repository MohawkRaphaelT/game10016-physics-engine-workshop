using UnityEngine;

public class ManualGravity : MonoBehaviour
{
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public Vector3 Direction { get; private set; } = Vector3.down;
    [field: SerializeField] public float Force { get; private set; } = 9.81f;

    private void Awake()
    {
        if (Rigidbody == null)
        {
            string msg = $"Missing Component {nameof(Rigidbody)} {nameof(Rigidbody)}.";
            throw new MissingComponentException(msg);
        }
    }

    private void FixedUpdate()
    {
        Vector3 force = Rigidbody.mass * Force * Direction;
        Rigidbody.AddForce(force, ForceMode.Force);
    }

    private void Reset()
    {
        if (Rigidbody == null)
            Rigidbody = GetComponent<Rigidbody>();
    }
}
