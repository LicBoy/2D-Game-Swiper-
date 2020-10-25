using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public string bonusName;
    public float chance;

    public virtual void ActivateBonus()
    {
        //Do something
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<CityScript>())
        {
            ActivateBonus();
        }
    }
}