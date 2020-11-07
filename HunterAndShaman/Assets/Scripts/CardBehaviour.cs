using System.Collections;
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

    const float PULSE_SKALER = 1.2f;


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
        yield return null;
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
        StartCoroutine(SmoothMove(transform.position, newPos, timeOfMoving));
    }

    public void MoveCardTo(Vector3 newPos, float seconds)
    {
        StartCoroutine(SmoothMove(transform.position, newPos, seconds));
    }


    IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }


    public void RotateTo(Quaternion newRot)
    {
        StartCoroutine(SmoothRotation(transform.rotation, newRot, timeOfMoving));
    }


    public void RotateTo(Quaternion newRot, float seconds)
    {
        StartCoroutine(SmoothRotation(transform.rotation, newRot, seconds));
    }


    IEnumerator SmoothRotation(Quaternion startRot, Quaternion endRot, float seconds)
    {
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / seconds;
            transform.rotation = Quaternion.Slerp(startRot, endRot, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }


    public void MakePulse()
    {
        MakePulse(timeOfMoving);
    }


    public void MakePulse(float seconds)
    {
        StartCoroutine(MakePulseCoroutine(seconds));
    }


    IEnumerator MakePulseCoroutine(float seconds)
    {
        Vector3 biggerScale = transform.localScale;
        Vector3 smallerScale = transform.localScale;

        smallerScale.x *= PULSE_SKALER;
        smallerScale.y *= PULSE_SKALER;
        smallerScale.z *= PULSE_SKALER;

        StartCoroutine(SmoothScaleChanging(biggerScale, smallerScale, seconds / 2));
        yield return new WaitForSeconds(seconds / 2);
        StartCoroutine(SmoothScaleChanging(smallerScale, biggerScale, seconds / 2));
    }


    IEnumerator SmoothScaleChanging(Vector3 oldScale, Vector3 newScale, float seconds)
    {
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / seconds;
            transform.localScale = Vector3.Lerp(oldScale, newScale, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
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
