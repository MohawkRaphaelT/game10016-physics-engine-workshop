using System.Collections.Generic;
using UnityEngine;

public class SoccerBallDebug : MonoBehaviour
{
    [field: SerializeField] public Rigidbody SoccerBallRB { get; private set; }

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
    }

    private void Reset()
    {
        if (SoccerBallRB == null)
            SoccerBallRB = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
        // Draw position and facing directions
        Gizmos.color = Color.red;
        Gizmos.DrawLineStrip(positions.ToArray(), false);
        Gizmos.color = Color.white;
        Gizmos.DrawLineStrip(normals.ToArray(), false);

        // Display axes of motion
        float velocityMagnitude = SoccerBallRB.velocity.magnitude;
        Vector3 velocityDirection = SoccerBallRB.velocity.normalized;
        Vector3 axisOfRotation = SoccerBallRB.angularVelocity.normalized;
        // Only render if significant forces exist
        if (velocityMagnitude < 0.01f)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + velocityDirection * 10);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + axisOfRotation * 10);
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

    public void ClearDebugData()
    {
        positions.Clear();
        normals.Clear();
    }
}
