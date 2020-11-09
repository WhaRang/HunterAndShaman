using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public static GameStateMachine machine;

    public enum STAGE 
    {
        SPAWN_CARDS,
        DRAW_CARDS,
        MIX_CARDS,
        MOVES,
        END_ROUND,
        DISCARD_CARDS,
    }

    public PlayersHand personsHand;
    public PlayersHand aiHand; 

    public ScoreManager personScore;
    public ScoreManager aiScore;

    const float DELTA_PAUSE = 0.2f;
    const int TOTAL_ROUNDS = 4;

    int curr_round = 1;

    float pause;
    float counter;

    float singlePause;
    float doublePause;

    STAGE curr_stage;

    bool canMove;


    private void Awake()
    {
        if (machine == null)
            machine = this.gameObject.GetComponent<GameStateMachine>();
    }


    private void Start()
    {
        singlePause = CardBehaviour.timeOfMoving + DELTA_PAUSE;
        doublePause = CardBehaviour.timeOfMoving + DELTA_PAUSE;
        pause = singlePause;
        curr_stage = STAGE.SPAWN_CARDS;
        counter = pause;
        canMove = false;
    }


    private void Update()
    {
        counter += Time.deltaTime;
        if (counter >= pause)
        {
            counter = 0.0f;
            ManageStage();
        }
    }


    public void ManageStage()
    {
        if (!canMove)
        {
            return;
        }

        counter = 0.0f;

        switch (curr_stage)
        {
            case STAGE.SPAWN_CARDS:
                {
                    SpawnCards();
                    curr_stage = STAGE.DRAW_CARDS;
                    break;
                }
            case STAGE.DRAW_CARDS:
                {
                    DrawCardsForEveryone();
                    curr_stage = STAGE.MIX_CARDS;
                    break;
                }
            case STAGE.MIX_CARDS:
                {
                    MixCards();
                    curr_stage = STAGE.MOVES;
                    break;
                }
            case STAGE.MOVES:
                {
                    MakeMoves();                    
                    curr_stage = STAGE.END_ROUND;
                    canMove = false;
                    break;
                }
            case STAGE.END_ROUND:
                {
                    EndRound();
                    curr_stage = STAGE.DISCARD_CARDS;
                    canMove = false;
                    break;
                }
            case STAGE.DISCARD_CARDS:
                {
                    if (curr_round < TOTAL_ROUNDS)
                    {
                        curr_round++;
                        curr_stage = STAGE.DRAW_CARDS;
                        break;
                    }
                    else
                    {
                        curr_round = 1;
                    }
                    DiscardCardsForEveryone();
                    RoundStateManager.manager.ProcessRoundScore(personScore.GetScore(), aiScore.GetScore());
                    RoundStateManager.manager.ManageState();
                    curr_stage = STAGE.SPAWN_CARDS;
                    canMove = false;
                    break;
                }
        }
    }


    void EndRound()
    {
        CardBehaviour horseCard =
            MiddleActionManager.manager.GetHorse().GetComponent<CardBehaviour>();
        EndRaffleManager.manager.ManageScore(horseCard,
            personsHand.GetChangeRolesCard(), aiHand.GetChangeRolesCard());
    }


    void MakeMoves()
    {
        MiddleActionManager.manager.StartAction();
        aiHand.MakeAutoMove(true);
        personsHand.MakeMove();
    }


    void MixCards()
    {
        aiHand.MixCards();
    }


    void SpawnCards()
    {
        personScore.ZeroScore();
        aiScore.ZeroScore();
        HorseDeck.deck.SpawnDeck();
        MainDeck.deck.SpawnDeck();
        ChangeRolesDeck.deck.SpawnDeck();
    }


    void DrawCardsForEveryone()
    {
        personsHand.DrawCards();
        aiHand.DrawCards();
        MiddleActionManager.manager.DrawHorseCard();
    }


    void DiscardCardsForEveryone()
    {
        personsHand.DiscardCards();
        aiHand.DiscardCards();
        MainDeck.deck.DiscardAll();
        ChangeRolesDeck.deck.DiscardAll();
    }


    public void StartMove()
    {
        canMove = true;
    }


    public void SkipPause()
    {
        counter = pause;
    }


    public void SetStage(STAGE newStage)
    {
        curr_stage = newStage;
    }
}
