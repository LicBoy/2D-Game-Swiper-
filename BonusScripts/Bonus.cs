using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public string[] namesTranslations = new string[2];
    public string bonusName;

    public string[] activationsTranslations = new string[2];
    public string bonusActivationPhrase;

    public string[] previewTranslations = new string[2];
    public string bonusPreviewText;
    
    public float chance;
    public Sprite bonusIcon;
    private BonusPreviewScript bonusPreview;

    public virtual void ActivateBonus()
    {
        bonusPreview = GameObject.FindGameObjectWithTag("BonusPreviewText").GetComponent<BonusPreviewScript>();

        //Do something
        if (bonusPreview == null)
        {
            print("Bonus preveiw obj wasn't found");
        }
        else
        {
            print("Bonus preview activated by " + bonusName);
            bonusPreview.ShowBonusPreview(bonusActivationPhrase);
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Player>())
        {
            ActivateBonus();
        }
    }

    public virtual bool HasDuration()
    {
        return false;
    }
}