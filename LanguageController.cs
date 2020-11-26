using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LanguageController : MonoBehaviour
{
    #region MAINMENU UI
    [Header("MainMenu")]
    public MenuHighscoreWaveChangeScript highscoreOrWaveScript;
    public TextMeshProUGUI highscoreORWaveWord;
    public TextMeshProUGUI playButtonText;
    public TextMeshProUGUI optionsButtonText;
    public TextMeshProUGUI optionsLanguageText;
    public TextMeshProUGUI optionsThemeText;
    public TextMeshProUGUI optionsSoundText;
    #endregion

    #region PLAYMODE UI
    [Header("PlayMode")]
    public TextMeshProUGUI streakText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI bonusChooseText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI gameOverScoreText;
    public BonusGenerator bonusesGenerator;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SetCorrectLanguage(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetCorrectLanguage(int sceneIndex) //Use sceneIndex for current scene
    {
        //First get current game language, then get needed dictionary
        string curLanguage = PlayerPrefs.GetString(MainMenuScript.GameLanguage);

        print("Current language is " + curLanguage);

        if(sceneIndex == 0) //If current scene is MainMenu
        {
            highscoreOrWaveScript.highscoreTranslation = Language.TranslationDictionary[curLanguage][sceneIndex][Language.MenuHighscoreWord];
            highscoreOrWaveScript.maxWaveTranslation = Language.TranslationDictionary[curLanguage][sceneIndex][Language.MenuMaxWaveWord];
            highscoreORWaveWord.text = highscoreOrWaveScript.isHighscoreText ? highscoreOrWaveScript.highscoreTranslation : highscoreOrWaveScript.maxWaveTranslation;

            playButtonText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.MenuPlayWord];
            optionsButtonText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.MenuOptionsWord];
            optionsLanguageText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.MenuLangWord];
            optionsThemeText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.MenuThemeWord];
            optionsSoundText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.MenuSoundWord];
        }
        else if(sceneIndex == 1)    //If current scene is PlayMode 
        {
            streakText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.PlayStreakWord];
            waveText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.PlayWaveWord];
            bonusChooseText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.PlayBonusChooseWord];
            gameOverText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.PlayGameOverWord];
            gameOverScoreText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.PlayGameOverScoreWord];

            int k = curLanguage == SystemLanguage.English.ToString() ? 0 : 1; //Check which language used, eng at index 0
            for(int i=0; i<bonusesGenerator.bonuses.Length; i++)
            {
                bonusesGenerator.bonuses[i].bonusName = bonusesGenerator.bonuses[i].namesTranslations[k];
                bonusesGenerator.bonuses[i].bonusActivationPhrase = bonusesGenerator.bonuses[i].activationsTranslations[k];
                bonusesGenerator.bonuses[i].bonusPreviewText = bonusesGenerator.bonuses[i].previewTranslations[k];
            }
        }

    }
}
