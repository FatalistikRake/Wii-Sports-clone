using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTennis : MonoBehaviour
{
    // Velocità di movimento del giocatore
    public float moveSpeed = 5f;

    // Riferimento al componente Rigidbody2D del giocatore
    private Rigidbody2D rb;

    // Ultimo movimento effettuato dal giocatore
    private Vector2 lastMovement;

    // In quale campo si trova lo spara palle
    private GameObject opponentPlayingField;



    [HideInInspector]
    public Vector2 PlayerInput;

    // Riferimento all'Animator del giocatore
    [HideInInspector]
    public Animator animator;

    // Flag per controllare se il giocatore può muoversi
    [HideInInspector]
    private bool playerCanMove = true;

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
        if (!playerCanMove)
            return;

        if (Input.GetKey(KeyCode.LeftShift)) { moveSpeed = 2.7f; } else { moveSpeed = 1.2f; }

        // Creazione del vettore di movimento normalizzato
        PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Se il giocatore si sta muovendo
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
            // Se il giocatore non si sta muovendo, arresta il Rigidbody2D
            rb.velocity = Vector2.zero;

            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", opponentPlayingField == FindFieldsManager_Tennis.bottomPlayingField ? -1 : 1);
            animator.SetFloat("Speed", 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("TopPlayingField") && opponentPlayingField != FindFieldsManager_Tennis.bottomPlayingField)
            {
                opponentPlayingField = FindFieldsManager_Tennis.bottomPlayingField;
            }
            else if (collision.CompareTag("BottomPlayingField") && opponentPlayingField != FindFieldsManager_Tennis.topPlayingField)
            {
                opponentPlayingField = FindFieldsManager_Tennis.topPlayingField;
            }
        }
    }
}
