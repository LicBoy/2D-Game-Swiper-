using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Animator animator;
    public float transitionTime;

    public void LoadNextLevel(int sceneIndex)
    {
        StartCoroutine(LoadScene(sceneIndex));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }

    void OnLevelWasLoaded(int level)
    {
        if(level == 1)
        {
            GameController.instance.StartCoroutine("LoadPlayerData");
            bool isMusicOn = PlayerPrefs.GetInt(MainMenuScript.SoundOn) == 1 ? true : false;
            if (isMusicOn)
            {
                GameController.instance.StartMusic(7f);
            }
        }
    }
}
