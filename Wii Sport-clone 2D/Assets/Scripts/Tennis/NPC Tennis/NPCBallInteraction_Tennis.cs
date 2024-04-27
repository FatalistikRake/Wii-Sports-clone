using Unity.VisualScripting;
using UnityEngine;

public class NPCBallInteraction_Tennis : MonoBehaviour
{
    public static bool ballIsShooted { get; private set; }

    public Transform shootingPoint;

    private GameObject ball;

    private void Start()
    {
        ballIsShooted = false;
    }

    private void Update()
    {
        // Questo controllo serve per far capire all'npc quando deve rimuoversi per prendere la palla
        if (PlayerBallInteraction.ballIsShooted)
        {
            ballIsShooted = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("collision npc:" + collision.name);
        if (collision.CompareTag("Ball"))
        {
            ballIsShooted = true;

            if (ball != collision.gameObject)
            {
                ball = collision.gameObject;
            }

            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            ball.GetComponent<BallShadow>().ResetShadow();

            Vector2 randomPosition = GetRadomPosition(GetComponentInParent<NPCMovement_Tennis>().opponentPlayingField.GetComponent<Collider2D>().bounds);
            // Debug.Log($"Random X: {randomPosition.x}, Random Y: {randomPosition.y}");

            Vector2 direction = randomPosition - (Vector2)shootingPoint.position; // Calcola la direzione del tiro

            float force = 5f * Vector2.Distance((Vector2)shootingPoint.position, randomPosition);

            // Applica la forza ( la spara verso il punto indicato )
            ball.GetComponent<Rigidbody2D>().AddForce(force * direction.normalized);

            // Si fa partire l'effetto dell'ombra
            ball.GetComponent<BallShadow>().InitShadowEffect((Vector2)shootingPoint.position, randomPosition, direction.normalized);
        }
    }

    private Vector2 GetRadomPosition(Bounds bounds)
    {
        return new(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
    }
}
