using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    private Vector2 lastVelocity;
    private Rigidbody2D rb;

    public List<Vector2> RandomPoints;


    public GameObject targetPrefab;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        else
        {
            Debug.Log("ci sono 0 punti successivi");
        }

        foreach (var item in RandomPoints)
        {
            Debug.Log($"RandomPoint: {item}");
        }
    }

    public void Bounce(float reduceForce)
    {
        gameObject.GetComponent<Rigidbody2D>().velocity /= reduceForce;
    }


    // Per rimbalzare nei oggetti tipo i muri
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var speed = lastVelocity.magnitude / .4f;
        var directionBounce = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        Debug.Log("Direction Bounce: " + directionBounce);

        rb.AddForce(directionBounce * Mathf.Max(speed, 0f));
        gameObject.GetComponent<BallShadow>().ResetShadow();
    }

    /// <summary>
    /// Qui creare un Corutine che dovrà partire nell BallShadow ( per efficenza )
    /// 
    /// in cui prende vari punti randomici per un totale di punti randomici totali ( massimo di 4 rimbalzi )
    /// e se la palla arriva al muro si ferma il tutto ed esce dal for
    /// </summary>

    public IEnumerator PickRandomicPoints(Vector2 endPosition, Vector2 direction)
    {
        for (int i = Random.Range(1, 3) - 1; i >= 0; i--)
        {
            RandomPoints.Add(endPosition + direction * Random.Range(1f, 1.5f));
        }

        foreach (Vector2 item in RandomPoints)
        {
            Debug.Log($"RandomPoint: {item}");
        }

        yield return null;
    }
}
