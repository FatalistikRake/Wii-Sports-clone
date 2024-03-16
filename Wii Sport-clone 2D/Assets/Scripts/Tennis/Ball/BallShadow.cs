using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallShadow : MonoBehaviour
{
    [SerializeField]
    private GameObject shadow; // L'ombra della palla

    private Vector2 direction = new(-.06f, .1f); // La direzione dell'ombra

    private Vector2 startPosition, endPosition; // Le posizioni di inizio e fine dell'ombra
    private float distance; // La distanza tra l'inizio e la fine

    private bool ballIsInAir = false; // Un flag per controllare se la palla è in aria

    void Update()
    {
        // Se l'ordine di rendering dell'ombra non è corretto, lo corregge
        if (shadow.GetComponent<SpriteRenderer>().sortingOrder != gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1)
        {
            shadow.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }

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
            shadow.transform.position += 0.735f * Time.deltaTime * direction.ConvertTo<Vector3>()
                                                    :
            //  Se la palla ha percorso meno della metà della distanza,ùl’ombra si muove in una certa
            //  direzione a una velocità proporzionale al tempo trascorso dall’ultimo frame (Time.deltaTime).
            shadow.transform.position -= 0.735f * Time.deltaTime * direction.ConvertTo<Vector3>();


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
                Debug.Log("Esce");
            }
        }
    }

    // Inizializza l'effetto ombra è mettiamo il flag a true per dire che la palla e in movimento
    public void InitShadowEffect(Vector2 startPosition, Vector2 endPosition)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;

        distance = Vector2.Distance(startPosition, endPosition);

        ballIsInAir = true;
    }
}
