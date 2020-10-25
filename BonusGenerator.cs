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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropBonus()
    {
        GameObject bonusObject = RandomizeBonus();
        if (bonusObject != null)
        {
            Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(spawnPosX[0], spawnPosX[1]), spawnPosY, 0);
            GameObject bonus = GameObject.Instantiate(bonusObject, spawnPos, Quaternion.identity);
        }
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
}
