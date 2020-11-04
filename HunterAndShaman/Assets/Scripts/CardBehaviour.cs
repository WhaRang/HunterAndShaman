using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CardBehaviour : MonoBehaviour
{
    Image cardImage;
    bool isFlipped;

    public Sprite backSprite;
    public Sprite frontSprite;

    float timeOfMoving = 0.5f;


    private void Start()
    {
        cardImage = GetComponent<Image>();
        isFlipped = false;
        cardImage.sprite = backSprite;
    }


    public void FlipCard()
    {
        StartCoroutine(FlipCardCoroutine());
    }


    IEnumerator FlipCardCoroutine()
    {
        RotateTo(Quaternion.Euler(0.0f, 90.0f, 0.0f), timeOfMoving / 2);
        yield return null;
        yield return new WaitForSeconds(timeOfMoving / 2);
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
        RotateTo(Quaternion.Euler(0.0f, 0.0f, 0.0f), timeOfMoving / 2);
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


    void RotateTo(Quaternion newRot, float seconds)
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
}
