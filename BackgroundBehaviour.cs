using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehaviour : MonoBehaviour
{
    private bool isDarkTheme;

    public GameObject darkBackground;
    public GameObject brightBackground;

    private GameObject primaryBackground;
    private GameObject secondaryBackground;

    private Animation primaryBackgAnim;
    private Animation secondaryBackgAnim;

    private string primaryAnimAppearName;
    private string primaryAnimIdleName;
    private string primaryAnimFadeName;
    private string secondaryAnimFadeName;
    private string secondaryAnimAppearName;

    public void SetTheme()
    {
        isDarkTheme = PlayerPrefs.GetInt(MainMenuScript.DarkTheme) == 1 ? true : false;

        if (isDarkTheme)
        {
            primaryBackground = darkBackground; secondaryBackground = brightBackground;
            primaryAnimAppearName = "DarkThemeAppearAnim";
            primaryAnimIdleName = "DarkThemeIdleAnim";
            primaryAnimFadeName = "DarkThemeFadeAnim";
            secondaryAnimFadeName = "BrightThemeFadeAnim";
            secondaryAnimAppearName = "BrightThemeAppearAnim";

            brightBackground.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            darkBackground.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
        else
        {
            primaryBackground = brightBackground; secondaryBackground = darkBackground;
            primaryAnimAppearName = "BrightThemeAppearAnim";
            primaryAnimIdleName = "BrightThemeIdleAnim";
            primaryAnimFadeName = "BrightThemeFadeAnim";
            secondaryAnimFadeName = "DarkThemeFadeAnim";
            secondaryAnimAppearName = "DarkThemeAppearAnim";

            brightBackground.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            darkBackground.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
        primaryBackgAnim = primaryBackground.GetComponent<Animation>();
        secondaryBackgAnim = secondaryBackground.GetComponent<Animation>();
    }

    public IEnumerator BackgroundAppear()
    {
        primaryBackgAnim.Play(primaryAnimAppearName);
        secondaryBackgAnim.Play(secondaryAnimFadeName);

        yield return new WaitForSeconds(primaryBackgAnim.GetClip(primaryAnimAppearName).length);

        primaryBackgAnim.Play(primaryAnimIdleName);
    }

    public void BackgroundDisappear()
    {
        primaryBackgAnim.Play(primaryAnimFadeName);
        secondaryBackgAnim.Play(secondaryAnimAppearName);
    }
}
