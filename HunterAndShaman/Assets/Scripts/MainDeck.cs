using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDeck : MonoBehaviour
{
    public static MainDeck deck;

    public CardBehaviour redCardPrefab;
    public CardBehaviour blackCardPrefab;

    const int DECK_SIZE = 24;
    int curr_card = 0;

    bool[] cardPlacement;
    CardBehaviour[] cards;

    Vector3 spawningPos = new Vector3(730.0f, -120.0f, 0.0f);


    private void Awake()
    {
        if (deck == null)
            deck = this.gameObject.GetComponent<MainDeck>();
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
                card = Instantiate(redCardPrefab);
            }
            else
            {
                card = Instantiate(blackCardPrefab);
            }
            card.transform.position = spawningPos;
            card.transform.rotation = transform.rotation;
            card.transform.SetParent(transform);

            card.MoveCardTo(transform.position);

            cards[i] = card;
        }
    }


    public CardBehaviour DrawCard()
    {
        if (curr_card >= DECK_SIZE)
        {
            return null;
        }

        curr_card++;
        return cards[DECK_SIZE - curr_card];
    }


    public int HowManyCards()
    {
        return DECK_SIZE - curr_card;
    }


    public void DiscardAll()
    {
        Vector3 descardingPos = transform.position;
        descardingPos.x = -Screen.width;

        for (int i = 0; i < DECK_SIZE - curr_card; i ++)
        {
            cards[i].MoveCardTo(descardingPos);
        }
    }
}
