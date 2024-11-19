using UnityEngine;

public class NetDampen : MonoBehaviour
{
    [field: SerializeField]
    public float DampenAtSpeendMPS { get; private set; } = 1f;

    [field: SerializeField, Range(0f, 1f)]
    public float DampenFactor { get; private set; } = 0.8f;

    private void OnCollisionEnter(Collision collision)
    {
        var soccerBall = collision.transform.GetComponentInChildren<SoccerBallDebug>();
        if (soccerBall == null)
            return;

        Rigidbody soccerBallRB = collision.rigidbody;
        float metersPerSecond = soccerBallRB.velocity.magnitude;
        if (metersPerSecond > DampenAtSpeendMPS)
        {
            soccerBallRB.velocity *= DampenFactor;
        }
    }
}
