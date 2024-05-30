using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject OptionPanel;
    public GameObject MainMenuPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenOptions()
    {
        OptionPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }

    public void CloseOptions()
    {
        OptionPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
