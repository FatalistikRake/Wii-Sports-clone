using System.Collections;
using UnityEngine;

public class Shootballs : MonoBehaviour
{
    // capire se una modalità Single Player (true) o Double Player (false)
    public bool gameModeSinglePlayer;

    public GameObject topPlayingField;
    public GameObject bottomPlayingField;

    // In quale campo si trova lo spara palle
    private GameObject opponentPlayingField;

    // per eseguire una sola volta il controllo del corrente campo
    private bool opponentPlayingFieldHasTriggered = false;

    // La palla che dovrà essere sparata
    public GameObject ballPrefab;

    // Velocita della palla sparata
    public float ballSpeed = 20;

    // Tempo prima che l'altra palla verrà sparata
    public float ballTimeCounter = 1;

    // Controllare se la palla è stata sparata
    public bool ballShooted = false;

    // Posizione iniziale dalla palla in cui verrà sparata
    public Transform shootingPoint;

    public GameObject targetPrefab;


    private void Update()
    {
        if (!ballShooted && Input.GetKey(KeyCode.P))
        {
            StartCoroutine(ShootTheBall());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && !opponentPlayingFieldHasTriggered)
        {
            if (collision.CompareTag("TopPlayingField"))
            {
                opponentPlayingField = bottomPlayingField;
                opponentPlayingFieldHasTriggered = true;
            }
            else if (collision.CompareTag("BottomPlayingField"))
            {
                opponentPlayingField = topPlayingField;
            }
        }
    }

    private IEnumerator ShootTheBall()
    {
        ballShooted = true;


        Vector3 randomPosition = GetRadomPosition(opponentPlayingField.GetComponent<BoxCollider2D>().bounds);
        
        // Debug.Log($"Random X: {randomPosition.x}, Random Y: {randomPosition.y}");

        Vector3 direction = (randomPosition - shootingPoint.position).normalized; // Calcola la direzione del tiro


        Instantiate(targetPrefab, randomPosition, Quaternion.identity); // crea un indicatore target nel punto randomico

        GameObject newBall = Instantiate(ballPrefab, shootingPoint.position, Quaternion.identity); // Crea la palla


        // Applica la forza ( la spara verso il punto indicato )
        newBall.GetComponent<Rigidbody2D>().AddForce(direction * ballSpeed);

        newBall.GetComponent<BallShadow>().InitShadowEffect(shootingPoint.position, randomPosition); // Inizia l'effetto dell'ombra della palla

        yield return new WaitForSeconds(ballTimeCounter);

        ballShooted = false;
    }

    /// <summary>
    /// Prende un punto randomico nel campo dell'avversario
    /// </summary>
    private Vector3 GetRadomPosition(Bounds bounds)
    {
        float minX = bounds.min.x;
        float maxX = bounds.max.x;
        float minY = bounds.min.y;
        float maxY = bounds.max.y;

        return new(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
    }
}
