using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    private Camera mainCam;
    private float[] bordersX = new float[2] {-2.5f, 2.5f };
    private float generateCounter = 0f;
    private float t = 0;
    private int bonusDoubler = 1;
    private int[] defendedProjsIndexes;

    private BonusGenerator bonusGenerator;
    private ProjectilesGenerator projGenerator;

    public Player player;
    public SkyBlocks[] blocks = new SkyBlocks[2];
    public GameOverUI gameoverUI;
    public GameObject lineRenderer;
    public BackgroundBehaviour backgroundController;
    public ChangeWaveUI changeWaveUI;
    public float changeColorTime = 15f;
    public float changeWavePauseTime = 5f;
    public int projesKilled = 0;
    public int totalProjsKilled = 0;
    public int curAmountOfProjs = 50;
    public int projsToAddPerWave = 10;
    public int projsToKillForBonus = 60;
    public bool changingLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        mainCam = Camera.main.GetComponent<Camera>();
        bonusGenerator = GetComponent<BonusGenerator>();
        projGenerator = GetComponent<ProjectilesGenerator>();

        StartCoroutine("LoadPlayerData"); //loading player data at the start of the scene
    }

    // Update is called once per frame
    void Update()
    {
        if (!changingLevel)
        {
            projGenerator.GenerateProjectiles();
        }
    }
    public void IncreaseKills(int swipeCounter)
    {
        projesKilled += 1;
        totalProjsKilled += 1;
        if (projesKilled >= curAmountOfProjs)
        {
            StartCoroutine(ChangeWave());
        }
        //num is always 1

        if (defendedProjsIndexes.Contains(projesKilled))
            projGenerator.DropDefendedProjectile();

        int countedScore = (swipeCounter > 2) ? 5 * swipeCounter : 10;
        player.ChangeScore(countedScore * bonusDoubler);
        if(totalProjsKilled % projsToKillForBonus == 0)
        {
            bonusGenerator.DropBonusAmountOfDrops();
        }
    }

    IEnumerator DoublePointsBonus(float time)
    {
        bonusDoubler = 2;
        yield return new WaitForSeconds(time);
        bonusDoubler = 1;
    }

    IEnumerator ChangeWave()
    {
        DestroyAllRemainingProjectiles();
        backgroundController.BackgroundDisappear();

        changingLevel = true;
        t = 0;
        player.ChangeWave();
        curAmountOfProjs += projsToAddPerWave + (int)(player.wave * 0.5);
        print("Wave " + (player.wave-1).ToString() + " has ended\nOn next wave you need to kill " + curAmountOfProjs + " projs");
        defendedProjsIndexes = projGenerator.CountDefendedAsteroidsAmount(curAmountOfProjs);
        for (int i = 0; i < blocks.Length; i++) //SKYBLOCKS
        {
            blocks[i].CalculateAmountOfHealth(player.wave);
        }
        bonusGenerator.ChangeBonusesDuration(player.wave); //Testing

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
        backgroundController.StartCoroutine("BackgroundAppear");
        changeColorTime += 5;
        projesKilled = 0;
        projGenerator.coldown -= projGenerator.coldown * 0.03f;
        changingLevel = false;
        t = 0;
    }

    IEnumerator LoadPlayerData()
    {
        projesKilled = 0;
        totalProjsKilled = 0;

        print("Loading player from " + Application.persistentDataPath);
        player.LoadPlayer();
        player.ChangeColorDependOnHP();
        for(int i=1; i<=player.wave; i++)
        {
            if (i == 1)
                continue;
            curAmountOfProjs += projsToAddPerWave + (int)(i * 0.5);
            projGenerator.coldown -= projGenerator.coldown * 0.03f;
            changeColorTime += 5;
        }

        for (int i = 0; i < blocks.Length; i++) //SKYBLOCKS
        {
            blocks[i].CalculateAmountOfHealth(player.wave);
        }
        bonusGenerator.ChangeBonusesDuration(player.wave);

        defendedProjsIndexes = projGenerator.CountDefendedAsteroidsAmount(curAmountOfProjs);
        changeWaveUI.StartCoroutine("ShowChangeWaveAnimation", player.wave);
        changingLevel = true;
        backgroundController.StartCoroutine("BackgroundAppear");
        yield return new WaitForSeconds(changeWavePauseTime);
        changingLevel = false;
    }

    public void GameOver()
    {
        DestroyAllRemainingProjectiles();

        projesKilled = 0;
        totalProjsKilled = 0;

        gameoverUI.scoreWord.text = Language.TranslationDictionary[PlayerPrefs.GetString(MainMenuScript.GameLanguage)]
            [SceneManager.GetActiveScene().buildIndex]
            [Language.PlayGameOverScoreWord];

        if (player.score > player.highscore)
        {
            player.highscore = player.score;
            gameoverUI.scoreWord.text = 
                Language.TranslationDictionary[PlayerPrefs.GetString(MainMenuScript.GameLanguage)]
                [SceneManager.GetActiveScene().buildIndex]
                [Language.PlayGameOverNewHighscoreWord];
        }
            
        if (player.wave > player.maxWave)
            player.maxWave = player.wave;

        lineRenderer.GetComponent<MouseMovement>().GameOverChanges();
        for (int i = 0; i < blocks.Length; i++) //SKYBLOCKS
        {
            blocks[i].GameOverChanges();
            blocks[i].CalculateAmountOfHealth(1);
        }
        changeWaveUI.GameOverChanges();

        gameoverUI.gameObject.SetActive(true);
        gameoverUI.scoreText.text = player.score.ToString();
        gameoverUI.ShowGameOverMenu();

        print("GAME OVER");
        changingLevel = true;
    }

    public void GameOverPlayerStats()
    {
        player.health = 100;
        player.armor = 0;
        player.wave = 1;
        player.score = 0;
        player.SavePlayer();
    }

    public void DestroyAllRemainingProjectiles()
    {
        ProjectileBehaviour[] remainingProjes = gameObject.GetComponentsInChildren<ProjectileBehaviour>();
        foreach (ProjectileBehaviour proj in remainingProjes)
            proj.DestroyProjectile();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
