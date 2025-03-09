using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameWon : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Canvas gameWonCanvas;

    void Start()
    {
        gameWonCanvas.enabled = false;
        videoPlayer.Play();
        StartCoroutine(EnableCanvasAfterDelay());
    }

    IEnumerator EnableCanvasAfterDelay()
    {
        yield return new WaitForSeconds((float)videoPlayer.length);
        gameWonCanvas.enabled = true;
    }

    public void GameWin()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
