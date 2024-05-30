using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public CanvasGroup OptionPanel;
    public CanvasGroup MenuPanel;
    private bool isPaused;

    public void OpenMenuPauseGame()
    {
        MenuPanel.alpha = 1;
        MenuPanel.blocksRaycasts = true;
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void ResumeGame()
    {
        MenuPanel.alpha = 0;
        MenuPanel.blocksRaycasts = false;
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenOptions()
    {
        OptionPanel.alpha = 1;
        OptionPanel.blocksRaycasts = true;
    }

    public void CloseOptions()
    {
        OptionPanel.alpha = 0;
        OptionPanel.blocksRaycasts = false;
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(1);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                OpenMenuPauseGame();
        }
    }
}
