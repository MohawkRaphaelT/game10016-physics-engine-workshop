using UnityEngine;

public class SoccerBallReset : MonoBehaviour
{
    [field: SerializeField] public Rigidbody SoccerBallRB { get; private set; }
    [field: SerializeField] public KeyCode ResetKey { get; private set; } = KeyCode.R;

    private Vector3 position;
    private Quaternion orientation;

    private void Awake()
    {
        if (SoccerBallRB == null)
        {
            string msg = $"Missing Component {nameof(Rigidbody)} {nameof(SoccerBallRB)}.";
            throw new MissingComponentException(msg);
        }

        position = transform.position;
        orientation = transform.rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(ResetKey))
        {
            SoccerBallRB.MovePosition(position);
            SoccerBallRB.MoveRotation(orientation);
            SoccerBallRB.velocity = Vector3.zero;
            SoccerBallRB.angularVelocity = Vector3.zero;
            foreach (var script in FindObjectsByType<SoccerBallDebug>(FindObjectsSortMode.None))
            {
                script.ClearDebugData();
            }
        }
    }

    private void Reset()
    {
        if (SoccerBallRB == null)
            SoccerBallRB = GetComponent<Rigidbody>();
    }
}
