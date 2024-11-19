using UnityEngine;

public class SoccerBallContactFriction : MonoBehaviour
{
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public Transform Ground { get; private set; }
    [field: SerializeField] public float ContactDrag { get; private set; } = 0f;
    [field: SerializeField] public float AirborneDrag { get; private set; } = 0f;
    [field: SerializeField] public float ContactAngularDrag { get; private set; } = 0f;
    [field: SerializeField] public float AirborneAngularDrag { get; private set; } = 0f;
    [field: SerializeField] public string DebugMsg { get; private set; }

    private void Awake()
    {
        if (Rigidbody == null)
        {
            string msg = $"Missing Component {nameof(Rigidbody)} {nameof(Rigidbody)}.";
            throw new MissingComponentException(msg);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform != Ground)
            return;

        Rigidbody.drag = ContactDrag;
        Rigidbody.angularDrag = ContactAngularDrag;

        DebugMsg = "Grounded";
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform != Ground)
            return;

        Rigidbody.drag = AirborneDrag;
        Rigidbody.angularDrag = AirborneAngularDrag;

        DebugMsg = "Airborne";
    }

    private void Reset()
    {
        if (Rigidbody == null)
            Rigidbody = GetComponent<Rigidbody>();
    }
}
