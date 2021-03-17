using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StreakUIController : MonoBehaviour
{
    public TMP_ColorGradient[] curColorModeGradients; //at index 0 - brightTheme, 1 - darkTheme

    private float counter = 0f;
    private float t = 0f;
    private float streakFontSize;
    private bool isShowingStreak = false;
    private bool isDisappearingStreak = false;
    private Color originalStreakColor;
    private Color currentStreakColor;
    private Color originalStreakWordColor;
    private int lastSwipeKillAmount;


    public float streakDisappearDuration = 3f;
    public float streakShownDuration = 3f;
    public TextMeshProUGUI streakWord;
    public TextMeshProUGUI streakText;

    // Start is called before the first frame update
    void Start()
    {
        streakFontSize = streakText.fontSize;
        originalStreakColor = streakText.color;
        currentStreakColor = originalStreakColor;
        originalStreakWordColor = streakWord.color;

        streakWord.enabled = false;
        streakText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowingStreak)
        {
            if(counter >= streakShownDuration)
            {
                isDisappearingStreak = true;
            }
            counter += Time.deltaTime;
        }

        if (isDisappearingStreak)
        {
            isShowingStreak = false;
            if (FadeIn())
            {
                streakWord.enabled = false; streakText.enabled = false;
                isShowingStreak = false; isDisappearingStreak = false;
                streakWord.color = originalStreakWordColor; streakText.color = originalStreakColor;
                streakText.fontSize = streakFontSize;
                counter = 0f; t = 0f;
            }
        }
    }

    public void ChangeColorDepengingOnTheme(bool isDarkTheme)
    {
        if (isDarkTheme)
        {
            streakWord.colorGradientPreset = curColorModeGradients[1];
            //streakText.colorGradientPreset = curColorModeGradients[1];
        }
        else
        {
            streakWord.colorGradientPreset = curColorModeGradients[0];
            //streakText.colorGradientPreset = curColorModeGradients[0];
        }
    }

    public void ShowStreak(int num)
    {
        if (isShowingStreak && num > lastSwipeKillAmount)
        {
            streakText.fontSize = Mathf.Clamp(streakFontSize + num * 4, streakFontSize, 128);
            streakText.color = new Color(streakText.color.r, streakText.color.g - ((float)num * 4) / 255, streakText.color.b);
        }
        else
        {
            streakText.fontSize = streakFontSize;
            streakText.color = originalStreakColor;
        }
        currentStreakColor = streakText.color;

        isShowingStreak = true;
        isDisappearingStreak = false;

        streakText.text = num.ToString();

        streakWord.color = originalStreakWordColor;
        streakWord.enabled = true; streakText.enabled = true;

        lastSwipeKillAmount = num;
        counter = 0f;
        t = 0f;
    }

    public bool FadeIn()
    {
        float val = Mathf.Lerp(0, 1, t);
        t += Time.deltaTime / streakDisappearDuration;
        streakWord.color = Color.Lerp(originalStreakWordColor, new Color(originalStreakWordColor.r, originalStreakWordColor.g, originalStreakWordColor.b, 0), val);
        streakText.color = Color.Lerp(currentStreakColor, new Color(currentStreakColor.r, currentStreakColor.g, currentStreakColor.b, 0), val);
        return t > 0.99;
    }
}
