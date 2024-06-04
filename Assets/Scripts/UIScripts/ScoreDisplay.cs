using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public Text scoreText;

    void Start()
    {
        InvokeRepeating(nameof(updateScore), 0.1f, 0.25f);
    }

    private void updateScore()
    {
        scoreText.text = "Score: " + ScoreController.currentScore.ToString();
    }
}
