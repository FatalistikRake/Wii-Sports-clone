using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallShadow : MonoBehaviour
{
    public GameObject shadow;

    private Vector3 shadowDirection = new(.03f, -.1f); // La direzione dell'ombra

    private Vector2 startPosition, endPosition; // Le posizioni di inizio e fine dell'ombra
    private float distance; // La distanza tra l'inizio e la fine

    private bool ballIsInAir = false; // Un flag per controllare se la palla è in aria

    private float shadowSpeedMovement;

    void Update()
    {
        // Se la palla è in aria, muove l'ombra in base alla posizione della palla
        if (ballIsInAir)
        {
                                        shadow.transform.position
                                                    =
            // controlla se la palla ha percorso meno della metà della distanza totale.
            // Se è vero, l’ombra si muove in una direzione, altrimenti si muove nella direzione opposta.
            Vector2.Distance(startPosition, transform.position.ConvertTo<Vector2>()) <= distance / 2
                                                    ?
            // Se la palla ha percorso meno della metà della distanza, l’ombra si muove in una certa
            // direzione a una velocità proporzionale al tempo trascorso dall’ultimo frame (Time.deltaTime).
            shadow.transform.position += shadowSpeedMovement * Time.deltaTime * shadowDirection
                                                    :
            //  Se la palla ha percorso più  della metà della distanza,l’ombra si muove in una certa
            //  direzione a una velocità proporzionale al tempo trascorso dall’ultimo frame (Time.deltaTime).
            shadow.transform.position -= shadowSpeedMovement * Time.deltaTime * shadowDirection;


            /*if (Vector2.Distance(startPosition, transform.position.ConvertTo<Vector2>()) <= distance / 2)
                Debug.Log($"e prima della metà   distanza percorsa palla: {Vector2.Distance(transform.position.ConvertTo<Vector2>(), startPosition)}    distance metà: {distance / 2}    distanza totale: {distance}");
            else
                Debug.Log($"e dopo della metà    distanza percorsa palla: {Vector2.Distance(transform.position.ConvertTo<Vector2>(), startPosition)}    distance metà: {distance / 2}    distanza totale: {distance}");
*/

            // Se la palla ha raggiunto la posizione finale, resetta l'ombra precisamente sotto la palla e il flag a falso
            if (Vector2.Distance(transform.position.ConvertTo<Vector2>(), endPosition) <= 0.1f)
            {
                ballIsInAir = false;
                shadow.transform.localPosition = Vector3.zero;
                gameObject.GetComponent<BallBounce>().Bounce();
                Debug.Log("La palla arriva al punto finale");
            }
        }
    }

    /// <summary>
    /// Inizializza l'effetto ombra è mettiamo il flag a true per dire che la palla e in movimento
    /// </summary>
    public void InitShadowEffect(Vector2 startPosition, Vector2 endPosition, Vector2 ballDirection)
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
    }
}
