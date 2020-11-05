using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRolesDeck : MonoBehaviour
{
    public static ChangeRolesDeck deck;
    public CardBehaviour changeRolesCardPrefab;

    const int DECK_SIZE = 8;
    int curr_card = 0;

    CardBehaviour[] cards;

    Vector3 spawningPos = new Vector3(730.0f, 120.0f, 0.0f);

    private void Awake()
    {
        if (deck == null)
            deck = this.gameObject.GetComponent<ChangeRolesDeck>();
    }

    private void Start()
    {
        cards = new CardBehaviour[DECK_SIZE];
    }

    public void SpawnDeck()
    {
        curr_card = 0;

        for (int i = 0; i < cards.Length; i++)
        {
            CardBehaviour card;
            card = Instantiate(changeRolesCardPrefab);
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

        for (int i = 0; i < DECK_SIZE - curr_card; i++)
        {
            cards[i].MoveCardTo(descardingPos);
        }
    }
}
