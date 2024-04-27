using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FindFieldsManager_Tennis : MonoBehaviour
{
    public static GameObject topPlayingField { get; private set; }
    public static GameObject bottomPlayingField { get; private set; }


    private void Awake()
    {
        topPlayingField = FindFirstObjectByType<TopPlayingField_Tennis>().gameObject;
        bottomPlayingField = FindFirstObjectByType<BottomPlayingField_Tennis>().gameObject;

        if (topPlayingField == null || bottomPlayingField == null)
        {
            Debug.LogError("Errore nel trovare i due campi");
        }
    }
}
