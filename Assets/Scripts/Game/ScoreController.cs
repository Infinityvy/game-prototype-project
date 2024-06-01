using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class ScoreController
{
    public static int highscore = 0;

    public static int currentScore = 0;

    public static void flush()
    {
        highscore = 0; 
        currentScore = 0;
    }

    public static void saveHighscore()
    {
        PlayerPrefs.SetInt("Highscore", highscore);
    }

    public static void loadHighScore() 
    {
        if (PlayerPrefs.HasKey("Highscore"))
            highscore = PlayerPrefs.GetInt("Highscore");
        else highscore = 0;
    }

    public static void updateHighscore()
    {
        TextMeshProUGUI textmesh = GameObject.Find("HighScoreText").GetComponent<TextMeshProUGUI>();

        if (currentScore > highscore)
        {
            textmesh.text = "NEW Highscore: " + currentScore.ToString() + " !!! \nPrevius Highscore: " + highscore;
            highscore = currentScore;
            saveHighscore();
        }
        else if (currentScore == highscore)
        {
            textmesh.text = "Matched Highscore: " + highscore.ToString() + "\nYour score: " + currentScore;
        }
        else if(currentScore < highscore)
        {
            textmesh.text = "Highscore: " + highscore.ToString() + "\nYour score: " + currentScore;
        }
    }
}
