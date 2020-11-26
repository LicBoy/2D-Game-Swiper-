using System;
using System.Collections;
using System.Reflection;
using Unity.Profiling;
using UnityEngine;

public class BonusGenerator : MonoBehaviour
{
    public Bonus[] bonuses = new Bonus[5];
    public int amountOfDrops = 1;
    public float[] bonusChancesOriginal;

    private float[] spawnPosX = new float[2] { -2.5f, 2.5f };
    private float spawnPosY = 5.5f;

    private void Start()
    {
        bonusChancesOriginal = new float[bonuses.Length];
        for (int i = 0; i < bonusChancesOriginal.Length; i++)
            bonusChancesOriginal[i] = bonuses[i].chance;
    }

    public void DropBonusAmountOfDrops(GameObject knownBonus = null)
    {
        GameObject[] droppedBonuses = new GameObject[amountOfDrops];

        for(int i=0; i<amountOfDrops; i++)
        {
            Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(spawnPosX[0], spawnPosX[1]), spawnPosY + UnityEngine.Random.Range(-0.5f, 0.5f), 0);

            if (i == 0  && knownBonus != null)
            {
                droppedBonuses[0] = knownBonus;
                print("i=0 Known bonus is not null: " + knownBonus.GetComponent<Bonus>().bonusName);
                GameObject.Instantiate(droppedBonuses[i], spawnPos, Quaternion.identity);
                continue;
            }
            GameObject randomizedBonus = RandomizeBonus();
            while (CheckIfSameTypeOfBonusInArray(randomizedBonus, droppedBonuses, i))
                randomizedBonus = RandomizeBonus();

            droppedBonuses[i] = randomizedBonus;
            GameObject.Instantiate(droppedBonuses[i], spawnPos, Quaternion.identity);
        }

        ReturnOriginalChances();
    }

    private bool CheckIfSameTypeOfBonusInArray(GameObject bonus, GameObject[] array, int curIndex)
    {
        for(int i=0; i<curIndex; i++)
        {
            if (array[i].GetComponent<Bonus>().bonusName == bonus.GetComponent<Bonus>().bonusName)
                return true;
        }
        return false;
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
                bonuses[i].chance /= 2;
                maxChance = curChance;
                indOfMax = i;
            }
        }

        print(bonuses[indOfMax].bonusName + " randomed with chance of " + bonuses[indOfMax].chance);
        return bonuses[indOfMax].gameObject; 
    }

    public GameObject[] RandomBonusPanel()
    {
        GameObject[] bonuses = new GameObject[3];
        float[] chances = new float[3];

        for(int k=0; k<3; k++)
        {
            GameObject bonusRand = RandomizeBonus();
            chances[k] = bonusRand.GetComponent<Bonus>().chance;
            bonuses[k] = bonusRand;
        }

        ReturnOriginalChances();
        return bonuses;
    }

    void ReturnOriginalChances()
    {
        for (int i = 0; i < bonuses.Length; i++)
            bonuses[i].chance = bonusChancesOriginal[i];
    }

    public void ChangeBonusesDuration(int wave)
    {
        for(int i=0; i<bonuses.Length; i++)
        {
            if (bonuses[i].HasDuration())
                bonuses[i].GetComponent<BonusWithDuration>().duration = 30 + wave;
        }
    }

    public IEnumerator ActivateBonusesDoublerBonus(float duration)
    {
        print("Extra bonus added");
        amountOfDrops += 1;
        yield return new WaitForSeconds(duration);
        amountOfDrops -= 1;
        print("Extra bonus decreased");
    }
}
