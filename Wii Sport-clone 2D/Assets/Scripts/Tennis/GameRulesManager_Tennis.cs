using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class GameRulesManager_Tennis : MonoBehaviour
{
    /// <summary>
    ///                 Regole:
    /// 
    /// 1. **Punteggio**: Il tennis si gioca in set e partite. Un set è composto da game, e un game è composto da punti.
    /// I punti sono assegnati come segue: 15, 30, 40 e poi "gioco". Se entrambi i giocatori hanno raggiunto 40, si parla di "deuce". 
    /// Dopo il deuce, un giocatore deve guadagnare due punti consecutivi per vincere il game.

    /// 2. **Servizio**: Il servizio inizia ogni punto.
    /// Il giocatore deve colpire la palla da dietro la linea di servizio nella metà diagonale opposta del campo.

    /// 3. **Doppi fault**: Se il servizio non raggiunge il lato opposto del campo o tocca la rete,
    /// viene conteggiato come doppio fallo e il giocatore perde il punto.

    /// 4. **Fuori campo**: Se la palla tocca il terreno al di fuori dei bordi del campo, viene considerata fuori e il punto va all'avversario.

    /// 5. **Rete e doppio rimbalzo**: Se la palla tocca la rete ma cade ancora nel campo avversario, il punto viene giocato di nuovo.
    /// Se il servizio colpisce la rete ma cade nel campo avversario, viene conteggiato come fault.

    /// 6. **Vincere un game**: Per vincere un game, un giocatore deve vincere quattro punti e avere almeno due punti in più del suo avversario.
    /// Se entrambi i giocatori raggiungono tre punti (40-40), il gioco si trova in uno stato di "deuce".
    /// Il giocatore che vince il punto successivo dopo il deuce ottiene l'"avvantaggio".
    /// Se questo giocatore vince anche il punto successivo, vince il game. Se perde il punto, il gioco torna a "deuce".

    /// 7. **Vincere un set**: Per vincere un set, un giocatore deve vincere sei games, con un vantaggio di almeno due games.
    /// Se entrambi i giocatori arrivano a 6-6, si gioca un tie-break per decidere il set.

    /// 8. **Vincere una partita**: Per vincere una partita, un giocatore deve vincere un numero specifico di set, che dipende dalle regole del torneo.

    /// </summary>

    public enum TypeOfTraining
    {
        // Continuare dopo
    }

    public enum TypeOfGame
    {
        SingleCourt,
        DoubleCourt
    }



    [SerializeField]
    private TypeOfGame gameType;


    private TennisBall ball;

    public List<GameObject> team1, team2;


    public GameObject playerPrefab, NpcPrefab;

    public GameObject serviceBoxTopSx, serviceBoxTopDx;
    public GameObject serviceBoxBottomSx, serviceBoxBottomDx;

    private int Punteggio;

    void Start()
    {
        CinemachineVirtualCamera virtualCamera;

        virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();

        if (virtualCamera == null)
        {
            Debug.LogError("Cinemachine Virtual Camera non trovata.");
            return;
        }

        int serviceTeam = 1; // da 1 a 2 perchè 1 è team1 e 2 è team2
        int serviceSide = Random.Range(1, 3);// da 1 a 2 perchè 1 è serviceTeam....BoxSx e 2 è serviceBox....Dx

        if (gameType == TypeOfGame.SingleCourt)
        {
            team1.Add(playerPrefab = Instantiate(
                        playerPrefab,
                            serviceTeam == 1// Controllo per capire quale team deve servire
                                    ? 
                                    serviceSide == 1 ? // Controllo per capire quale lato deve servire
                                        serviceBoxBottomSx.transform.position : serviceBoxBottomDx.transform.position
                                    : // Posiziona il giocatore nell'area di servizio in basso a destra
                                FindFieldsManager_Tennis.bottomPlayingField.GetComponent<Collider2D>().bounds.min,
                        Quaternion.identity
                        ));

            virtualCamera.Follow = playerPrefab.transform;

            team2.Add(Instantiate(
                        NpcPrefab,
                            serviceTeam == 2 ? // Controllo per capire quale team deve servire
                                serviceSide == 2 ? // Controllo per capire quale lato deve servire
                                    serviceBoxTopSx.transform.position : // Posiziona il giocatore nell'area di servizio in basso a sinistra
                                    serviceBoxTopDx.transform.position : // Posiziona il giocatore nell'area di servizio in basso a destra
                             FindFieldsManager_Tennis.topPlayingField.GetComponent<Collider2D>().bounds.center,
                        Quaternion.identity
                        ));
        }
        else if (gameType == TypeOfGame.DoubleCourt)
        {
            team1.Add(Instantiate(playerPrefab));
            team1.Add(Instantiate(NpcPrefab));
            team2.Add(Instantiate(NpcPrefab));
            team2.Add(Instantiate(NpcPrefab));
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (ball != FindFirstObjectByType<TennisBall>().gameObject && FindFirstObjectByType<TennisBall>() != null)
        {
            ball = FindFirstObjectByType<TennisBall>();
        }


    }
}
