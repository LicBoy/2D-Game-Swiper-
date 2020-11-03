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

    private bool gotDamaged = false;
    private float damageCounter = 0f;

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

    }

    // Update is called once per frame
    void Update()
    {
        if (gotDamaged)
        {
            DamageBlink(Color.white, Color.red);
            if (damageCounter > 1f)
            {
                gotDamaged = false;
                damageCounter = 0f;
            }

            damageCounter += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ProjectileBehaviour otherProjectileObject = other.GetComponent<ProjectileBehaviour>();
        if (otherProjectileObject != null)
        {
            GetDamage(otherProjectileObject.damage);
        }
    }

    void GetDamage(int value)
    {
        gotDamaged = true;
        damageCounter = 0f;

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

    public void DamageBlink(Color colorA, Color colorB)
    {
        float val = Mathf.PingPong(Time.time * 20, 1);
        GetComponent<SpriteRenderer>().color = Color.Lerp(colorA, colorB, val);
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
