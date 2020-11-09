using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    int score;


    private void Start()
    {
        score = 0;
    }


    private void Update()
    {
        scoreText.text = "Score:\n" + score;
    }


    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }


    public void ZeroScore()
    {
        score = 0;
    }


    public int GetScore()
    {
        return score;
    }
}
