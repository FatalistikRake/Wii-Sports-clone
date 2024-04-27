using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TopPlayingField_Tennis : MonoBehaviour
{
    public GameObject[] topPartsOfCamp;

    private void Start()
    {
        if (topPartsOfCamp.Length < 5)
        {
            // Itera attraverso tutti i GameObject nell'array lowerPartsOfCamp
            foreach (GameObject child in topPartsOfCamp)
            {
                // Distruggi immediatamente il GameObject figlio
                DestroyImmediate(child);
            }

            // Ottieni tutti i GameObject figlio e salvali direttamente in lowerPartsOfCamp
            topPartsOfCamp = gameObject.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
        }
    }
}
