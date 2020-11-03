using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public string bonusName;
    public string bonusActivationPhrase;
    public float chance;
    public Sprite bonusIcon;

    public virtual void ActivateBonus()
    {
        //Do something
        BonusPreviewScript bonusPreview = GameObject.FindGameObjectWithTag("BonusPreviewText").GetComponent<BonusPreviewScript>();
        bonusPreview.ShowBonusPreview(bonusActivationPhrase);
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Player>())
        {
            ActivateBonus();
        }
    }
}