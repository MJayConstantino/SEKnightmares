using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public Animator transition; // Reference to the Animator component for crossfade animation
    public float transitionTime = 1f; // Time for the crossfade transition

    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
// Reference to the Canvas component

    void Start()
    {
        // Ensure the Canvas is initially disabled

        // Play the video when the GameOver scene starts
        videoPlayer.Play();

        // Enable the Canvas after a short delay
        StartCoroutine(SwapSceneAfterDelay());
    }


    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            // Set the flag to true to indicate a click
            StartCoroutine(LoadSceneWithTransition("MainMenu"));
        }
    }

    IEnumerator SwapSceneAfterDelay()
    {
        // Wait for the video to finish playing (adjust the delay as needed)
        yield return new WaitForSeconds((float)videoPlayer.length);

        StartCoroutine(LoadSceneWithTransition("MainMenu"));

        // Enable the Canvas

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
