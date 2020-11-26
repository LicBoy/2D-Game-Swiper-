using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Language
{
    //MainMenu text names
    #region MAINMENU
    public const string MenuHighscoreWord = "Highscore";
    public const string MenuMaxWaveWord = "Max Waves";
    public const string MenuPlayWord = "Play";
    public const string MenuOptionsWord = "Options";
    public const string MenuLangWord = "Language";
    public const string MenuThemeWord = "DarkTheme";
    public const string MenuSoundWord = "SoundOn";
    #endregion

    //PlayMode text names
    #region PLAYMODE
    //UI Elements
    public const string PlayStreakWord = "Streak";
    public const string PlayWaveWord = "Wave";
    public const string PlayBonusChooseWord = "Choose Bonus";
    public const string PlayGameOverWord = "Game over";
    public const string PlayGameOverScoreWord = "Score";
    public const string PlayGameOverNewHighscoreWord = "NewHighscore";
    #endregion

    #region RUSSIAN Tranlsation

    //MainMenu russian translation
    public static Dictionary<string, string> rusMainMenuTranlsation = new Dictionary<string, string>()
    {
        {MenuHighscoreWord, "Рекорд"},
        {MenuMaxWaveWord, "Макс. дней" },
        {MenuPlayWord, "Игра"},
        {MenuOptionsWord, "Настройки"},
        {MenuLangWord, "Язык" },
        {MenuThemeWord, "Тёмный режим"},
        {MenuSoundWord, "Звук"}
    };

    //PlayMode russian translation
    public static Dictionary<string, string> rusPlayModeTranlsation = new Dictionary<string, string>()
    {
        {PlayStreakWord, "Стрик" },
        {PlayWaveWord, "День" },
        {PlayBonusChooseWord, "Выбор бонуса" },
        {PlayGameOverWord, "Конец игры" },
        {PlayGameOverScoreWord, "Счёт" },
        {PlayGameOverNewHighscoreWord, "Новый рекорд!" }
    };

    public static Dictionary<int, Dictionary<string, string>> rusTranslation = new Dictionary<int, Dictionary<string, string>>()
    {
        { 0, rusMainMenuTranlsation},
        { 1, rusPlayModeTranlsation}
    };
    #endregion


    #region ENGLISH Tranlsation

    //MainMenu english translation
    public static Dictionary<string, string> engMainMenuTranlsation = new Dictionary<string, string>()
    {
        {MenuHighscoreWord, "Highscore"},
        {MenuMaxWaveWord, "Max Waves" },
        {MenuPlayWord, "Play"},
        {MenuOptionsWord, "Options"},
        {MenuLangWord, "Language" },
        {MenuThemeWord, "Dark theme"},
        {MenuSoundWord, "Sound"}
    };

    //PlayMode english translation
    public static Dictionary<string, string> engPlayModeTranlsation = new Dictionary<string, string>()
    {
        {PlayStreakWord, "Streak" },
        {PlayWaveWord, "Wave" },
        {PlayBonusChooseWord, "Choose bonus" },
        {PlayGameOverWord, "Game over" },
        {PlayGameOverScoreWord, "Score" },
        {PlayGameOverNewHighscoreWord, "New highscore!" }
    };

    public static Dictionary<int, Dictionary<string, string>> engTranslation = new Dictionary<int, Dictionary<string, string>>()
    {
        { 0, engMainMenuTranlsation},
        { 1, engPlayModeTranlsation}
    };
    #endregion

    /*  Next Dictionary splits into 2 dictionaries of each language
*   Each language splits into 2 dictionaris with keys  0 or 1 - MainMenu or PlayMode in build settings 
*   Last dictionary consist of cur language pair string-string for each UI element of current scene 
*/
    public static Dictionary<string, Dictionary<int, Dictionary<string, string>>> TranslationDictionary = new Dictionary<string, Dictionary<int, Dictionary<string, string>>>()
    {
        {SystemLanguage.Russian.ToString(), rusTranslation },
        {SystemLanguage.English.ToString(), engTranslation }
    };
}
