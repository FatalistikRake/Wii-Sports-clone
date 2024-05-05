using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BallBounce))]
public class BallShadow : MonoBehaviour
{
    public GameObject shadow;

    public bool ballIsInAir { get; private set; } = false;// Un flag per controllare se la palla è in aria

    private Vector2 shadowDirection = new(.03f, -.1f); // La direzione dell'ombra

    public Vector2 startPosition { get; private set; } // Le posizioni di inizio e fine dell'ombra
    public Vector2 endPosition { get; private set; } // Le posizioni di inizio e fine dell'ombra

    private float distance; // La distanza tra l'inizio e la fine


    private float shadowSpeedMovement;

    void Update()
    {
        // Se la palla è in aria, muove l'ombra in base alla posizione della palla
        if (ballIsInAir)
        {
            // controlla se la palla ha percorso meno della metà della distanza totale.
            // Se è vero, l’ombra si muove in una direzione, altrimenti si muove nella direzione opposta.
            if (Vector2.Distance(startPosition, (Vector2)transform.position) <= distance / 2)
                // Se la palla ha percorso meno della metà della distanza, l’ombra si muove in una certa
                // direzione a una velocità proporzionale al tempo trascorso dall’ultimo frame (Time.deltaTime).
                shadow.transform.position += shadowSpeedMovement * Time.deltaTime * (Vector3)shadowDirection;
            else
                //  Se la palla ha percorso più della metà della distanza,l’ombra si muove in una certa
                //  direzione a una velocità proporzionale al tempo trascorso dall’ultimo frame (Time.deltaTime).
                shadow.transform.position -= shadowSpeedMovement * Time.deltaTime * (Vector3)shadowDirection;


            /*if (Vector2.Distance(startPosition, (Vector2)transform.position) <= distance / 2)
                Debug.Log($"e prima della metà   distanza percorsa palla: {Vector2.Distance((Vector2)transform, startPosition)}    distance metà: {distance / 2}    distanza totale: {distance}");
            else
                Debug.Log($"e dopo della metà    distanza percorsa palla: {Vector2.Distance((Vector2)transform.position, startPosition)}    distance metà: {distance / 2}    distanza totale: {distance}");*/

            // Se la palla ha raggiunto la posizione finale, resetta l'ombra precisamente sotto la palla e il flag a falso
            if (Vector2.Distance((Vector2)transform.position, endPosition) <= 0.1f)
            {
                ballIsInAir = false;
                shadow.transform.localPosition = Vector3.zero;
                gameObject.GetComponent<BallBounce>().Bounce();

                // Quando e l'ultimo rimbalzo si ferma pian piano la palla la palla
                if (gameObject.GetComponent<BallBounce>().RandomPoints.Count <= 0 && gameObject.GetComponent<Rigidbody2D>() != null)
                {
                    ResetShadow();
                    StartCoroutine(SlowdownBall());
                }
            }
        }
    }

    // Metodo per rallentare gradualmente la palla
    public IEnumerator SlowdownBall()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        // Riduci gradualmente la velocità fino a che non raggiunge lo zero
        while (rb.velocity.magnitude > 0)
        {
            // Calcola la velocità di rallentamento in base alla velocità attuale
            Vector2 slowdownVelocity = 0.1f * Time.deltaTime * rb.velocity.normalized;

            // Assicurati che la velocità di rallentamento non superi la velocità attuale
            slowdownVelocity = Vector2.ClampMagnitude(slowdownVelocity, rb.velocity.magnitude);

            // Applica la velocità di rallentamento
            rb.velocity -= slowdownVelocity;

            // Aspetta il prossimo frame
            yield return null;
        }

        // Assicurati che la velocità sia esattamente zero
        rb.velocity = Vector2.zero;
    }

    /// <summary>
    /// Inizializza l'effetto ombra è mettiamo il flag a true per dire che la palla e in movimento
    /// </summary>
    public void InitShadowEffect(Vector2 startPosition, Vector2 endPosition, Vector2 ballDirection)
    {
        StartCoroutine(gameObject.GetComponent<BallBounce>().PickRandomicPoints(endPosition, ballDirection));

        this.startPosition = startPosition;
        this.endPosition = endPosition;
        shadowSpeedMovement = .835f;

        distance = Vector2.Distance(startPosition, endPosition);

        ballIsInAir = true;
    }

    public void InitShadowEffect(Vector2 startPosition, Vector2 endPosition, Vector2 ballDirection, float shadowSpeedMovement)
    {
        if (gameObject.GetComponent<BallBounce>().RandomPoints != null)
        {
            StartCoroutine(gameObject.GetComponent<BallBounce>().PickRandomicPoints(endPosition, ballDirection));
        }
        else
        {
            Debug.Log("Non a creato nessun punto");
        }

        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.shadowSpeedMovement = shadowSpeedMovement;

        distance = Vector2.Distance(startPosition, endPosition);

        ballIsInAir = true;
    }

    public void BounceShadowEffect(Vector2 nextPosition)
    {
        startPosition = endPosition;
        endPosition = nextPosition;

        distance = Vector2.Distance(startPosition, endPosition);

        ballIsInAir = true;
    }


    public void ResetShadow()
    {
        ballIsInAir = false;
        shadow.transform.localPosition = Vector3.zero;
        gameObject.GetComponent<BallBounce>().RandomPoints.Clear();
    }
}
