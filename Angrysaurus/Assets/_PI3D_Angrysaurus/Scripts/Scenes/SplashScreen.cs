using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;

    [SerializeField] string nextScene = "SCN_MainMenu";

    void Start()
    {
        videoPlayer.loopPointReached += EndVideo;
    }

    void EndVideo(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextScene);
    }
}