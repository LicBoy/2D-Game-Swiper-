using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    private Camera mainCam;
    private float[] bordersX = new float[2] {-2.5f, 2.5f };
    private float generateCounter = 0f;
    private int curWave = 1;
    private bool changingLevel = false;
    private float t = 0;

    private BonusGenerator bonusGenerator;

    public GameObject projectile;
    public GameObject city;
    public GameObject blocks;
    public GameObject lineRenderer;
    public float coldown = 3f;
    public float changeColorTime = 15f;
    public float changeWavePauseTime = 5f;
    public int projesKilled = 0;
    public int totalProjsKilled = 0;
    public int curAmountOfProjs = 50;
    public int projsToAddPerWave = 10;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        { // Экземпляр менеджера был найден
            instance = this; // Задаем ссылку на экземпляр объекта
        }
        else if (instance == this)
        { // Экземпляр объекта уже существует на сцене
            Destroy(gameObject); // Удаляем объект
        }

        // Теперь нам нужно указать, чтобы объект не уничтожался
        // при переходе на другую сцену игры
        DontDestroyOnLoad(gameObject);

        mainCam = Camera.main.GetComponent<Camera>();
        bonusGenerator = GetComponent<BonusGenerator>();

        print(Screen.width + " " + Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        if (projesKilled >= curAmountOfProjs)
        {
            StartCoroutine(ChangeWave());
        }

        if (!changingLevel)
        {
            ChangeCamBackgroundByTime(Color.grey + new Color(0.3f, 0.3f, 0.3f, 0), Color.grey - new Color(0.4f, 0.4f, 0.4f, 0), changeColorTime);
            GenerateProjectiles();
        }

        else if(changingLevel)
        {
            ChangeCamBackgroundByTime(Color.grey - new Color(0.4f, 0.4f, 0.4f, 0), Color.grey + new Color(0.3f, 0.3f, 0.3f, 0), changeWavePauseTime);
        }
    }

    void ChangeCamBackgroundByTime(Color colorFrom, Color colorTo, float duration)
    {
        //float val = Mathf.PingPong(Time.time /10, 1);
        mainCam.backgroundColor = Color.Lerp(colorFrom, colorTo, t);
        if (t < 1)
        {
            t += Time.deltaTime / duration;
        } 
    }

    void GenerateProjectiles()
    {
        generateCounter += Time.deltaTime;
        if(generateCounter >= coldown)
        {
            generateCounter = UnityEngine.Random.Range(0f, 0.7f);
            GameObject newProjectile = GameObject.Instantiate(projectile, transform);
        }
    }

    public void IncreaseKills(int num)
    {
        projesKilled += num;
        totalProjsKilled += num;
        if(totalProjsKilled % 20 == 0)
        {
            bonusGenerator.DropBonus();
        }
    }

    IEnumerator ChangeWave()
    {
        changingLevel = true;
        t = 0;
        curAmountOfProjs += projsToAddPerWave + (int)(curWave * 0.5);
        print("Wave " + curWave + " has ended\nOn next wave you need to kill " + curAmountOfProjs + " projs");

        yield return new WaitForSeconds(changeWavePauseTime);
        changeColorTime += 5;

        projesKilled = 0;
        curWave++;
        coldown -= coldown * 0.03f;
        changingLevel = false;
        t = 0;
    }

    void GameOver()
    {
        print("GAME OVER");
        changingLevel = true;
    }

}
