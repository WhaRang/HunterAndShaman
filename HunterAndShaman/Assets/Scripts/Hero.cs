using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    public enum HEROES
    {
        HUNTER,
        SHAMAN
    }

    public HEROES heroType;

    public Image sourceImage;
    public Sprite hunterSprite;
    public Sprite shamanSprite;

    public Vector3 changingPos;

    float timeOfChanging = CardBehaviour.timeOfMoving;


    public void SetHero(HEROES newHeroType)
    {
        if (newHeroType != heroType)
        {
            ChangeHero();
        }
    }


    public void ChangeHero()
    {
        ChangeHero(timeOfChanging);
    }


    public void ChangeHero(float seconds)
    {
        StartCoroutine(ChangeHeroCoroutine(seconds));
    }


    IEnumerator ChangeHeroCoroutine(float seconds)
    {
        Vector3 startingPos = transform.position;

        MovingAnimations.instance.MoveObjTo(gameObject, changingPos, seconds / 2);
        yield return new WaitForSeconds(seconds / 2);
        ChangeAppearence();
        MovingAnimations.instance.MoveObjTo(gameObject, startingPos, seconds / 2);
    }


    void ChangeAppearence()
    {
        if (heroType == HEROES.HUNTER)
        {
            heroType = HEROES.SHAMAN;
            sourceImage.sprite = shamanSprite;
        } 
        else
        {
            heroType = HEROES.HUNTER;
            sourceImage.sprite = hunterSprite;
        }
    }
}
