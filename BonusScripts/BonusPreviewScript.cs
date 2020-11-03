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
        previewText.text = bonusText;
        anim.Play();
    }
}
