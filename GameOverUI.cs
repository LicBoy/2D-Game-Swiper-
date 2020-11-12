using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public GameObject animObj;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreWord;
    public GameObject panel;

    private Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = animObj.GetComponent<Animation>();
        panel.SetActive(false);
    }

    public void ShowGameOverMenu()
    {
        anim.Play("GameOverAnim");
    }

    public void CloseGameOverMenu()
    {
        anim.Play("GameOverAnimReverse");
    }

    public void SetMenuNotActive()
    {
        anim.Stop();
        panel.SetActive(false);
    }

    public void RestartGame()
    {
        GameController.instance.GetComponent<GameController>().StartCoroutine("LoadPlayerData");
    }
}
