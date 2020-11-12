using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWaveUI : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public TextMeshProUGUI integerUI;
    public TextMeshProUGUI bonusCounter;
    public GameObject bonusesPanel;
    public GameObject[] bonusPanelButtons;

    private bool isBonusPanelActive = false;
    private float bonusesTimer = 0f;
    private float bonusesAnimationTime;
    private ColorBlock disabledColor;
    private GameObject[] curBonuses;

    private void Start()
    {
        bonusesAnimationTime = bonusesPanel.gameObject.GetComponent<Animation>().clip.length;
        disabledColor = bonusPanelButtons[0].GetComponent<Button>().colors;
    }

    private void Update()
    {
        if(isBonusPanelActive)
        {
            bonusCounter.text = ((int)Mathf.Round(bonusesAnimationTime - bonusesTimer)).ToString();
            if(bonusesTimer >= bonusesAnimationTime)
            {
                isBonusPanelActive = false;
                bonusesTimer = 0f;
            }
            bonusesTimer += Time.deltaTime;
        }
    }

    public IEnumerator ShowChangeWaveAnimation(int wave)
    {
        integerUI.text = wave.ToString();
        yield return new WaitForSeconds(2);
        textUI.gameObject.GetComponent<Animation>().Play();
        integerUI.gameObject.GetComponent<Animation>().Play();
    }

    public void BonusPanelAppear()
    {
        isBonusPanelActive = true;

        curBonuses = GameController.instance.GetComponent<BonusGenerator>().RandomBonusPanel();
        for(int i=0; i<3; i++)
        {
            bonusPanelButtons[i].GetComponent<Image>().sprite = curBonuses[i].GetComponent<Bonus>().bonusIcon;
            bonusPanelButtons[i].GetComponent<Button>().interactable = true;
            bonusPanelButtons[i].GetComponent<Button>().colors = disabledColor;
        }

        bonusesPanel.gameObject.GetComponent<Animation>().Play();
    }


    public void DropBonusOnClick(GameObject whichButtonPressed)
    {
        int i = 0;
        foreach(GameObject obj in bonusPanelButtons)
        {
            if (obj.gameObject.Equals(whichButtonPressed))
            {
                ColorBlock colBlock = obj.GetComponent<Button>().colors;
                colBlock.normalColor = colBlock.selectedColor;
                colBlock.disabledColor = colBlock.selectedColor;

                GameController.instance.GetComponent<BonusGenerator>().DropBonus(curBonuses[i]);
                obj.GetComponent<Button>().colors = colBlock;
            }
            i++;
        }
    }

    public void GameOverChanges()
    {
        isBonusPanelActive = false;
    }
}
