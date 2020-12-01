using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    private int bonusDoubler = 1;
    private int[] defendedProjsIndexes;

    private BonusGenerator bonusGenerator;
    private ProjectilesGenerator projGenerator;

    public Player player;
    public SkyBlocks[] blocks = new SkyBlocks[2];
    public GameObject[] walls = new GameObject[2];
    public GameOverUI gameoverUI;
    public GameObject lineRenderer;
    public BackgroundBehaviour backgroundController;
    public ChangeWaveUI changeWaveUI;
    public float changeWavePauseTime = 5f;
    public int projesKilled = 0;
    public int totalProjsKilled = 0;
    public int curAmountOfProjs = 50;
    public int projsToAddPerWave = 10;
    public int projsToKillForBonus = 60;
    public bool changingLevel = false;

    public Vector3 cityScale;
    private float originalResAspect = 800f / 480f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        bonusGenerator = GetComponent<BonusGenerator>();
        projGenerator = GetComponent<ProjectilesGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!changingLevel && SceneManager.GetActiveScene().buildIndex == 1)
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
            ChangeWave();
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

    public void ChangeWave()
    {
        StartCoroutine("DestroyAllRemainingProjectiles");

        player.ChangeWave();
        print("Wave " + (player.wave-1).ToString() + " has ended\nOn next wave you need to kill " + curAmountOfProjs + " projs");
        defendedProjsIndexes = projGenerator.CountDefendedAsteroidsAmount(curAmountOfProjs);
        for (int i = 0; i < blocks.Length; i++) //SKYBLOCKS
        {
            blocks[i].CalculateAmountOfHealth(player.wave);
        }
        bonusGenerator.ChangeBonusesDuration(player.wave);

        //Changing player data if needed
        if (player.score > player.highscore)
            player.highscore = player.score;
        if (player.wave > player.maxWave)
            player.maxWave = player.wave;
        print("Cur highscore: " + player.highscore + "\nCur MaxWave: " + player.maxWave);
        player.SavePlayer(); //saving player on wave change

        changeWaveUI.StartCoroutine("BonusPanelAppear", player.wave);
        projesKilled = 0;
        curAmountOfProjs += projsToAddPerWave + (int)(player.wave * 0.5);
        projGenerator.coldown -= projGenerator.coldown * 0.03f;
    }


    IEnumerator LoadPlayerData()
    {
        gameoverUI = FindObjectOfType<GameOverUI>();
        backgroundController = FindObjectOfType<BackgroundBehaviour>();
        changeWaveUI = FindObjectOfType<ChangeWaveUI>();
        lineRenderer.GetComponent<MouseMovement>().StreakUIController = FindObjectOfType<StreakUIController>();
        lineRenderer.GetComponent<MouseMovement>().mirrorLine.GetComponent<MirrorLineColliderScript>().streakController = FindObjectOfType<StreakUIController>();
        backgroundController.SetTheme();
        projGenerator.SetCurrentTheme();

        float newAspect = (float)Screen.height / (float)Screen.width;
        player.transform.localScale = cityScale * (originalResAspect / newAspect) + new Vector3(0.01f, 0.01f, 0.01f);
        player.transform.position = new Vector3(player.transform.position.x, -5, 0);
        print("Original aspect: " + originalResAspect + "\nCurrent resolution: " + newAspect + "\n Calculated scale: " + player.transform.localScale);
        walls[0].transform.position = new Vector3(walls[0].transform.position.x * (originalResAspect / newAspect) - 0.1f,
            walls[0].transform.position.y,
            walls[0].transform.position.z);
        walls[1].transform.position = new Vector3(walls[1].transform.position.x * (originalResAspect / newAspect) + 0.1f,
            walls[1].transform.position.y,
            walls[1].transform.position.z);


        projesKilled = 0;
        totalProjsKilled = 0;

        print("Loading player from " + Application.persistentDataPath);
        player.LoadPlayer();
        player.ChangeColorDependOnHP();

        curAmountOfProjs = 50; projGenerator.coldown = 1;
        for (int i=2; i<=player.wave; i++)
        {
            curAmountOfProjs += projsToAddPerWave + (int)(i * 0.5);
            projGenerator.coldown -= projGenerator.coldown * 0.03f;
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
        StartCoroutine("DestroyAllRemainingProjectiles");

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

        lineRenderer.GetComponent<MouseMovement>().RemoveAllBonuses();
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

    public IEnumerator DestroyAllRemainingProjectiles()
    {
        ProjectileBehaviour[] remainingProjes = gameObject.GetComponentsInChildren<ProjectileBehaviour>();
        foreach (ProjectileBehaviour proj in remainingProjes)
        {
            proj.GetComponent<Collider2D>().enabled = false;
        }

        foreach (ProjectileBehaviour proj in remainingProjes)
        {
            if (proj != null)
            {
                proj.DestroyProjectile();
                yield return new WaitForSeconds(0.2f);
            }
        }

    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
