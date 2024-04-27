using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prova : MonoBehaviour
{
    public Transform racket;
    public Transform ball;

    void Start()
    {
        Debug.Log("distance: " + Vector2.Distance(racket.position, ball.position));
    }
}
