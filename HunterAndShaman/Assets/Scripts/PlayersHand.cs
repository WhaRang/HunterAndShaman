using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersHand : MonoBehaviour
{
    Vector3 firstCardPos = new Vector3(-375.0f, 0.0f, 0.0f);
    Vector3 secondCardPos = new Vector3(-125.0f, 0.0f, 0.0f);
    Vector3 thirdCardPos = new Vector3(125.0f, -0.0f, 0.0f);

    Vector3 changeRolesCardPos = new Vector3(375.0f, 0.0f, 0.0f);

    CardBehaviour firstCard;
    CardBehaviour secondCard;
    CardBehaviour thirdCard;

    CardBehaviour changeRolesCard;

    public bool shouldFlip;


    private void Start()
    {
        firstCardPos.y = transform.position.y;
        secondCardPos.y = transform.position.y;
        thirdCardPos.y = transform.position.y;
        changeRolesCardPos.y = transform.position.y;
    }


    public void DrawCards()
    {
        firstCard = MainDeck.deck.DrawCard(firstCardPos);
        secondCard = MainDeck.deck.DrawCard(secondCardPos);
        thirdCard = MainDeck.deck.DrawCard(thirdCardPos);

        changeRolesCard = ChangeRolesDeck.deck.DrawCard(changeRolesCardPos);

        if (firstCard == null || secondCard == null ||
            thirdCard == null || changeRolesCard == null)
        {
            return;
        }

        if (shouldFlip)
        {
            firstCard.FlipCard();
            secondCard.FlipCard();
            thirdCard.FlipCard();

            changeRolesCard.FlipCard();
        }
        else
        {
            firstCard.RotateTo(transform.rotation);
            secondCard.RotateTo(transform.rotation);
            thirdCard.RotateTo(transform.rotation);

            changeRolesCard.RotateTo(transform.rotation);
        }
    }


    public void DiscardCards()
    {
        if (firstCard == null || secondCard == null ||
            thirdCard == null || changeRolesCard == null)
        {
            return;
        }

        Vector3 discardingPos = Vector3.zero;
        discardingPos.x = -Screen.width;
        discardingPos.y = transform.position.y;

        firstCard.MoveCardTo(discardingPos);
        secondCard.MoveCardTo(discardingPos);
        thirdCard.MoveCardTo(discardingPos);

        changeRolesCard.MoveCardTo(discardingPos);
    }
}
