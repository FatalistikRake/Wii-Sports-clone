using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisBall : MonoBehaviour
{
    public BallShadow ballShadow { get; private set; }
    public BallBounce ballBounce { get; private set; }

    public bool ballIsMoving { get; private set; }

    public Vector2 startPosition { get; private set; }
    public Vector2 endPosition { get; private set; }

    public GameObject currentField { get; private set; }

    private Vector2 ballPosition;

    private float forceShoot;

    void Start()
    {
        ballShadow = GetComponent<BallShadow>();
        ballBounce = GetComponent<BallBounce>();

        if (ballShadow == null || ballBounce == null)
        {
            Debug.LogError("ballShadow o ballBounce non sono stati assegnati correttamente");
        }

        ballPosition = (Vector2)transform.position;

        startPosition = ballShadow.startPosition;
        endPosition = ballShadow.endPosition;

        forceShoot = ForcefullThower.ForceShot;

        ballIsMoving = ballShadow.ballIsInAir;
    }

    void Update()
    {
        if (ballPosition != (Vector2)transform.position)
            ballPosition = (Vector2)transform.position;

        if (startPosition != ballShadow.startPosition)
            startPosition = ballShadow.startPosition;

        if (endPosition != ballShadow.endPosition)
            endPosition = ballShadow.endPosition;

        if (forceShoot != ForcefullThower.ForceShot)
            forceShoot = ForcefullThower.ForceShot;

        if (ballIsMoving != ballShadow.ballIsInAir)
            ballIsMoving = ballShadow.ballIsInAir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("TopPlayingField") && currentField != FindFieldsManager_Tennis.topPlayingField)
            {
                currentField = FindFieldsManager_Tennis.topPlayingField;
            }
            else if (collision.CompareTag("BottomPlayingField"))
            {
                currentField = FindFieldsManager_Tennis.bottomPlayingField;
            }
        }
    }
}
