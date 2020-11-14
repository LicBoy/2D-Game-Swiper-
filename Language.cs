using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Language
{
    //MainMenu text names

    public const string MenuPlayWord = "Play";
    public const string MenuOptionsWord = "Options";
    public const string MenuLangWord = "Language";
    public const string MenuThemeWord = "DarkTheme";
    public const string MenuSoundWord = "SoundOn";

    #region RUSSIAN Tranlsation

    //MainMenu russian translation
    public static Dictionary<string, string> rusMainMenuTranlsation = new Dictionary<string, string>()
    {
        {MenuPlayWord, "Игра"},
        {MenuOptionsWord, "Настройки"},
        {MenuLangWord, "Язык" },
        {MenuThemeWord, "Тёмный режим"},
        {MenuSoundWord, "Звук"}
    };

    //PlayMode russian translation
    public static Dictionary<string, string> rusPlayModeTranlsation = new Dictionary<string, string>()
    {

    };
    #endregion


    #region ENGLISH Tranlsation

    //MainMenu english translation
    public static Dictionary<string, string> engMainMenuTranlsation = new Dictionary<string, string>()
    {
        {MenuPlayWord, "Play"},
        {MenuOptionsWord, "Options"},
        {MenuLangWord, "Language" },
        {MenuThemeWord, "Dark theme"},
        {MenuSoundWord, "Sound"}
    };

    //PlayMode english translation
    public static Dictionary<string, string> engPlayModeTranlsation = new Dictionary<string, string>()
    {

    };
    #endregion

    public static Dictionary<int, Dictionary<string, string>> rusTranslation = new Dictionary<int, Dictionary<string, string>>()
    {
        { 0, rusMainMenuTranlsation},
        { 1, rusPlayModeTranlsation}
    };

    public static Dictionary<int, Dictionary<string, string>> engTranslation = new Dictionary<int, Dictionary<string, string>>()
    {
        { 0, engMainMenuTranlsation},
        { 1, engPlayModeTranlsation}
    };

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
