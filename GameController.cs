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

    public GameObject projectile;
    public float coldown = 3f;
    public int projesKilled = 0;
    public int baseAmountOfProjs = 50;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (projesKilled >= baseAmountOfProjs)
        {
            StartCoroutine(ChangeWave());
        }

        if (!changingLevel)
        {
            ChangeCamBackgroundByTime();
            GenerateProjectiles();
        }
    }

    void ChangeCamBackgroundByTime()
    {
        float val = Mathf.PingPong(Time.time /10, 1);
        mainCam.backgroundColor = Color.Lerp(Color.blue, Color.white, val);
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
        
    }

    IEnumerator ChangeWave()
    {
        changingLevel = true;
        baseAmountOfProjs += projsToAddPerWave + (int)(curWave * 0.5);
        print("Wave " + curWave + " has ended\nOn next wave you need to kill " + baseAmountOfProjs + " projs");

        yield return new WaitForSeconds(3);

        projesKilled = 0;
        curWave++;
        coldown -= coldown * 0.03f;
        changingLevel = false;
    }

}
