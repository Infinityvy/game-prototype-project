using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject OptionPanel;
    public GameObject MenuPanel;

    public void OpenMenuPauseGame()
    {
        MenuPanel.SetActive(true);
        Session.instance.setPaused(true);
        Session.instance.isPaused = true;
    }
    public void ResumeGame()
    {
        MenuPanel.SetActive(false);
        Session.instance.setPaused(false);
        Session.instance.isPaused = false;
    }

    public void OpenOptions()
    {
        OptionPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        OptionPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string name)
    {
        Session.instance.setPaused(false);
        SceneManager.LoadScene(name);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (Session.instance.isPaused)
                ResumeGame();
            else
                OpenMenuPauseGame();
        }
    }
}
