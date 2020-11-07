using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardBehaviour))]
public class HorseCard : MonoBehaviour
{
    const int START_HEALTH = 1;
    const int MAX_HEALTH = 2;
    const int MIN_HEALTH = 0;

    CardBehaviour cardBehaviour;
    int health;

    public Sprite health_02;
    public Sprite health_12;
    public Sprite health_22;

    Sprite curr_sprtie;


    private void Start()
    {
        curr_sprtie = health_12;
        cardBehaviour = GetComponent<CardBehaviour>();
        health = START_HEALTH;
    }


    public void AffectOnHorse(CardBehaviour affectingCard)
    {
        if (affectingCard.cardType != CardBehaviour.CARD_TYPE.ORDINARY)
        {
            return;
        }

        if (affectingCard.cardColor == cardBehaviour.cardColor)
        {
            health++;
        }
        else
        {
            health--;
        }

        ManageHealth();
        ManageSprites();
    }


    void ManageSprites()
    {
        switch (health)
        {
            case 0:
                {
                    curr_sprtie = health_02;
                    break;
                }
            case 1:
                {
                    curr_sprtie = health_12;
                    break;
                }
            case 2:
                {
                    curr_sprtie = health_22;
                    break;
                }

        }

        cardBehaviour.SetSprite(curr_sprtie);
    }


    void ManageHealth()
    {
        if (health > MAX_HEALTH)
        {
            health = MAX_HEALTH;
        }

        if (health < MIN_HEALTH)
        {
            health = MIN_HEALTH;
        }
    }


    public CardBehaviour.CARD_COLOR GetColor()
    {
        return cardBehaviour.cardColor;
    }
}
