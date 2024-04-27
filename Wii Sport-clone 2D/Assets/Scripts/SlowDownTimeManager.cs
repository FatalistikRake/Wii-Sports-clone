using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownTimeManager : MonoBehaviour
{
    public bool isPaused;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    void Start()
    {
        isPaused = false;
    }
    private void Update()
    {
        if( Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            isPaused = true;
            StartCoroutine(SlowMotion(true));
        }
    }
     
    public void StopTimeImmediately(bool isPaused)
    {
        if(isPaused)
        {
            // Ferma il tempo
            Time.timeScale = 0;
        }
        else
        {
            // Riavvia il tempo
            Time.timeScale = 1;
        }
    }

    public IEnumerator SlowMotion(bool isInSlowMotion)
    {
        if (isInSlowMotion)
        {
            while (Time.timeScale > 0.4f)
            {
                Time.timeScale -= 0.01f;

                yield return null;
            }
        }
        else
        {
            // Increase the time scale back to normal
            while (Time.timeScale < 1)
            {
                Time.timeScale += 0.1f * 1;
                yield return null;
            }
        }
        isPaused = false;
    }
}
