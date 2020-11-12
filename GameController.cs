using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    private Camera mainCam;
    private float[] bordersX = new float[2] {-2.5f, 2.5f };
    private float generateCounter = 0f;
    private float t = 0;

    private BonusGenerator bonusGenerator;
    private ChangeWaveUI changeWaveUI;

    public Player player;
    public SkyBlocks[] blocks = new SkyBlocks[2];
    public GameOverUI gameoverUI;
    public GameObject projectile;
    public GameObject lineRenderer;
    public float coldown = 3f;
    public float changeColorTime = 15f;
    public float changeWavePauseTime = 5f;
    public int projesKilled = 0;
    public int totalProjsKilled = 0;
    public int curAmountOfProjs = 50;
    public int projsToAddPerWave = 10;
    public bool changingLevel = false;

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
        changeWaveUI = GetComponent<ChangeWaveUI>();

        StartCoroutine("LoadPlayerData"); //loading player data at the start of the scene
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

    public void IncreaseKills(int swipeCounter)
    {
        projesKilled += 1;
        totalProjsKilled += 1;
        //num is always 1
        int countedScore = (swipeCounter > 2) ? 5 * swipeCounter : 10;
        player.ChangeScore(countedScore);
        if(totalProjsKilled % 20 == 0)
        {
            bonusGenerator.DropBonus();
        }
    }

    IEnumerator ChangeWave()
    {
        ProjectileBehaviour[] remainingProjes = gameObject.GetComponentsInChildren<ProjectileBehaviour>();
        foreach (ProjectileBehaviour proj in remainingProjes)
            proj.DestroyProjectile();

        changingLevel = true;
        t = 0;
        player.ChangeWave();
        curAmountOfProjs += projsToAddPerWave + (int)(player.wave * 0.5);
        print("Wave " + (player.wave-1).ToString() + " has ended\nOn next wave you need to kill " + curAmountOfProjs + " projs");

        //Changing player data if needed
        if (player.score > player.highscore)
            player.highscore = player.score;
        if (player.wave > player.maxWave)
            player.maxWave = player.wave;
        print("Cur highscore: " + player.highscore + "\nCur MaxWave: " + player.maxWave);
        player.SavePlayer(); //saving player on wave change

        changeWaveUI.StartCoroutine("ShowChangeWaveAnimation", player.wave);
        changeWaveUI.BonusPanelAppear();
        yield return new WaitForSeconds(changeWavePauseTime);
        changeColorTime += 5;

        projesKilled = 0;
        coldown -= coldown * 0.03f;
        changingLevel = false;
        t = 0;
    }

    IEnumerator LoadPlayerData()
    {
        projesKilled = 0;
        totalProjsKilled = 0;

        print("Loading player from " + Application.persistentDataPath);
        player.LoadPlayer();
        for(int i=1; i<=player.wave; i++)
        {
            if (i == 1)
                continue;
            curAmountOfProjs += projsToAddPerWave + (int)(i * 0.5);
            coldown -= coldown * 0.03f;
            changeColorTime += 5;
        }

        changeWaveUI.StartCoroutine("ShowChangeWaveAnimation", player.wave);
        changingLevel = true;
        yield return new WaitForSeconds(changeWavePauseTime);
        changingLevel = false;
    }

    public void GameOver()
    {
        gameoverUI.scoreWord.text = "Score";

        if (player.score > player.highscore)
        {
            player.highscore = player.score;
            gameoverUI.scoreWord.text = "New highscore!";
        }
            
        if (player.wave > player.maxWave)
            player.maxWave = player.wave;

        lineRenderer.GetComponent<MouseMovement>().GameOverChanges();
        for (int i = 0; i < blocks.Length; i++)
            blocks[i].GameOverChanges();
        changeWaveUI.GameOverChanges();

        gameoverUI.panel.SetActive(true);
        gameoverUI.scoreText.text = player.score.ToString();
        gameoverUI.ShowGameOverMenu();

        player.health = 100;
        player.armor = 0;
        player.wave = 1;
        player.score = 0;
        player.SavePlayer();
        print("GAME OVER");
        changingLevel = true;
    }
}
