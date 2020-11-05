using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseDeck : MonoBehaviour
{
    public static HorseDeck deck;
    public CardBehaviour redHorseCardPrefab;
    public CardBehaviour blackHorseCardPrefab;

    const int DECK_SIZE = 4;
    int curr_card = 0;

    CardBehaviour[] cards;
    bool[] cardPlacement;

    Vector3 spawningPos = new Vector3(-730.0f, 0.0f, 0.0f);


    private void Awake()
    {
        if (deck == null)
            deck = this.gameObject.GetComponent<HorseDeck>();
    }


    private void Start()
    {
        cardPlacement = new bool[DECK_SIZE];
        cards = new CardBehaviour[DECK_SIZE];
    }


    void ShuffleDeck()
    {
        for (int i = 0; i < cardPlacement.Length; i++)
        {
            cardPlacement[i] = false;
        }

        System.Random r = new System.Random();
        int cardsLeft = DECK_SIZE / 2;
        while (cardsLeft != 0)
        {
            int currCard = r.Next(0, DECK_SIZE);
            if (!cardPlacement[currCard])
            {
                cardPlacement[currCard] = true;
                cardsLeft--;
            }
        }
    }


    public void SpawnDeck()
    {
        curr_card = 0;
        ShuffleDeck();

        for (int i = 0; i < cards.Length; i++)
        {
            CardBehaviour card;
            if (cardPlacement[i])
            {
                card = Instantiate(redHorseCardPrefab);
            }
            else
            {
                card = Instantiate(blackHorseCardPrefab);
            }
            card.transform.position = spawningPos;
            card.transform.rotation = transform.rotation;
            card.transform.SetParent(transform);

            card.MoveCardTo(transform.position);

            cards[i] = card;
        }
    }


    public void DrawCard()
    {
        if (curr_card >= DECK_SIZE)
        {
            return;
        }

        cards[DECK_SIZE - curr_card - 1].MoveCardTo(Vector3.zero);
        cards[DECK_SIZE - curr_card - 1].FlipCard();

        curr_card++;
    }


    public void DiscardCurrentCard()
    {
        if (curr_card > DECK_SIZE)
        {
            return;
        }


        Vector3 newPos = Vector3.zero;
        newPos.x = -Screen.width;
        newPos.y = transform.position.y;

        cards[DECK_SIZE - curr_card].MoveCardTo(newPos);
    }


    public int HowManyCards()
    {
        return DECK_SIZE - curr_card;
    }
}
