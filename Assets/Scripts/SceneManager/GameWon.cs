using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameWon : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public Canvas gameWonCanvas; // Reference to the Canvas component

    void Start()
    {
        // Ensure the Canvas is initially disabled
        gameWonCanvas.enabled = false;

        // Play the video when the GameOver scene starts
        videoPlayer.Play();

        // Enable the Canvas after a short delay
        StartCoroutine(EnableCanvasAfterDelay());
    }

    IEnumerator EnableCanvasAfterDelay()
    {
        // Wait for the video to finish playing (adjust the delay as needed)
        yield return new WaitForSeconds((float)videoPlayer.length);

        // Enable the Canvas
        gameWonCanvas.enabled = true;
    }

    public void GameWin()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
