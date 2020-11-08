using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoundManager : MonoBehaviour
{
    public static EndRoundManager manager;

    public ScoreManager personScore;
    public ScoreManager aiScore;

    public Hero personHero;
    public Hero aiHero;

    public Vector3 personWinningPos;
    public Vector3 aiWinningPos;

    float timeOfManaging = CardBehaviour.timeOfMoving;

    const int SCORE_FOR_HORSE = 2;
    const int SCORE_FOR_CH_CARD = 1;


    private void Awake()
    {
        if (manager == null)
            manager = this.gameObject.GetComponent<EndRoundManager>();
    }


    public void ManageScore(CardBehaviour horseCard,
        CardBehaviour personChangeRoleCard, CardBehaviour aiChangeRoleCard)
    {
        bool isPersonWin;
        int horseHp = horseCard.GetComponent<HorseCard>().GetHelath();

        if (horseHp <= 0)
        {
            isPersonWin = personHero.heroType == Hero.HEROES.HUNTER ? true : false;
        } 
        else
        {
            isPersonWin = personHero.heroType == Hero.HEROES.SHAMAN ? true : false;
        }

        StartCoroutine(ManageScoreCoroutine(isPersonWin, horseCard, personChangeRoleCard, aiChangeRoleCard));
    }


    IEnumerator ManageScoreCoroutine(bool isPersonWin, CardBehaviour horseCard,
        CardBehaviour personChangeRoleCard, CardBehaviour aiChangeRoleCard)
    {
        StartCoroutine(HorseScoreManageCoroutine(isPersonWin, horseCard));
        yield return new WaitForSeconds(1.5f * timeOfManaging);

        if (personChangeRoleCard != null)
            StartCoroutine(ChangeRolesCardCoroutine(isPersonWin, personWinningPos, personChangeRoleCard));

        if (aiChangeRoleCard != null)
        {
            aiChangeRoleCard.FlipCard();
            StartCoroutine(ChangeRolesCardCoroutine(isPersonWin, aiWinningPos, aiChangeRoleCard));
        }

        if (personChangeRoleCard != null || aiChangeRoleCard != null)
            yield return new WaitForSeconds(timeOfManaging);

        GameStateMachine.machine.StartMove();
    }


    IEnumerator ChangeRolesCardCoroutine(bool isPersonWin, Vector3 pos, CardBehaviour card)
    {
        MovingAnimations.instance.MoveObjTo(card.gameObject, pos, timeOfManaging / 2);
        yield return new WaitForSeconds(timeOfManaging / 2);

        if (isPersonWin)
        {
            personScore.AddScore(SCORE_FOR_CH_CARD);
        }
        else
        {
            aiScore.AddScore(SCORE_FOR_CH_CARD);
        }
        card.MakePulse();
        yield return new WaitForSeconds(timeOfManaging);

        Vector3 endPos = card.gameObject.transform.position;
        endPos.x = -Screen.width;
        MovingAnimations.instance.MoveObjTo(card.gameObject, endPos, timeOfManaging);
    }


    IEnumerator HorseScoreManageCoroutine(bool isPersonWin, CardBehaviour horseCard)
    {
        Vector3 winningPos = isPersonWin ? personWinningPos : aiWinningPos;
        MovingAnimations.instance.MoveObjTo(horseCard.gameObject, winningPos, timeOfManaging / 2);
        yield return new WaitForSeconds(timeOfManaging / 2);

        if (isPersonWin)
        {
            personScore.AddScore(SCORE_FOR_HORSE);
        }
        else
        {
            aiScore.AddScore(SCORE_FOR_HORSE);
        }
        horseCard.MakePulse();
        yield return new WaitForSeconds(timeOfManaging);

        Vector3 horseEndPos = horseCard.gameObject.transform.position;
        horseEndPos.x = -Screen.width;
        MovingAnimations.instance.MoveObjTo(horseCard.gameObject, horseEndPos, timeOfManaging);
    }
}
