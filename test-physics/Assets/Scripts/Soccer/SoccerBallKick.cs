using UnityEngine;

public class SoccerBallKick : MonoBehaviour
{
    [field: SerializeField] public Rigidbody SoccerBallRB { get; private set; }
    [field: SerializeField] public float KickForceKPH { get; private set; } = 1;
    [field: SerializeField] public float KickSpinForce { get; private set; } = 1;
    [field: SerializeField] public bool DoKickBall { get; private set; }
    [field: SerializeField] public KeyCode KickKey { get; private set; } = KeyCode.Space;


    private void Awake()
    {
        if (SoccerBallRB == null)
        {
            string msg = $"Missing Component {nameof(Rigidbody)} {nameof(SoccerBallRB)}.";
            throw new MissingComponentException(msg);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KickKey))
        {
            DoKickBall = true;
        }
    }

    private void FixedUpdate()
    {
        if (!DoKickBall)
            return;
        DoKickBall = false;

        Vector3 kickDirection = SoccerBallRB.transform.forward;
        Vector3 force = KickForceKPH * SoccerBallRB.mass * kickDirection;
        SoccerBallRB.AddForce(force, ForceMode.Impulse);

        Vector3 kickSpinDirection = SoccerBallRB.transform.up;
        Vector3 torque = kickSpinDirection * KickSpinForce;
        SoccerBallRB.AddTorque(torque, ForceMode.Impulse);
    }

    private void Reset()
    {
        if (SoccerBallRB == null)
            SoccerBallRB = GetComponent<Rigidbody>();
    }
}
