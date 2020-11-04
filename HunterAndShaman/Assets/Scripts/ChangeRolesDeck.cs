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

    private void Awake()
    {
        if (deck == null)
            deck = this.gameObject.GetComponent<ChangeRolesDeck>();
    }

    private void Start()
    {
        cards = new CardBehaviour[DECK_SIZE];
        SpawnDeck();
    }

    void SpawnDeck()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            CardBehaviour obj;
            obj = Instantiate(changeRolesCardPrefab);
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

        cards[DECK_SIZE - curr_card - 1].MoveCardTo(new Vector3(840.0f, 300.0f, 0.0f));
        cards[DECK_SIZE - curr_card - 1].FlipCard();

        curr_card++;
    }

    public int HowManyCards()
    {
        return DECK_SIZE - curr_card;
    }
}
