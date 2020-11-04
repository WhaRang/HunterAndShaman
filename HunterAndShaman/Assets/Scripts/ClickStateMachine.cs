using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickStateMachine : MonoBehaviour
{
    public enum STAGE 
    {
        SPAWN_CARDS,
        DRAW_CARDS,
        DISCARD_CARDS,
    }

    public PlayersHand personsHand;
    public PlayersHand aiHand;

    //const float DELTA_PAUSE = 0.1f;

    float pause;
    float counter;

    bool canClick;
    STAGE curr_stage;

    private void Start()
    {
        pause = CardBehaviour.timeOfMoving; //+ DELTA_PAUSE;
        curr_stage = STAGE.SPAWN_CARDS;
        canClick = true;
        counter = 0.0f;
    }


    private void Update()
    {
        counter += Time.deltaTime;
        if (counter >= pause)
        {
            counter = 0.0f;
            canClick = true;
        }
    }


    public void OnClick()
    {
        if (!canClick)
        {
            return;
        }

        canClick = false;
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
                    curr_stage = STAGE.DISCARD_CARDS;
                    break;
                }

            case STAGE.DISCARD_CARDS:
                {
                    DiscardCardsForEveryone();
                    if (MainDeck.deck.HowManyCards() > 0)
                    {
                        curr_stage = STAGE.DRAW_CARDS;
                    }
                    else
                    {
                        curr_stage = STAGE.SPAWN_CARDS;
                    }
                    break;
                }
        }
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
        HorseDeck.deck.DiscardCard();
    }
}
