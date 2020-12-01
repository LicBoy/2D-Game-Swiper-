using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    public int armor;
    public int wave;
    public int maxWave;
    public int highscore;
    public int score;

    public Color fullHealthColor;
    public Color lowestHealthColor;
    public GameObject explosion;

    private float redChange;
    private float greenChange;
    private float blueChange;
    private Animation animation;
    private SpriteRenderer spriteRenderer;

    public Player()
    {
        this.health = 100;
        this.armor = 0;
        this.wave = 1;
        this.maxWave = 0;
        this.highscore = 0;
        this.score = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        redChange = (fullHealthColor.r - lowestHealthColor.r) / 100;
        greenChange = (fullHealthColor.g - lowestHealthColor.g) / 100;
        blueChange = (fullHealthColor.b - lowestHealthColor.b) / 100;

        animation = GetComponent<Animation>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SavePlayer(bool firstSave = false)
    {
        if(firstSave)
        {
            health = 100;
            armor = 0;
            wave = 1;
            maxWave = 0;
            highscore = 0;
            score = 0;
        }
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        health = data.health;
        armor = data.armor;
        wave = data.wave;
        maxWave = data.maxWave;
        highscore = data.highscore;
        score = data.score;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ProjectileBehaviour otherProjectileObject = other.GetComponent<ProjectileBehaviour>();
        if (otherProjectileObject != null)
        {
            GetDamage(otherProjectileObject.damage);
        }
    }

    public void ChangeColorDependOnHP()
    {
        animation.Stop();
        spriteRenderer.color = new Color(
            (lowestHealthColor.r + health * redChange),
            (lowestHealthColor.g + health * greenChange),
            (lowestHealthColor.b + health * blueChange),
            1);
    }

    void GetDamage(int value)
    {

        if (armor > 0)
        {
            armor -= value;
            PlayArmorDamageAnim();

            if (armor < 0)
            {
                health += armor;
                armor = 0;
                PlayHealthDamageAnim();
            }
        }
        else
        {
            health -= value;
            PlayHealthDamageAnim();
        }

        if (health <= 0)
        {
            StartCoroutine("ExplodeAnimation", 0.3f);
            GameController.instance.GameOver();
        }
    }

    public IEnumerator ExplodeAnimation(float dur)
    {
        Vector3 newScale = new Vector3(1.5f, 1.5f, 1);
        Vector3 firstSpawn;
        Vector3 secondSpawn;
        Vector3 thirdSpawn;

        float random = UnityEngine.Random.Range(0, 1f);
        if (random < 0.333f) //If first explosion randomed
        {
            firstSpawn = new Vector3(-2, -4, 0);

            random = UnityEngine.Random.Range(0, 1f);
            secondSpawn = random < 0.5 ? new Vector3(0, -4, 0) : new Vector3(2, -4, 0);
            thirdSpawn = random < 0.5 ? new Vector3(2, -4, 0) : new Vector3(0, -4, 0);
        }
        else if (random < 0.666f) // If second explosion
        {
            firstSpawn = new Vector3(0, -4, 0);
            random = UnityEngine.Random.Range(0, 1f);
            secondSpawn = random < 0.5 ? new Vector3(-2, -4, 0) : new Vector3(2, -4, 0);
            thirdSpawn = random < 0.5 ? new Vector3(2, -4, 0) : new Vector3(-2, -4, 0);
        }
        else
        {
            firstSpawn = new Vector3(2, -4, 0);
            random = UnityEngine.Random.Range(0, 1f);
            secondSpawn = random < 0.5 ? new Vector3(-2, -4, 0) : new Vector3(0, -4, 0);
            thirdSpawn = random < 0.5 ? new Vector3(0, -4, 0) : new Vector3(-2, -4, 0);
        }

        yield return new WaitForSeconds(dur);
        GameObject firstExplosion = GameObject.Instantiate(explosion, firstSpawn, Quaternion.identity);
        firstExplosion.transform.localScale = newScale; Destroy(firstExplosion, 3);
        yield return new WaitForSeconds(dur);
        GameObject secondExplosion = GameObject.Instantiate(explosion, secondSpawn, Quaternion.identity);
        secondExplosion.transform.localScale = newScale; Destroy(secondExplosion, 3);
        yield return new WaitForSeconds(dur);
        GameObject thirdExplosion = GameObject.Instantiate(explosion, thirdSpawn, Quaternion.identity);
        thirdExplosion.transform.localScale = newScale; Destroy(thirdExplosion, 3);

        GameController.instance.GameOverPlayerStats();
    }

    void PlayArmorDamageAnim()
    {
        if (!animation.IsPlaying("CityArmorDamageAnimation"))
            animation.Play("CityArmorDamageAnimation");
        else if (animation.IsPlaying("CityArmorDamageAnimation"))
            animation["CityArmorDamageAnimation"].time = 0;
    }

    void PlayHealthDamageAnim()
    {
        if (!animation.IsPlaying("CityDamagedAnimation"))
            animation.Play("CityDamagedAnimation");
        else if (animation.IsPlaying("CityDamagedAnimation"))
            animation["CityDamagedAnimation"].time = 0;
    }

    public void Heal(int val)
    {
        health = Mathf.Clamp(health + val, 0, 100);
        ChangeColorDependOnHP();
        print("Healed for " + val + " hp");
    }

    public void GiveArmor(int val)
    {
        int armorWas = armor;
        armor = Mathf.Clamp(armor + val, 0, 50);
        if(armorWas != armor)
        {
            animation.Play("CityArmorGainAnim");
            print("Added " + val + " armor");
        }
    }

    public void ChangeWave()
    {
        wave += 1;
    }

    public void ChangeScore(int val)
    {
        score += val;
    }
}
