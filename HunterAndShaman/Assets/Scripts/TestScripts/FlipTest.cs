using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipTest : MonoBehaviour
{
    public CardBehaviour card;

    float pause;
    float counter;

    private void Start()
    {
        counter = 0.0f;
        pause = CardBehaviour.timeOfMoving / 2;
    }

    private void Update()
    {
        counter += Time.deltaTime;
        if (counter >= 2 * pause)
        {
            counter = 0.0f;
            card.FlipCard(pause);
        }
    }
}
