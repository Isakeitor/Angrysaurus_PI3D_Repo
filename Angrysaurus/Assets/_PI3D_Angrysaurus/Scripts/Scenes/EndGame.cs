using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame: MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] string gameSceneName = "Game";
    [SerializeField] string mainMenuSceneName = "MainMenu";

    // REINICIAR NIVEL
    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(gameSceneName);
    }

    // VOLVER AL MENÚ
    public void BackToMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuSceneName);
    }
}