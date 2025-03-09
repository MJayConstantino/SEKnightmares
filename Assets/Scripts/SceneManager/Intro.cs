using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.Play();
        StartCoroutine(SwapSceneAfterDelay());
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(LoadSceneWithTransition("MainMenu"));
        }
    }

    IEnumerator SwapSceneAfterDelay()
    {
        yield return new WaitForSeconds((float)videoPlayer.length);
        StartCoroutine(LoadSceneWithTransition("MainMenu"));
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
