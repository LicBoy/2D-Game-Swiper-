using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{
    private UnityEngine.UI.Scrollbar scroller;
    private RectTransform rectTransform;
    private Animation animation;
    private Vector2 startPos;

    public float disappearVal = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        scroller = GetComponent<Scrollbar>();
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        animation = GetComponent<Animation>();

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClick()
    {
        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            animation.Play("OptionsAppearAnim");
        }
    }

    public void EndDrag()
    {
        if (scroller.value > disappearVal)
        {
            animation.Play("OptionsDisappearAnim");
            scroller.interactable = false;
        }  
        else
        {
            scroller.value = 0;
            animation.Play("OptionsBackMoveAnim");
        }
    }

    public void OnDragEnd()
    {
        scroller.value = 0;
        gameObject.SetActive(false);
        rectTransform.anchoredPosition = startPos;
        scroller.interactable = true;
    }

}
