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
    public string gameLanguage;
    public bool isDarkTheme;
    public bool soundOn;

    public Color fullHealthColor;
    public Color lowestHealthColor;

    private Animation animation;
    private SpriteRenderer spriteRenderer;

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

    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animation>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

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
        float redChange = (fullHealthColor.r - lowestHealthColor.r) / 100;
        float greenChange = (fullHealthColor.g - lowestHealthColor.g) / 100;
        float blueChange = (fullHealthColor.b - lowestHealthColor.b) / 100;

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
            if (armor < 0)
            {
                health += armor;
                armor = 0;
            }
        }
        else
        {
            health -= value;
        }

        if (health <= 0)
        {
            GameController.instance.GameOver();
        }

        if (!animation.isPlaying)
            animation.Play("CityDamagedAnimation");
        else if(animation.isPlaying)
            animation["CityDamagedAnimation"].time = 0;
      
    }

    public void Heal(int val)
    {
        health = Mathf.Clamp(health + val, 0, 100);
        print("Healed for " + val + " hp");
    }

    public void GiveArmor(int val)
    {
        armor = Mathf.Clamp(armor + val, 0, 50);
        print("Added " + val + " armor");
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
