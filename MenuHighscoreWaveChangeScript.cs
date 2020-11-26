using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuHighscoreWaveChangeScript : MonoBehaviour
{
    public bool isHighscoreText = false;

    public string highscoreTranslation;
    public string maxWaveTranslation;

    public MainMenuScript mainScript;

    public void ChangeText()
    {
        isHighscoreText = !isHighscoreText;
        mainScript.HighscoreAndWaveControl(isHighscoreText, highscoreTranslation, maxWaveTranslation);
    }
}
