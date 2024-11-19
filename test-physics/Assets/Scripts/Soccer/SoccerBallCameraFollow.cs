using UnityEngine;

public class SoccerBallCameraFollow : MonoBehaviour
{
    [field: SerializeField]
    public Transform Target { get; private set; }

    [field: SerializeField]
    public float XOffset { get; private set; }

    [field: SerializeField]
    public float YOffset { get; private set; }

    [field: SerializeField]
    public float ZOffset { get; private set; }

    [field: SerializeField, Range(0f, 1f)]
    public float LookAtStrength { get; private set; } = 1f;


    void Update()
    {
        SetPosition();
        LookAtTarget();
    }

    private void OnValidate()
    {
        float positionZ = ComputePositionZ();
        bool isIncorrectPosition =
            this.transform.position.x != XOffset ||
            this.transform.position.y != YOffset ||
            this.transform.position.z != positionZ;
        if (isIncorrectPosition)
        {
            SetPosition();
            LookAtTarget();
        }
    }

    private float ComputePositionZ()
    {
        float positionZ = Target.position.z + ZOffset;
        return positionZ;
    }

    private void LookAtTarget()
    {
        Vector3 position = Target.position;
        position.y = Mathf.Lerp(0, position.y, LookAtStrength);
        transform.LookAt(position);
    }

    private void SetPosition()
    {
        Vector3 position = new Vector3();
        position.x = XOffset;
        position.y = YOffset;
        position.z = ComputePositionZ();
        transform.position = position;
    }
}
