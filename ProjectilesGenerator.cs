using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProjectilesGenerator : MonoBehaviour
{
    public float coldown = 1;
    public float chanceOfDefendedAster = 0.04f;

    private float generateCounter = 0f;
    private bool isDarkTheme;
    private int[] defendedAsteroidArray;

    public GameObject asteroid1;
    public Color[] asteroid1_Colors;
    public GameObject asteroid2;
    public Color[] asteroid2_Colors;
    public GameObject asteroidDefended;
    public Color[] asteroidDefended_Colors;

    private void Start()
    {
        SetCurrentTheme();
    }

    public void SetCurrentTheme()
    {
        isDarkTheme = PlayerPrefs.GetInt(MainMenuScript.DarkTheme) == 1 ? true : false;

        if(isDarkTheme)
        {
            //Apply darker colors
            asteroid1.GetComponent<SpriteRenderer>().color = asteroid1_Colors[0];
            asteroid2.GetComponent<SpriteRenderer>().color = asteroid2_Colors[0];
            asteroidDefended.GetComponent<SpriteRenderer>().color = asteroidDefended_Colors[0];
        }
        else
        {
            //Appply brighter colors
            asteroid1.GetComponent<SpriteRenderer>().color = asteroid1_Colors[1];
            asteroid2.GetComponent<SpriteRenderer>().color = asteroid2_Colors[1];
            asteroidDefended.GetComponent<SpriteRenderer>().color = asteroidDefended_Colors[1];
        }
    }

    public Color GetDefendedAsteroidColor()
    {
        Color col = isDarkTheme ? asteroidDefended_Colors[0] : asteroidDefended_Colors[1];
        return col;
    }

    public void GenerateProjectiles()
    {
        generateCounter += Time.deltaTime;
        if (generateCounter >= coldown)
        {
            generateCounter = UnityEngine.Random.Range(0f, 0.7f);
            GameObject.Instantiate(RandomWhichAsteroid(), transform);
        }
    }

    private GameObject RandomWhichAsteroid()
    {
        float num = UnityEngine.Random.Range(0, 1f);
        if (num < 0.5)
            return asteroid1;
        return asteroid2;
    }

    public int[] CountDefendedAsteroidsAmount(int projesOnCurWave)
    {
        int amount = Mathf.RoundToInt(projesOnCurWave * chanceOfDefendedAster);
        defendedAsteroidArray = new int[amount];

        print("COUNTING DEFENDED asters. Amount of defended asters :" + amount);
        for(int i=0; i<amount; i++)
        {
            int temp = Random.Range(1, projesOnCurWave-2);
            while(defendedAsteroidArray.Contains(temp))
                temp = Random.Range(1, projesOnCurWave-2);
            defendedAsteroidArray[i] = temp;
        }

        print("Totally will be dropped " + amount + " defened asters of totally " + projesOnCurWave);
        return defendedAsteroidArray;
    }

    public void DropDefendedProjectile()
    {
        GameObject defProjectile = GameObject.Instantiate(asteroidDefended, transform);
    }

}
