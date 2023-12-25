using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLevel : MonoBehaviour
{
    public Animator story; // Reference to the Animator component for crossfade animation
    public Canvas firstCanvas; // Reference to the first Canvas
    public Canvas secondCanvas; // Reference to the second Canvas

    public Animator transition;
    public float transitionTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Disable the second canvas initially
        firstCanvas.enabled = true;
        secondCanvas.enabled = false;

        // Start the animation
        StartCoroutine(AnimateTutorial());
    }

    // Update is called once per frame
    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(LoadSceneWithTransition("Level_Proper"));
        }
    }

    IEnumerator AnimateTutorial()
    {
        // Set the trigger to start the story animation


        // Wait for the transitionTime
        yield return new WaitForSeconds(story.GetCurrentAnimatorStateInfo(0).length);

        // Disable the first canvas
        firstCanvas.enabled = false;

        // Enable the second canvas
        secondCanvas.enabled = true;

        // Wait for the left mouse button click or any other condition to proceed
    }

    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

    // This method is called by an animation event
    public void OnAnimationEnd()
    {
        // Animation has ended, set the flag to indicate it
        LoadSceneWithTransition("Level_Proper");
    }
}
