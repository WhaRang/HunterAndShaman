using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleActionManager : MonoBehaviour
{
    public static MiddleActionManager manager;

    const int TOTAL_MOVES = 3;
    int curr_move;

    bool isPersonMoved;
    bool isAiMoved;

    CardBehaviour personCard;
    CardBehaviour aiCard;

    HorseCard horseCard;

    Vector3 aiCardPos = new Vector3(0.0f, 350.0f, 0.0f);
    Vector3 personCardPos = new Vector3(0.0f, -350.0f, 0.0f);

    Vector3 flushPos = Vector3.zero;

    float positioningTime;


    private void Awake()
    {
        if (manager == null)
            manager = this.gameObject.GetComponent<MiddleActionManager>();
    }


    private void Start()
    {
        flushPos.x = -Screen.width;
        positioningTime = CardBehaviour.timeOfMoving / 2;
        isPersonMoved = false;
        isAiMoved = false;
        curr_move = 1;
    }


    public void MakeMove(bool isAi, CardBehaviour card)
    {
        if (isAi)
        {
            AiMove(card);
        }
        else
        {
            PersonMove(card);
        }
    }


    public void PersonMove(CardBehaviour card)
    {
        personCard = card;
        StartCoroutine(PersonMoveCoroutine());
    }


    IEnumerator PersonMoveCoroutine()
    {
        personCard.MoveCardTo(personCardPos, positioningTime);
        personCard.FlipCard(positioningTime);
        yield return new WaitForSeconds(positioningTime * 2);
        isPersonMoved = true;
        MoveWasMade();
    }


    public void AiMove(CardBehaviour card)
    {
        aiCard = card;
        StartCoroutine(AiMoveCoroutine());
    }


    IEnumerator AiMoveCoroutine()
    {
        aiCard.MoveCardTo(aiCardPos, positioningTime);
        yield return new WaitForSeconds(positioningTime * 2);
        isAiMoved = true;
        MoveWasMade();
    }


    void MoveWasMade()
    {
        if (isPersonMoved && isAiMoved)
        {
            PerformAction();
        }
    }


    void PerformAction()
    {
        StartCoroutine(ActionCoroutine());
    }


    IEnumerator ActionCoroutine()
    {
        aiCard.FlipCard(positioningTime);
        personCard.FlipCard(positioningTime);
        yield return new WaitForSeconds(positioningTime * 2);
        personCard.MakePulse();
        horseCard.AffectOnHorse(personCard);
        yield return new WaitForSeconds(positioningTime * 2);
        aiCard.MakePulse();
        horseCard.AffectOnHorse(aiCard);
        yield return new WaitForSeconds(positioningTime * 2);
        FlushCards();
        yield return new WaitForSeconds(positioningTime);
        LateAction();
        yield return new WaitForSeconds(positioningTime);
    }


    void FlushCards()
    {
        flushPos.y = aiCard.transform.position.y;
        aiCard.MoveCardTo(flushPos);

        flushPos.y = personCard.transform.position.y;
        personCard.MoveCardTo(flushPos);
    }


    void LateAction()
    {
        if (curr_move < TOTAL_MOVES)
        {
            curr_move++;
            GameStateMachine.machine.SetStage(GameStateMachine.STAGE.MOVES);
            GameStateMachine.machine.StartMove();
        }
        else
        {
            curr_move = 1;
            GameStateMachine.machine.StartMove();
        }
    }


    public void StartAction()
    {
        isPersonMoved = false;
        isAiMoved = false;
    }


    public void DrawHorseCard()
    {
        CardBehaviour tempCard =  HorseDeck.deck.DrawCard();
        horseCard = tempCard.gameObject.GetComponent<HorseCard>();
    }
}
