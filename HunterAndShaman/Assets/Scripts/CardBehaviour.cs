﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CardBehaviour : MonoBehaviour
{
    public enum CARD_COLOR 
    { 
        RED,
        BLACK,
        DOUBLE
    }

    public enum CARD_TYPE
    {
        ORDINARY,
        CHANGE_ROLE,
        HORSE
    }

    public CARD_COLOR cardColor;
    public CARD_TYPE cardType;

    Image cardImage;
    bool isFlipped;
    bool isChoosed;
    bool isClickable;

    public Sprite backSprite;
    public Sprite frontSprite;

    public static float timeOfMoving = 0.5f;

    const float PULSE_SCALER = 1.2f;


    private void Start()
    {
        cardImage = GetComponent<Image>();
        isFlipped = false;
        isChoosed = false;
        isClickable = false;
        cardImage.sprite = backSprite;
    }


    private void Update()
    {
        if (transform.position.x < -Screen.width * 0.75)
        {
            Destroy(gameObject);
        }
    }


    public void FlipCard()
    {
        FlipCard(timeOfMoving);
    }


    public void FlipCard(float flipTime)
    {
        StartCoroutine(FlipCardCoroutine(flipTime));
    }


    IEnumerator FlipCardCoroutine(float flipTime)
    {
        RotateTo(Quaternion.Euler(0.0f, 90.0f, 0.0f), flipTime / 2);
        yield return new WaitForSeconds(flipTime / 2);
        if (isFlipped)
        {
            isFlipped = false;
            cardImage.sprite = backSprite;
        }
        else
        {
            isFlipped = true;
            cardImage.sprite = frontSprite;
        }
        RotateTo(Quaternion.Euler(0.0f, 0.0f, 0.0f), flipTime / 2);
    }


    public void MoveCardTo(Vector3 newPos)
    {
        MoveCardTo(newPos, timeOfMoving);
    }

    public void MoveCardTo(Vector3 newPos, float seconds)
    {
        MovingAnimations.instance.MoveObjTo(gameObject, newPos, seconds);
    }


    public void RotateTo(Quaternion newRot)
    {
        RotateTo(newRot, timeOfMoving);
    }


    public void RotateTo(Quaternion newRot, float seconds)
    {
        MovingAnimations.instance.RotateTo(gameObject, newRot, seconds);
    }


    public void MakePulse()
    {
        MakePulse(timeOfMoving);
    }


    public void MakePulse(float seconds)
    {
        MovingAnimations.instance.MakePulse(gameObject, PULSE_SCALER, seconds);
    }


    public void OnClick()
    {
        if (!isClickable)
        {
            return;
        }

        isClickable = false;
        isChoosed = true;
    }


    public bool IsFlipped()
    {
        return isFlipped;
    }


    public bool IsChoosed()
    {
        return isChoosed;
    }


    public void SetChoosed(bool choosed)
    {
        isChoosed = choosed;
    }


    public void SetClickable(bool clickable)
    {
        isClickable = clickable;
    }


    public void SetSprite(Sprite newSprite)
    {
        cardImage.sprite = newSprite;
    }
}