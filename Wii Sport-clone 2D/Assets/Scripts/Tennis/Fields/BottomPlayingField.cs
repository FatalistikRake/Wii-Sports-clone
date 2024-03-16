using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BottomPlayingField : MonoBehaviour
{
    public GameObject[] lowerPartsOfCamp;

    private void Start()
    {
        if (lowerPartsOfCamp.Length < 5)
        {
            // Itera attraverso tutti i GameObject nell'array lowerPartsOfCamp
            foreach (GameObject child in lowerPartsOfCamp)
            {
                // Distruggi immediatamente il GameObject figlio
                DestroyImmediate(child);
            }

            // Ottieni tutti i GameObject figlio e salvali direttamente in lowerPartsOfCamp
            lowerPartsOfCamp = gameObject.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
        }
    }
}
