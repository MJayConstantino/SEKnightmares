using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameOver : MonoBehaviour
{
    public Animator transition; // Reference to the Animator component for crossfade animation
    public float transitionTime = 1f; // Time for the crossfade transition

    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public Canvas gameOverCanvas; // Reference to the Canvas component

    void Start()
    {
        // Ensure the Canvas is initially disabled
        gameOverCanvas.enabled = false;

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
        gameOverCanvas.enabled = true;
    }

    public void MainMenu()
    {
        // Start the transition to the MainMenu scene
        StartCoroutine(LoadSceneWithTransition("MainMenu"));
    }

    public void Retry()
    {
        // Start the transition to the Level_Proper scene
        StartCoroutine(LoadSceneWithTransition("Level_Proper"));
    }

    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        // Set the trigger to start the crossfade animation
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }

        // Wait for the transitionTime
        yield return new WaitForSeconds(transitionTime);

        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }
}
