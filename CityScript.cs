using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityScript : MonoBehaviour
{
    private bool gotDamaged = false;
    private float damageCounter = 0f;

    public int health = 100;
    public int armor = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gotDamaged)
        {
            DamageBlink(Color.white, Color.red);
            if(damageCounter > 1f)
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
        if(otherProjectileObject != null)
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
            if(armor < 0)
            {
                health += armor;
                armor = 0;
            }
        } else {
            health -= value;
        }

        if(health <= 0)
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
}
