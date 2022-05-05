using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayGame()
    {
        FindObjectOfType<AudioManager>().PlaySound("Theme");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void HowtoPlay()
    {
        FindObjectOfType<AudioManager>().PlaySound("Theme");
        Time.timeScale = 1;
        SceneManager.LoadScene("HowToPlay");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
