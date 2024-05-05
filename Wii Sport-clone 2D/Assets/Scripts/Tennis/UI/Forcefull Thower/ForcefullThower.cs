using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForcefullThower : MonoBehaviour
{
    public Gradient gradient;
    public Image fill;

    public static float ForceShot { get; private set; }

    private void Start()
    {
        fill.fillAmount = 0;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            fill.fillAmount += .04f;
            fill.color = gradient.Evaluate(fill.fillAmount);
            ForceShot = fill.fillAmount * 15;
            // Torare il numero          ---- giusto ( equilibrato )
            // Debug.Log($"force shot: {ForceShot}");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            fill.fillAmount = 0;
            fill.color = gradient.Evaluate(fill.fillAmount);
        }
    }
}
