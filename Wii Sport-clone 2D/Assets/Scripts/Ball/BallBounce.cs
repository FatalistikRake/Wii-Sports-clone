using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BallShadow))]
public class BallBounce : MonoBehaviour
{
    private Vector2 lastVelocity;
    private Rigidbody2D rb;

    public List<Vector2> RandomPoints;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        RandomPoints.Clear();
    }

    private void Update()
    {
        lastVelocity = rb.velocity;
    }

    public void Bounce()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity /= 1.7f;

        if (RandomPoints.Count > 0)
        {
            gameObject.GetComponent<BallShadow>().BounceShadowEffect(RandomPoints[0]);
            RandomPoints.RemoveAt(0);
        }
        Debug.Log("Boing!!!");
    }

    // Per rimbalzare nei oggetti tipo i muri
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            var speed = lastVelocity.magnitude / .4f;
            var directionBounce = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            // Debug.Log("Direction Bounce: " + directionBounce);

            rb.AddForce(Mathf.Max(speed, 0f) * Time.timeScale * directionBounce);
            gameObject.GetComponent<BallShadow>().ResetShadow();
            StartCoroutine(gameObject.GetComponent<BallShadow>().SlowdownBall());
        }
    }

    /// <summary>
    /// Prende vari punti randomici per un totale di punti randomici totali ( massimo di 4 rimbalzi )
    /// </summary>

    public IEnumerator PickRandomicPoints(Vector2 endPosition, Vector2 direction)
    {
        int nPunti = Random.Range(1, 3);

        for (int i = 0; i < nPunti; i++)
        {
            RandomPoints.Add(endPosition + (direction * Random.Range(.4f, .8f)));
        }

        /*foreach (Vector2 item in RandomPoints)
        {
            Debug.Log($"RandomPoint: {item}");
        }*/

        yield return null;
    }
}
