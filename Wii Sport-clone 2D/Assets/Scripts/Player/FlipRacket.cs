using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipRacket : MonoBehaviour
{
    private Vector2 leftHand = new(-0.1139f, 0.0186f), RightHand = new(0.1139f, 0.0186f);
    public GameObject Racket;

    private float x_directionRacket;

    private void Start()
    {
        if (Racket == null)
            Debug.LogError("Il prefab della raccheta non è stato impostato");

        if (GetComponent<PlayerMovementTennis>() != null)
            x_directionRacket = GetComponent<PlayerMovementTennis>().PlayerInput.x;
        else if (GetComponent<NPCMovement_Tennis>() != null)
            x_directionRacket = GetComponent<NPCMovement_Tennis>().NpcMovement.x;
        else
            Debug.LogError("Non e stato impostato corretamente un tipo di movimento");
    }

    void Update()
    {
        if (GetComponent<PlayerMovementTennis>() != null && x_directionRacket != GetComponent<PlayerMovementTennis>().PlayerInput.x)
            x_directionRacket = GetComponent<PlayerMovementTennis>().PlayerInput.x;
        else if (GetComponent<NPCMovement_Tennis>() != null && x_directionRacket != GetComponent<NPCMovement_Tennis>().NpcMovement.x)
            x_directionRacket = GetComponent<NPCMovement_Tennis>().NpcMovement.x;

        if (x_directionRacket > 0)
        {
            Racket.transform.localPosition = RightHand;
            if(Racket.transform.localScale.x > 0)
                Racket.transform.localScale = new(-Racket.transform.localScale.x, Racket.transform.localScale.y);
        }
        else if (x_directionRacket < 0)
        {
            Racket.transform.localPosition = leftHand;
            if (Racket.transform.localScale.x < 0)
                Racket.transform.localScale = new (Mathf.Abs(Racket.transform.localScale.x), Racket.transform.localScale.y);
        }
    }
}
