using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LanguageController : MonoBehaviour
{
    #region MAIN MENU UI
    public TextMeshProUGUI playButtonText;
    public TextMeshProUGUI optionsButtonText;
    public TextMeshProUGUI optionsLanguageText;
    public TextMeshProUGUI optionsThemeText;
    public TextMeshProUGUI optionsSoundText;
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

        playButtonText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.MenuPlayWord];
        optionsButtonText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.MenuOptionsWord];
        optionsLanguageText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.MenuLangWord];
        optionsThemeText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.MenuThemeWord];
        optionsSoundText.text = Language.TranslationDictionary[curLanguage][sceneIndex][Language.MenuSoundWord];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
