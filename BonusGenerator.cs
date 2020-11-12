using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class BonusGenerator : MonoBehaviour
{
    public Bonus[] bonuses = new Bonus[5];

    private float[] spawnPosX = new float[2] {-2.5f, 2.5f };
    private float spawnPosY = 5.5f;

    public void DropBonus(GameObject knownBonus = null)
    {
        if (knownBonus == null)
        {
            knownBonus = RandomizeBonus();
            if (knownBonus == null)
            {
                Debug.LogError("Couldn't randomize bonus, check RandomizeBonus() func!");
            }
        }
        Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(spawnPosX[0], spawnPosX[1]), spawnPosY, 0);
        GameObject.Instantiate(knownBonus, spawnPos, Quaternion.identity);
    }

    public GameObject RandomizeBonus()
    {
        float maxChance = 0f;
        int indOfMax = 0;
        for(int i=0; i<bonuses.Length; i++)
        {
            float curChance = UnityEngine.Random.Range(0, bonuses[i].chance);
            if (curChance > maxChance)
            {
                maxChance = curChance;
                indOfMax = i;
            }
        }

        print(bonuses[indOfMax].bonusName);
        return bonuses[indOfMax].gameObject; 
    }

    public GameObject[] RandomBonusPanel()
    {
        GameObject[] bonuses = new GameObject[3];

        for(int k=0; k<3; k++)
        {
            GameObject bonusRand = RandomizeBonus();
            bonuses[k] = bonusRand;
        }

        return bonuses;
    }
}
