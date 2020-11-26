using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    /*Player prefs keys are: GameLanguage, DarkTheme, SoundOn
     * GameLanguage - string, name of SystemLanguage on first start
     * DarkTheme - int, 1 - dark, 0 - light
     * SoundOn - int, 1 - sound on, 0 - sound off   
    */
    // Start is called before the first frame update

    public const string GameLanguage = "GameLanguage";
    public const string DarkTheme = "DarkTheme";
    public const string SoundOn = "SoundOn";

    public GameObject languagesPanel;
    public Sprite rusSprite;
    public Sprite engSprite;
    public Image langButtonImage;
    public Toggle themeToggle;
    public Toggle soundToggle;

    public TextMeshProUGUI highscoreORWaveWord;
    public TextMeshProUGUI highscoreORWaveNumber;

    private int highscore;
    private int maxWave;

    void Start()
    {
        //Loading player to get highscore
        Player data = new Player();
        data.LoadPlayer();
        highscore = data.highscore;
        maxWave = data.maxWave;
        
        CheckPrefs();
    }

    public void HighscoreAndWaveControl(bool isHighscoreNow, string highscoreText, string wavesText)
    {
        highscoreORWaveWord.text = isHighscoreNow ? highscoreText : wavesText;
        highscoreORWaveNumber.text = isHighscoreNow ? highscore.ToString() : maxWave.ToString();
    }

    void CheckPrefs()
    {
        //Check if all prefs are there, if any doesn't exist, recreate all
        if (!PlayerPrefs.HasKey(GameLanguage) || !PlayerPrefs.HasKey(DarkTheme) || !PlayerPrefs.HasKey(SoundOn))
        {
            if (Application.systemLanguage == SystemLanguage.Russian) //We have only 2 languages, so ru for russians and eng for all others
            {
                PlayerPrefs.SetString(GameLanguage, SystemLanguage.Russian.ToString());
                langButtonImage.sprite = rusSprite;
            }
            else
            {
                PlayerPrefs.SetString(GameLanguage, SystemLanguage.English.ToString());
                langButtonImage.sprite = engSprite;
            }
            PlayerPrefs.SetInt(DarkTheme, 1);
            PlayerPrefs.SetInt(SoundOn, 1);
            print("NO player prefs found");
        }
        else
        {
            //Loading correct language image, setting toggles
            if (PlayerPrefs.GetString(GameLanguage) == SystemLanguage.Russian.ToString())
                langButtonImage.sprite = rusSprite;
            else
                langButtonImage.sprite = engSprite;

            themeToggle.isOn = PlayerPrefs.GetInt(DarkTheme) == 1? true : false;

            soundToggle.isOn = PlayerPrefs.GetInt(SoundOn) == 1 ? true : false;

            print("Prefs are here\nGameLanguage is " + PlayerPrefs.GetString(GameLanguage) + "\nTheme is " +
                PlayerPrefs.GetInt(DarkTheme).ToString() + "\nSoundOn is " + PlayerPrefs.GetInt(SoundOn).ToString());
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LanguageChoose()
    {
        if (languagesPanel.activeSelf)
            languagesPanel.SetActive(false);
        else
            languagesPanel.SetActive(true);
    }

    public void SetGameLanguage(GameObject buttonName)
    {
        //check if buttonName is correct
        if (buttonName.name != SystemLanguage.Russian.ToString() && buttonName.name != SystemLanguage.English.ToString())
        {
            Debug.LogError("Language names are not correct! Got " + buttonName.name + " Needed " + SystemLanguage.Russian.ToString() + " or " + SystemLanguage.English.ToString());
            return;
        }
           
        PlayerPrefs.SetString(GameLanguage, buttonName.name);
        print("Succesfully changed langauge to " + buttonName.name);
    }

    public void SetTheme(bool isDarkTheme)
    {
        int num = isDarkTheme ? 1 : 0;
        PlayerPrefs.SetInt(DarkTheme, num);
        print("Successfully changed darktheme to " + isDarkTheme);
    }

    public void SetSound(bool isSoundOn)
    {
        int num = isSoundOn ? 1 : 0;
        PlayerPrefs.SetInt(SoundOn, num);
        print("Successfully changed soundOn to " + isSoundOn);
    }
}
