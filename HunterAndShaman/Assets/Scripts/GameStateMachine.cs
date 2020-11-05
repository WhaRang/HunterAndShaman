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
        DISCARD_CARDS,
    }

    public PlayersHand personsHand;
    public PlayersHand aiHand;

    const float DELTA_PAUSE = 0.5f;
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
        canMove = true;
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
                    curr_stage = STAGE.DISCARD_CARDS;
                    canMove = false;
                    break;
                }
            case STAGE.DISCARD_CARDS:
                {
                    if (curr_round < TOTAL_ROUNDS)
                    {
                        HorseDeck.deck.DiscardCurrentCard();
                        curr_round++;
                        curr_stage = STAGE.DRAW_CARDS;
                        break;
                    }
                    else
                    {
                        curr_round = 1;
                    }
                    DiscardCardsForEveryone();
                    curr_stage = STAGE.SPAWN_CARDS;
                    break;
                }
        }
    }


    void MakeMoves()
    {
        MiddleActionManager.manager.StartAction();
        aiHand.MakeAutoMove(true);
        personsHand.MakeAutoMove(false);
    }


    void MixCards()
    {
        aiHand.MixCards();
    }


    void SpawnCards()
    {
        HorseDeck.deck.SpawnDeck();
        MainDeck.deck.SpawnDeck();
        ChangeRolesDeck.deck.SpawnDeck();
    }


    void DrawCardsForEveryone()
    {
        personsHand.DrawCards();
        aiHand.DrawCards();
        HorseDeck.deck.DrawCard();
    }


    void DiscardCardsForEveryone()
    {
        personsHand.DiscardCards();
        aiHand.DiscardCards();
        HorseDeck.deck.DiscardCurrentCard();
        MainDeck.deck.DiscardAll();
        ChangeRolesDeck.deck.DiscardAll();
    }


    public void StartMove()
    {
        canMove = true;
    }


    public void SetStage(STAGE newStage)
    {
        curr_stage = newStage;
    }
}
