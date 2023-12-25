using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public AudioSource start;

    public void StartGame()
    {
        start.Play();
        StartCoroutine(LoadSceneWithTransition("TutorialDirections")); // Replace "YourLevelSceneName" with the actual scene name
    }

    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
