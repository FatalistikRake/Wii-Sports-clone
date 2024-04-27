using System.Collections;
using UnityEngine;

public class Shootballs : MonoBehaviour
{
    /// <summary>
    /// capire se una modalità Single Player (true) o Double Player (false)
    /// </summary>
    public bool gameModeSinglePlayer;

    /// <summary>
    /// In quale campo si trova lo spara palle
    /// </summary>
    private GameObject opponentPlayingField;

    /// <summary>
    /// La palla che dovrà essere sparata
    /// </summary>
    public GameObject ballPrefab;

    /// <summary>
    /// Velocita della palla sparata
    /// </summary>
    public float ballSpeed = 20;

    /// <summary>
    /// Tempo prima che l'altra palla verrà sparata
    /// </summary>
    public float ballTimeCounter = 1;

    /// <summary>
    /// Controllare se la palla è stata sparata
    /// </summary>
    private bool ballShooted = false;

    /// <summary>
    /// Posizione iniziale dalla palla in cui verrà sparata
    /// </summary>
    public Transform shootingPoint;

    private void Update()
    {
        if (!ballShooted && Input.GetKey(KeyCode.P))
        {
            StartCoroutine(ShootTheBall());
        }
    }

    private IEnumerator ShootTheBall()
    {
        ballShooted = true;


        Vector2 randomPosition = GetRadomPosition(opponentPlayingField.GetComponent<BoxCollider2D>().bounds);
        
        // Debug.Log($"Random X: {randomPosition.x}, Random Y: {randomPosition.y}");

        Vector2 direction = (randomPosition - (Vector2)shootingPoint.position).normalized; // Calcola la direzione del tiro

        GameObject newBall = Instantiate(ballPrefab, shootingPoint.position, Quaternion.identity); // Crea la palla

        // Applica la forza ( la spara verso il punto indicato )
        newBall.GetComponent<Rigidbody2D>().AddForce(ballSpeed * direction);

        // Inizia l'effetto dell'ombra della palla con la velocità costante già impostata
        newBall.GetComponent<BallShadow>().InitShadowEffect(shootingPoint.position, randomPosition, direction);

        yield return new WaitForSeconds(ballTimeCounter);

        ballShooted = false;
    }


    // per eseguire una sola volta il controllo del corrente campo
    private bool opponentPlayingFieldHasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && !opponentPlayingFieldHasTriggered)
        {
            if (collision.CompareTag("TopPlayingField"))
            {
                opponentPlayingField = FindFieldsManager_Tennis.bottomPlayingField;
                opponentPlayingFieldHasTriggered = true;
            }
            else if (collision.CompareTag("BottomPlayingField") && !opponentPlayingFieldHasTriggered)
            {
                opponentPlayingField = FindFieldsManager_Tennis.topPlayingField;
                opponentPlayingFieldHasTriggered = true;
            }
        }
    }

    /// <summary>
    /// Prende un punto randomico nel campo dell'avversario
    /// </summary>
    private Vector2 GetRadomPosition(Bounds bounds)
    {
        return new(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
    }
}
