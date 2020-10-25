using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityScript : MonoBehaviour
{
    public int health = 100;
    public int armor = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
            GameController.instance.SendMessage("GameOver");
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
}
