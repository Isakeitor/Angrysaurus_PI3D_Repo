using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] string gameSceneName = "Game";
    [SerializeField] string infoSceneName = "Info";

    // PLAY
    public void PlayGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(gameSceneName);
    }

    // INFO
    public void OpenInfo()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(infoSceneName);
    }

    // EXIT
    public void ExitGame()
    {
        Debug.Log("Cerrando juego...");

        Application.Quit();
    }
}