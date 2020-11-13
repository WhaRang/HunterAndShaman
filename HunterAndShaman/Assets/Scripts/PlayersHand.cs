using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersHand : MonoBehaviour
{
    Vector3 firstCardPos = new Vector3(-375.0f, 0.0f, 0.0f);
    Vector3 secondCardPos = new Vector3(-125.0f, 0.0f, 0.0f);
    Vector3 thirdCardPos = new Vector3(125.0f, -0.0f, 0.0f);

    Vector3 changeRolesCardPos = new Vector3(375.0f, 0.0f, 0.0f);

    Vector3[] availablePositions;

    CardBehaviour firstCard;
    CardBehaviour secondCard;
    CardBehaviour thirdCard;
    CardBehaviour changeRolesCard;

    CardBehaviour cardToMove;

    public bool shouldFlip;

    const float STD_DIFF = 20.0f;
    bool shouldWaitForChoosedCard;


    private void Start()
    {
        shouldWaitForChoosedCard = false;
        SetCardPositionsY();
    }


    private void Update()
    {
        if (shouldWaitForChoosedCard)
        {
            cardToMove = WaitForChoosedCard();
            if (cardToMove != null)
            {
                SetAllCardsClickableTo(false);
                cardToMove.SetChoosed(false);

                MiddleActionManager.manager.MakeMove(false, cardToMove);
                shouldWaitForChoosedCard = false;
            }
        }
    }


    public void DrawCards()
    {
        if (AllIsNull())
            DrawCardsOneByOne();
        else
            DrawCardsNeeded();
        InitializeAvailablePositions();

        if (shouldFlip)
        {
            FlipCardsOneByOne();
        }
        else
        {
            RotateCardsTowardsPlayer();
        }
    }


    public void DiscardCards()
    {
        Vector3 discardingPos = Vector3.zero;
        discardingPos.x = -Screen.width;
        discardingPos.y = transform.position.y;

        if (firstCard != null)
            firstCard.MoveCardTo(discardingPos);
        if (secondCard != null)
            secondCard.MoveCardTo(discardingPos);
        if (thirdCard != null)
            thirdCard.MoveCardTo(discardingPos);

        if (changeRolesCard != null)
            changeRolesCard.MoveCardTo(discardingPos);
    }


    public void MakeMove()
    {
        SetAllCardsClickableTo(true);
        shouldWaitForChoosedCard = true;
    }


    public void MakeAutoMove(bool isAi)
    {
        int indexOfChoosedCard = Random.Range(0, availablePositions.Length);

        CardBehaviour choosedCard = FindCardByIndex(indexOfChoosedCard);
        MiddleActionManager.manager.MakeMove(isAi, choosedCard);

        RemovePositionAtIndex(indexOfChoosedCard);
    }


    public void MixCards()
    {
        StartCoroutine(MixCardsCoroutine());
    }


    IEnumerator MixCardsCoroutine()
    {
        Vector3 centerPos = Vector3.zero;
        centerPos.y = transform.position.y;

        MoveAllCardsTo(centerPos);

        Vector3[] positions = new Vector3[4];
        positions[0] = firstCardPos;
        positions[1] = secondCardPos;
        positions[2] = thirdCardPos;
        positions[3] = changeRolesCardPos;

        System.Random rnd = new System.Random();
        Vector3[] randomPositions = positions.OrderBy(x => rnd.Next()).ToArray();

        yield return new WaitForSeconds(CardBehaviour.timeOfMoving / 2);

        MoveAllCardsToSpecifiedPositions(randomPositions);
    }


    void RemovePositionAtIndex(int indexToRemove)
    {
        int oldLength = availablePositions.Length;
        Vector3[] newAvailablePosotions = new Vector3[oldLength - 1];

        int newI = 0;
        for (int i = 0; i < availablePositions.Length; i++)
        {
            if (i != indexToRemove)
            {
                newAvailablePosotions[newI] = availablePositions[i];
                newI++;
            }
        }

        availablePositions = new Vector3[oldLength - 1];
        availablePositions = newAvailablePosotions;
    }


    CardBehaviour FindCardByIndex(int index)
    {
        float targetX = availablePositions[index].x;

        if (firstCard != null && nearlyEqual(firstCard.transform.position.x, targetX, STD_DIFF))
        {
            return firstCard;
        }

        if (secondCard != null && nearlyEqual(secondCard.transform.position.x, targetX, STD_DIFF))
        {
            return secondCard;
        }

        if (thirdCard != null && nearlyEqual(thirdCard.transform.position.x, targetX, STD_DIFF))
        {
            return thirdCard;
        }

        if (changeRolesCard != null && nearlyEqual(changeRolesCard.transform.position.x, targetX, STD_DIFF))
        {
            return changeRolesCard;
        }

        return null;
    }


    void MoveAllCardsToSpecifiedPositions(Vector3[] positions)
    {
        firstCard.MoveCardTo(positions[0], CardBehaviour.timeOfMoving / 2);
        secondCard.MoveCardTo(positions[1], CardBehaviour.timeOfMoving / 2);
        thirdCard.MoveCardTo(positions[2], CardBehaviour.timeOfMoving / 2);
        changeRolesCard.MoveCardTo(positions[3], CardBehaviour.timeOfMoving / 2);
    }


    void MoveAllCardsTo(Vector3 newPos)
    {
        firstCard.MoveCardTo(newPos, CardBehaviour.timeOfMoving / 2);
        secondCard.MoveCardTo(newPos, CardBehaviour.timeOfMoving / 2);
        thirdCard.MoveCardTo(newPos, CardBehaviour.timeOfMoving / 2);
        changeRolesCard.MoveCardTo(newPos, CardBehaviour.timeOfMoving / 2);
    }


    void SetCardPositionsY()
    {
        firstCardPos.y = transform.position.y;
        secondCardPos.y = transform.position.y;
        thirdCardPos.y = transform.position.y;
        changeRolesCardPos.y = transform.position.y;
    }


    void DrawCardsNeeded()
    {
        if (secondCard != null)
        {
            firstCard = secondCard;
        }

        if (thirdCard != null)
        {
            firstCard = thirdCard;
        }

        firstCard.MoveCardTo(firstCardPos);

        secondCard = MainDeck.deck.DrawCard();
        secondCard.MoveCardTo(secondCardPos);

        thirdCard = MainDeck.deck.DrawCard();
        thirdCard.MoveCardTo(thirdCardPos);

        changeRolesCard = ChangeRolesDeck.deck.DrawCard();
        changeRolesCard.MoveCardTo(changeRolesCardPos);
    }


    void DrawCardsOneByOne()
    {
        if (firstCard == null)
        {
            firstCard = MainDeck.deck.DrawCard();
            firstCard.MoveCardTo(firstCardPos);
        }
        if (secondCard == null)
        {
            secondCard = MainDeck.deck.DrawCard();
            secondCard.MoveCardTo(secondCardPos);
        }
        if (thirdCard == null)
        {
            thirdCard = MainDeck.deck.DrawCard();
            thirdCard.MoveCardTo(thirdCardPos);
        }
        if (changeRolesCard == null)
        {
            changeRolesCard = ChangeRolesDeck.deck.DrawCard();
            changeRolesCard.MoveCardTo(changeRolesCardPos);
        }
    }


    void FlipCardsOneByOne()
    {
        if (!firstCard.IsFlipped())
            firstCard.FlipCard();
        if (!secondCard.IsFlipped())
            secondCard.FlipCard();
        if (!thirdCard.IsFlipped())
            thirdCard.FlipCard();
        if (!changeRolesCard.IsFlipped())
            changeRolesCard.FlipCard();
    }


    void RotateCardsTowardsPlayer()
    {
        firstCard.RotateTo(transform.rotation);
        secondCard.RotateTo(transform.rotation);
        thirdCard.RotateTo(transform.rotation);
        changeRolesCard.RotateTo(transform.rotation);
    }


    void InitializeAvailablePositions()
    {
        availablePositions = new Vector3[4];
        availablePositions[0] = firstCardPos;
        availablePositions[1] = secondCardPos;
        availablePositions[2] = thirdCardPos;
        availablePositions[3] = changeRolesCardPos;
    }

    public static bool nearlyEqual(float a, float b, float epsilon)
    {
        float diff = System.Math.Abs(a - b);

        if (a == b)
        { 
            return true;
        }
        else
        { 
            return diff < epsilon;
        }
    }


    void SetAllCardsClickableTo(bool clickable)
    {
        if (firstCard != null)
            firstCard.SetClickable(clickable);
        if (secondCard != null)
            secondCard.SetClickable(clickable);
        if (thirdCard != null)
            thirdCard.SetClickable(clickable);
        if (changeRolesCard != null)
            changeRolesCard.SetClickable(clickable);
    }


    CardBehaviour WaitForChoosedCard()
    {           
        if (firstCard != null && firstCard.IsChoosed() == true)        
            return firstCard;            
        if (secondCard != null && secondCard.IsChoosed() == true)        
            return secondCard;            
        if (thirdCard != null && thirdCard.IsChoosed() == true)        
            return thirdCard;            
        if (changeRolesCard != null && changeRolesCard.IsChoosed() == true)        
            return changeRolesCard;

        return null;
    }


    public CardBehaviour GetChangeRolesCard()
    {
        return changeRolesCard;
    }


    bool AllIsNull()
    {
        return (firstCard == null) &&
               (secondCard == null) &&
               (thirdCard == null) &&
               (changeRolesCard == null);
    }
}
