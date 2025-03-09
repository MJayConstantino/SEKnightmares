using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLevel : MonoBehaviour
{
    public Animator story;
    public Canvas firstCanvas;
    public Canvas secondCanvas;
    public Animator transition;
    public float transitionTime = 1f;


    void Start()
    {
        firstCanvas.enabled = true;
        secondCanvas.enabled = false;
        StartCoroutine(AnimateTutorial());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(LoadSceneWithTransition("Level_Proper"));
        }
    }

    IEnumerator AnimateTutorial()
    {
        yield return new WaitForSeconds(story.GetCurrentAnimatorStateInfo(0).length);
        firstCanvas.enabled = false;
        secondCanvas.enabled = true;
    }

    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

    public void OnAnimationEnd()
    {
        LoadSceneWithTransition("Level_Proper");
    }
}
