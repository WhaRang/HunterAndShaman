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
        StartCoroutine(ManageRoundCoroutine());  
    }


    IEnumerator ManageRoundCoroutine()
    {
        bannerPrefab.SetText("Round " + curr_round);
        Banner obj = Instantiate(bannerPrefab);
        obj.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        curr_round++;
        yield return new WaitForSeconds(pauseTime);
        GameStateMachine.machine.StartMove();
    }


    void Result()
    {
        StartCoroutine(ResultCoroutine());
    }


    IEnumerator ResultCoroutine()
    {
        bannerPrefab.SetText("Good game!");
        Banner obj = Instantiate(bannerPrefab);
        obj.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        yield return new WaitForSeconds(pauseTime);
    }
}
