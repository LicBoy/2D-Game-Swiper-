using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWaveUI : MonoBehaviour
{
    public TMP_ColorGradient[] curColorModeGradients; //at index 0 - brightTheme, 1 - darkTheme

    public TextMeshProUGUI textUI;
    public TextMeshProUGUI integerUI;
    public TextMeshProUGUI bonusCounter;
    public GameObject bonusesPanel;
    public Button[] bonusPanelButtons;
    public TextMeshProUGUI[] bonusButtonsText;

    private bool isBonusPanelActive = false;
    private int curWave;
    private float bonusesTimer = 0f;
    private float bonusesAnimationTime;
    private ColorBlock disabledColor;
    private GameObject[] curBonuses;

    private void Start()
    {
        
        disabledColor = bonusPanelButtons[0].GetComponent<Button>().colors;
    }

    private void Update()
    {
        if(isBonusPanelActive)
        {
            bonusCounter.text = (bonusesAnimationTime - bonusesTimer).ToString("0");
            if(bonusesTimer >= bonusesAnimationTime)
            {
                isBonusPanelActive = false;
                bonusesTimer = 0f;
                BonusPanelClose(curWave);
            }
            bonusesTimer += Time.deltaTime;
        }
    }

    public void ChangeColorDependingOnTheme(bool isDarkTheme)
    {
        if (isDarkTheme)
        {
            textUI.colorGradientPreset = curColorModeGradients[1];
            integerUI.colorGradientPreset = curColorModeGradients[1];
        }
        else
        {
            textUI.colorGradientPreset = curColorModeGradients[0];
            integerUI.colorGradientPreset = curColorModeGradients[0];
        }
    }

    public IEnumerator ShowChangeWaveAnimation(int wave)
    {
        yield return new WaitForSeconds(2f);
        integerUI.text = wave.ToString();
        textUI.gameObject.GetComponent<Animation>().Play();
        integerUI.gameObject.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(textUI.gameObject.GetComponent<Animation>().clip.length);
        bonusesTimer = 0f;
        GameController.instance.changingLevel = false;
    }

    public IEnumerator BonusPanelAppear(int wave)
    {
        bonusesAnimationTime = 9f;
        curWave = wave;
        GameController.instance.changingLevel = true;
        yield return new WaitForSeconds(2f);
        GameController.instance.backgroundController.BackgroundDisappear();

        curBonuses = GameController.instance.GetComponent<BonusGenerator>().RandomBonusPanel();
        for (int i = 0; i < 3; i++)
        {
            bonusPanelButtons[i].GetComponent<Image>().sprite = curBonuses[i].GetComponent<Bonus>().bonusIcon;
            bonusPanelButtons[i].GetComponent<Button>().interactable = true;
            bonusPanelButtons[i].GetComponent<Button>().colors = disabledColor;
            bonusButtonsText[i].text = curBonuses[i].GetComponent<Bonus>().bonusPreviewText;
        }

        bonusesPanel.GetComponent<Animation>().Play("BonusPanelAppearAnim");
        yield return new WaitForSeconds(bonusesPanel.GetComponent<Animation>().GetClip("BonusPanelAppearAnim").length);
        bonusesPanel.GetComponent<Animation>().Play("BonusPanelIdleAnim");
        isBonusPanelActive = true;
    }

    public void BonusPanelClose(int wave)
    {
        isBonusPanelActive = false;
        if (GameController.instance.backgroundController != null)
            GameController.instance.backgroundController.StartCoroutine("BackgroundAppear");
        bonusesPanel.GetComponent<Animation>().Play("BonusPanelDisappearAnim");
        StartCoroutine("ShowChangeWaveAnimation", curWave);
    }

    public void DropBonusOnClick(int buttonNumInt)
    {
        ColorBlock colBlock = bonusPanelButtons[buttonNumInt].GetComponent<Button>().colors;
        colBlock.normalColor = colBlock.selectedColor;
        colBlock.disabledColor = colBlock.selectedColor;

        GameController.instance.GetComponent<BonusGenerator>().DropBonusAmountOfDrops(curBonuses[buttonNumInt]);
        bonusPanelButtons[buttonNumInt].GetComponent<Button>().colors = colBlock;

        BonusPanelClose(curWave);
    }

    public void GameOverChanges()
    {
        isBonusPanelActive = false;
        GameController.instance.RemoveAllBonuses();
    }

    public void TurnMusicOff(float time)
    {
        GameController.instance.StopMusic(time);
        StartCoroutine("ShowAdAfterTime", time);
    }

    IEnumerator ShowAdAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        GameController.instance.GetComponent<AdManager>().DisplayInterstitial();
    }
}
