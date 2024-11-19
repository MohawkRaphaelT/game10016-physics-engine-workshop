using UnityEngine;

public class SoccerBallMagnusEffect : MonoBehaviour
{
    [field: SerializeField] public Rigidbody SoccerBallRB { get; private set; }
    [field: SerializeField] public float MaxYCurveForce { get; private set; } = 0.2f;


    private void Awake()
    {
        if (SoccerBallRB == null)
        {
            string msg = $"Missing Component {nameof(Rigidbody)} {nameof(SoccerBallRB)}.";
            throw new MissingComponentException(msg);
        }
    }

    private void FixedUpdate()
    {
        ApplyCurve();
    }

    private void Reset()
    {
        if (SoccerBallRB == null)
            SoccerBallRB = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
        // Display axes of motion
        float velocityMagnitude = SoccerBallRB.velocity.magnitude;
        Vector3 velocityDirection = SoccerBallRB.velocity.normalized;
        Vector3 axisOfRotation = SoccerBallRB.angularVelocity.normalized;
        Vector3 curveDirection = Vector3.Cross(velocityDirection, axisOfRotation);
        // Only render if significant forces exist
        if (velocityMagnitude < 0.01f)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + curveDirection * 10);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + velocityDirection * 10);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + axisOfRotation * 10);
    }

    private void ApplyCurve()
    {
        // Split velocity components
        Vector3 velocityDirection = SoccerBallRB.velocity.normalized;
        //float magnitude = SoccerBallRB.velocity.magnitude;

        // Split angular velocity components
        Vector3 axisOfRotation = SoccerBallRB.angularVelocity.normalized;
        float torque = SoccerBallRB.angularVelocity.magnitude;
        torque = System.MathF.Sqrt(torque);

        // Create direction of force
        Vector3 curveDirection = Vector3.Cross(axisOfRotation, velocityDirection);
        // dampen any upward pull
        if (curveDirection.y > 0)
        {
            curveDirection.y *= MaxYCurveForce;
        }
        float curveForce = torque * SoccerBallRB.mass;

        // Apply force
        Vector3 force = curveDirection * curveForce;
        SoccerBallRB.AddForce(force, ForceMode.Force);
    }
}
