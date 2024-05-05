using System.Collections;
using UnityEngine;

public class PlayerBallInteraction : MonoBehaviour
{
    
    public Transform shootingPoint;

    public static bool ballIsShooted { get; private set; }

    private GameObject ball;
    
    private bool isCurrentlyColliding;
    private bool ballWasHit;

    private Vector2 ballEndPosition;


    private void Start()
    {
        isCurrentlyColliding = false;
        ballWasHit = false;
    }

    private void Update()
    {
        // Questo controllo serve per far capire all'npc quando deve rimuoversi per prendere la palla
        if (NPCBallInteraction_Tennis.ballIsShooted && !ballIsShooted)
        {
            ballIsShooted = false;
        }

        if (Input.GetKeyUp(KeyCode.Space) && !ballWasHit)
        {
            ballWasHit = true;
            StartCoroutine(TimeFakeAnimation());

            // qua sta colpendo la palla con la racchetta al momento giusto
            if (isCurrentlyColliding && ballWasHit)
            {
                if (ball != null)
                {
                    ballIsShooted = true;

                    ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                    // Qui resetta l'ombra e toglie anche il target
                    ball.GetComponent<BallShadow>().ResetShadow();

                    Vector2 direction = (AimArrow.MousePosition - shootingPoint.position).normalized;
                    float force = ForcefullThower.ForceShot;

                    // Calcolo della posizione finale della palla
                    ballEndPosition = (Vector2)shootingPoint.position + force * direction;

                    // Applico la forza alla palla
                    ball.GetComponent<Rigidbody2D>().AddForce(force * direction.normalized);

                    // Si fa partire l'effetto dell'ombra
                    ball.GetComponent<BallShadow>().InitShadowEffect(shootingPoint.position, ballEndPosition, direction.normalized);
                }
                else
                {
                    Debug.Log("La palla non è stata settata");
                }
            }
        }
    }

    // questa funzione serve solo a simulare un'animazione che poi questo verrà
    // tolto è sara messo l'evento su unity alla fine dell'animazione
    private IEnumerator TimeFakeAnimation()
    {
        yield return new WaitForSeconds(.7f);
        ballWasHit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball") && !ballWasHit)
        {
            ball = collision.gameObject;

            isCurrentlyColliding = true;
            // Debug.Log($"entra la collisione: {collision.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            isCurrentlyColliding = false;
            ball = null;
        }
    }
}
