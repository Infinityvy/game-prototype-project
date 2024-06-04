using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Text text = GameObject.Find("HighScoreText").GetComponent<Text>();

        if (currentScore > highscore)
        {
            text.text = "NEW Highscore: " + currentScore.ToString() + " !!! \nPrevius Highscore: " + highscore;
            highscore = currentScore;
            saveHighscore();
        }
        else if (currentScore == highscore)
        {
            text.text = "Matched Highscore: " + highscore.ToString() + "\nYour score: " + currentScore;
        }
        else if(currentScore < highscore)
        {
            text.text = "Highscore: " + highscore.ToString() + "\nYour score: " + currentScore;
        }
    }
}
