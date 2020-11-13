using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundStateManager : MonoBehaviour
{
    public static RoundStateManager manager;

    public enum ROUND_STATES
    { 
        FIRST_ROUND,
        SECOND_ROUND,
        THIRD_ROUND,
        RESULT
    }

    public Banner bannerPrefab;

    float pauseTime;
    int curr_round;
    ROUND_STATES curr_state;

    int personRoundScore;
    int aiRoundScore;


    private void Awake()
    {
        if (manager == null)
            manager = this.gameObject.GetComponent<RoundStateManager>();
    }


    private void Start()
    {
        curr_round = 1;
        pauseTime = Banner.lifeTime;
        curr_state = ROUND_STATES.FIRST_ROUND;
        personRoundScore = 0;
        aiRoundScore = 0;
        ManageState();
    }


    public void ManageState()
    {
        switch(curr_state)
        {
            case ROUND_STATES.FIRST_ROUND:
                {
                    curr_state = ROUND_STATES.SECOND_ROUND;
                    ManageRound();
                    break;
                }

            case ROUND_STATES.SECOND_ROUND:
                {
                    curr_state = ROUND_STATES.THIRD_ROUND;
                    ManageRound();
                    break;
                }

            case ROUND_STATES.THIRD_ROUND:
                {
                    curr_state = ROUND_STATES.RESULT;
                    ManageRound();
                    break;
                }

            case ROUND_STATES.RESULT:
                {
                    curr_state = ROUND_STATES.FIRST_ROUND;
                    Result();
                    break;
                }
        }
    }


    void ManageRound()
    {
        if (!CheckIfNotTheEnd())
            StartCoroutine(ManageRoundCoroutine());  
    }


    IEnumerator ManageRoundCoroutine()
    {
        bannerPrefab.SetText("Round " + curr_round + "\n" + personRoundScore + " - " + aiRoundScore);
        Banner rndBanner = Instantiate(bannerPrefab);
        rndBanner.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        curr_round++;
        yield return new WaitForSeconds(pauseTime);
        GameStateMachine.machine.StartMove();
    }


    bool CheckIfNotTheEnd()
    {
        if (personRoundScore >= 2 || aiRoundScore >= 2)
        {
            curr_state = ROUND_STATES.RESULT;
            ManageState();
            return true;
        }

        return false;
    }



    void Result()
    {
        StartCoroutine(ResultCoroutine());
    }


    IEnumerator ResultCoroutine()
    {
        string additionalStr = GetAdditionalString();

        bannerPrefab.SetText(additionalStr + "\n" + personRoundScore + " - " + aiRoundScore);
        Banner obj = Instantiate(bannerPrefab);
        obj.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        yield return new WaitForSeconds(pauseTime);
        personRoundScore = 0;
        aiRoundScore = 0;
        curr_round = 0;
        ManageState();
    }


    string GetAdditionalString()
    {
        string additionalString = "";

        if (personRoundScore > aiRoundScore)
        {
            additionalString = "You win!";
        }

        if (aiRoundScore > personRoundScore)
        {
            additionalString = "You lose!";
        }

        if (aiRoundScore == personRoundScore)
        {
            additionalString = "Draw!";
        }

        return additionalString;
    }


    public void ProcessRoundScore(int personScore, int aiScore)
    {
        if (personScore > aiScore)
        {
            personRoundScore++;
        }

        if (aiScore > personScore)
        {
            aiRoundScore++;
        }

        if (aiScore == personScore)
        {
            aiRoundScore++;
            personRoundScore++;
        }
    }
}
