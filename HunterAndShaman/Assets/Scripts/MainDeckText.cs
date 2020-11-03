using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MainDeckText : MonoBehaviour
{
    Text textField;

    private void Start()
    {
        textField = GetComponent<Text>();
    }

    private void Update()
    {
        if (MainDeck.deck.HowManyCards() >= 0)
        {
            textField.text = "" + MainDeck.deck.HowManyCards();
        }
    }
}
