using System.Collections.Generic;
using UnityEngine;

public class SoccerBallDebug : MonoBehaviour
{
    [field: SerializeField] public Rigidbody SoccerBallRB { get; private set; }
    [field: SerializeField] public float MaxSpeedMPS { get; private set; }
    [field: SerializeField] public float MaxSpeedKMH { get; private set; }
    [field: SerializeField] public float MaxForceTorque { get; private set; }
    [field: SerializeField] public float MaxForceTorqueSqrt { get; private set; }
    [field: SerializeField] public float MaxForceTorqueCbrt { get; private set; }

    private const int gizmoCount = 8196;
    private List<Vector3> positions = new();
    private List<Vector3> normals = new();

    private void Awake()
    {
        if (SoccerBallRB == null)
        {
            string msg = $"Missing Component {nameof(Rigidbody)} {nameof(SoccerBallRB)}.";
            throw new MissingComponentException(msg);
        }

        positions = new List<Vector3>(gizmoCount);
        normals = new List<Vector3>(gizmoCount);
    }

    private void FixedUpdate()
    {
        RecordDebugData();
        RecordMaxSpeedMPS();
    }

    private void Reset()
    {
        if (SoccerBallRB == null)
            SoccerBallRB = GetComponent<Rigidbody>();
    }

    private void RecordDebugData()
    {
        // Only render if significant forces exist
        float velocityMagnitude = SoccerBallRB.velocity.magnitude;
        if (velocityMagnitude < 0.01f)
            return;

        // Clear old data when at limit
        if (positions.Count >= gizmoCount)
        {
            positions.RemoveAt(0);
            normals.RemoveAt(0);
        }
        // Add new data
        positions.Add(transform.position);
        normals.Add(transform.position + transform.forward);
    }

    private void RecordMaxSpeedMPS()
    {
        // Linear Force
        float metersPerSecond = SoccerBallRB.velocity.magnitude;
        if (MaxSpeedMPS < metersPerSecond)
        {
            MaxSpeedMPS = metersPerSecond;
            // div seconds in minute
            // div minutes in hour
            // mul 1000 meters
            MaxSpeedKMH = metersPerSecond * 60 * 60 / 1000;
        }

        // Rotational Force
        float poundsPerSecond = SoccerBallRB.angularVelocity.magnitude;
        if (MaxForceTorque < poundsPerSecond)
        {
            MaxForceTorque = poundsPerSecond;
            MaxForceTorqueSqrt = System.MathF.Sqrt(poundsPerSecond);
            MaxForceTorqueCbrt = System.MathF.Cbrt(poundsPerSecond);
        }
    }

    public void ClearDebugData()
    {
        positions.Clear();
        normals.Clear();
    }
}
