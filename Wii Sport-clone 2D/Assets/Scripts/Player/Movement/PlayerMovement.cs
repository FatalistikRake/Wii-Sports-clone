using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Velocità di movimento del giocatore
    public float moveSpeed = 5f;

    // Riferimento al componente Rigidbody2D del giocatore
    private Rigidbody2D rb;

    // Riferimento all'Animator del giocatore
    [HideInInspector]
    public Animator animator;

    public Vector2 PlayerInput;


    // Ultimo movimento effettuato dal giocatore
    private Vector2 lastMovement;

    // Flag per controllare se il giocatore può muoversi
    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        // Ottenimento del componente Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // Verifica se il componente Rigidbody2D è stato ottenuto correttamente
        if (!TryGetComponent(out rb))
        {
            Debug.LogWarning("Il componente Rigidbody2D non riesco a prenderlo");
        }

        // Ottenimento del componente Animator
        animator = GetComponent<Animator>();

        // Verifica se il componente Animator è stato ottenuto correttamente
        if (!TryGetComponent(out animator))
        {
            Debug.LogWarning("Il componente Animator non riesco a prenderlo");
        }
    }

    private void Update()
    {
        // Controlla se il giocatore può muoversi
        if (!canMove)
            return;

        // Creazione del vettore di movimento normalizzato
        PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Se il tempo ancora non e fermo e il giocatore si sta muovendo
        if (PlayerInput != Vector2.zero)
        {
            lastMovement = PlayerInput;

            // Imposta la velocità del Rigidbody2D e aggiorna i parametri dell'Animator
            rb.velocity = PlayerInput * moveSpeed;

            animator.SetFloat("Horizontal", PlayerInput.x);
            animator.SetFloat("Vertical", PlayerInput.y);
            animator.SetFloat("Speed", 1);
        }
        else
        {
            rb.velocity = Vector2.zero;

            // Aggiorna i parametri dell'Animator con l'ultimo movimento
            animator.SetFloat("Horizontal", lastMovement.x);
            animator.SetFloat("Vertical", lastMovement.y);
            animator.SetFloat("Speed", 0);
        }
    }
}
