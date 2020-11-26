using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusPreviewScript : MonoBehaviour
{
    private TextMeshProUGUI previewText;
    private Animation anim;

    private void Start()
    {
        previewText = gameObject.GetComponent<TextMeshProUGUI>();
        anim = gameObject.GetComponent<Animation>();
    }

    public void ShowBonusPreview(string bonusText)
    {
        if(anim.isPlaying)
        {
            anim["BonusPreviewAnim"].time = 0;
            previewText.text += "\n" + bonusText;
        }
        else
        {
            previewText.text = bonusText;
            anim.Play();
        } 
    }
}
