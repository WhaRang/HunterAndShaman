using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Banner : MonoBehaviour
{
    public Text textField;
    public Animator animator;

    public static float lifeTime = 1.5f;
    float animTime = 0.33f;


    private void Start()
    {
        StartCoroutine(LifeTimeCoroutine());
    }


    IEnumerator LifeTimeCoroutine()
    {
        yield return new WaitForSeconds(lifeTime - animTime);
        animator.SetTrigger("out");
        yield return new WaitForSeconds(animTime);
        Destroy(gameObject);
    }


    public void SetText(string newText)
    {
        textField.text = newText;
    }


    public void SetLifeTime(float newLifeTime)
    {
        lifeTime = newLifeTime;
    }
}
