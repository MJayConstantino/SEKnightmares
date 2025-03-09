using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameOver : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public VideoPlayer videoPlayer;
    public Canvas gameOverCanvas;

    void Start()
    {
        gameOverCanvas.enabled = false;
        videoPlayer.Play();
        StartCoroutine(EnableCanvasAfterDelay());
    }

    IEnumerator EnableCanvasAfterDelay()
    {
        yield return new WaitForSeconds((float)videoPlayer.length);
        gameOverCanvas.enabled = true;
    }

    public void MainMenu()
    {
        StartCoroutine(LoadSceneWithTransition("MainMenu"));
    }

    public void Retry()
    {
        StartCoroutine(LoadSceneWithTransition("Level_Proper"));
    }

    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
