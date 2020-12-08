using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreWord;

    private Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
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
    }

    public void RestartGame()
    {
        GameController.instance.GetComponent<GameController>().StartCoroutine("LoadPlayerData");
    }

    public void DisplayAd()
    {
        GameController.instance.GetComponent<AdManager>().DisplayInterstitial();
    }
}
