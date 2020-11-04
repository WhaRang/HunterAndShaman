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


    private void Awake()
    {
        if (deck == null)
            deck = this.gameObject.GetComponent<HorseDeck>();
    }


    private void Start()
    {
        cardPlacement = new bool[DECK_SIZE];
        cards = new CardBehaviour[DECK_SIZE];
        ShuffleDeck();
        SpawnDeck();
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

    void SpawnDeck()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            CardBehaviour obj;
            if (cardPlacement[i])
            {
                obj = Instantiate(redHorseCardPrefab);
            }
            else
            {
                obj = Instantiate(blackHorseCardPrefab);
            }
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.transform.SetParent(transform);

            cards[i] = obj;
        }
    }


    public void DrawCard()
    {
        if (curr_card >= DECK_SIZE)
        {
            return;
        }

        cards[DECK_SIZE - curr_card - 1].MoveCardTo(new Vector3(540.0f, 1080.0f, 0.0f));
        cards[DECK_SIZE - curr_card - 1].FlipCard();

        curr_card++;
    }


    public int HowManyCards()
    {
        return DECK_SIZE - curr_card;
    }
}
