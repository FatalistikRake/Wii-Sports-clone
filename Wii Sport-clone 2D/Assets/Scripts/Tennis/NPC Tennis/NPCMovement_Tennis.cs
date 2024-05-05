using System.Collections;
using UnityEngine;

public class NPCMovement_Tennis : MonoBehaviour
{

    /// to-do list:
    ///                         /
    /// .9. settare il campo  \/
    /// 
    /// 1. riuscire a muovere l'npc verso la palla ( Passo numero UNO ahaha )
    /// 
    /// 2. Mettere la racchetta nella posizione giusta.
    /// 
    /// 3. far colpire la palla in un punto casuale dell'altro campo


    /// Usare NavMesh per far muovere il player

    private Vector2 npcMovement;

    public Vector2 NpcMovement { get => npcMovement; private set => npcMovement = value; }

    private Vector2 endPosition;

    /// <summary>
    /// La velocita dell npc
    /// </summary>
    private float moveSpeed = 1.2f;

    /// <summary>
    /// Controllo per vedere se npc si sta muovendo o si deve muovere
    /// </summary>
    public bool npcCanMoving;


    /// <summary>
    /// Il GameObject della palla
    /// </summary>
    private TennisBall ball;

    /// <summary>
    /// Il Rigidbody2D del NPC
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// L'Animator del NPC
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Il GameObject della racchetta
    /// </summary>
    public GameObject racket;


    /// <summary>
    /// Il GameObject del campo avversario
    /// </summary>
    public GameObject opponentPlayingField { get; private set; }

    /// <summary>
    /// Il GameObject del corrente campo
    /// </summary>
    public GameObject currentPlayingField { get; private set; }

    void Start()
    {
        if (FindFirstObjectByType<TennisBall>() == null)
        {
            Debug.Log("Ancora non c'è nessuna pallina in campo");
        }

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

        npcCanMoving = true;
    }

    void Update()
    {
        if(ball != FindFirstObjectByType<TennisBall>().gameObject && FindFirstObjectByType<TennisBall>() != null)
        {
            ball = FindFirstObjectByType<TennisBall>();
        }


        /// da rivedere questo pezzo di codice
        /// {

        // Calcola il vettore direzione tra observer e target
        Vector2 direction = (Vector2)(ball.transform.position - transform.position);

        // Calcola il prodotto vettoriale bidimensionale tra il vettore direzione della tua vista e il vettore direzione del target
        float crossProduct = direction.x * Vector2.up.y - direction.y * Vector2.up.x;

        // Controlla il segno del componente x del prodotto vettoriale
        float sign = Mathf.Sign(crossProduct);

        /// }


        if (PlayerBallInteraction.ballIsShooted && !NPCBallInteraction_Tennis.ballIsShooted && ball.ballIsInAir)
        {
            if (InsideOfTheField(ball.endPosition,
                    currentPlayingField.GetComponent<Collider2D>().bounds))
            {
                // destra
                if (sign > 0)
                {
                    endPosition = new(ball.endPosition.x - GetComponent<Renderer>().bounds.size.x / 2f - .0251f, ball.endPosition.y - .07f);
                }
                // sinistra
                else if (sign < 0)
                {
                    endPosition = new(ball.endPosition.x + GetComponent<Renderer>().bounds.size.x + .0251f, ball.endPosition.y - .07f);
                }

                transform.position = Vector2.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);

                npcMovement = new(sign, transform.position.y);

                animator.SetFloat("Horizontal", npcMovement.x);
                animator.SetFloat("Vertical", opponentPlayingField == FindFieldsManager_Tennis.bottomPlayingField ? -npcMovement.y : npcMovement.y);
                animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
            }
            /*else
            {
                if (Vector2.Distance((Vector2)transform.position, currentPlayingField.GetComponent<Collider2D>().bounds.center) > 0.01f)
                {
                    NpcGoToCenter();
                }
            }*/
        }

    }

    
    private void NpcGoToCenter()
    {
        Vector2 center = currentPlayingField.GetComponent<Collider2D>().bounds.center;

        if (ball.currentField != currentPlayingField)
        {
            if (transform.position != (Vector3)center)
            {
                StartCoroutine(MoveToCenter(center));
            }
            else
            {
                NpcNotMoving();
            }
        }
    }

    private IEnumerator MoveToCenter(Vector2 center)
    {
        while (Vector2.Distance((Vector2)transform.position, center) > 0.01f && ball.currentField == opponentPlayingField)
        {
            transform.position = Vector2.MoveTowards(transform.position, center, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }


    private void NpcNotMoving()
    {
        rb.velocity = Vector2.zero;

        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", opponentPlayingField == FindFieldsManager_Tennis.bottomPlayingField ? -1 : 1);
        animator.SetFloat("Speed", 0);
    }

    // per eseguire una sola volta il controllo del corrente campo
    private bool opponentPlayingFieldHasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (!opponentPlayingFieldHasTriggered)
            {
                if (collision.CompareTag("TopPlayingField"))
                {
                    opponentPlayingField = FindFieldsManager_Tennis.bottomPlayingField;
                    currentPlayingField = FindFieldsManager_Tennis.topPlayingField;

                    opponentPlayingFieldHasTriggered = true;
                }
                else if (collision.CompareTag("BottomPlayingField") && !opponentPlayingFieldHasTriggered)
                {
                    opponentPlayingField = FindFieldsManager_Tennis.topPlayingField;
                    currentPlayingField = FindFieldsManager_Tennis.bottomPlayingField;
                    
                    opponentPlayingFieldHasTriggered = true;

                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
            transform.position = Vector3.zero;
    }

    private bool InsideOfTheField(Vector2 endPosition, Bounds boundsField)
    {
        if (endPosition.x >= boundsField.min.x
         && endPosition.x <= boundsField.max.x
         && endPosition.y >= boundsField.min.y
         && endPosition.y <= boundsField.max.y)
        {
            return true;
        }

        return false;
    }
}
