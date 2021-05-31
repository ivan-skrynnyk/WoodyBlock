using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;

    private int _score;

    void Start()
    {
        _score = 0;
        UpdateScoreText();
    }

    private void AddScore(int score)
    {
        _score += score;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = _score.ToString();
    }

    private void OnEnable()
    {
        GameEvents.AddScore += AddScore;
    }

    private void OnDisable()
    {
        GameEvents.AddScore -= AddScore;
    }
}
