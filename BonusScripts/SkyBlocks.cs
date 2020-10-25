using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBlocks : MonoBehaviour
{
    public GameObject[] blocks = new GameObject[2];
    private float counter = 0f;
    private float bonusDuration;
    private bool activated = false;
    private Color originalColor;

    private void Start()
    {
        foreach(GameObject obj in blocks)
        {
            obj.SetActive(false);
        }
        originalColor = blocks[0].GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if(activated)
        {
            counter += Time.deltaTime;
            if (counter > bonusDuration - 1)
                FastBlink(originalColor, originalColor - new Color(0, 0, 0, 1));
                
            if (counter > bonusDuration)
            {
                counter = 0;
                print("End of skyblocks");
                activated = false;
                foreach (GameObject obj in blocks)
                {
                    obj.SetActive(false);
                    obj.GetComponent<SpriteRenderer>().color = originalColor;
                }
            }
        }
    }

    void ActivateBlocks(float duration)
    {
        bonusDuration = duration;
        print("BLOX ACTIVATED");
        if (activated)
            counter = 0;
        else
        {
            activated = true;
            foreach(GameObject obj in blocks)
            {
                obj.SetActive(true);
            }
        }
    }

    void FastBlink(Color colorA, Color colorB)
    {
        float val = Mathf.PingPong(Time.time * 10, 1);
        foreach(GameObject obj in blocks)
            obj.GetComponent<SpriteRenderer>().color = Color.Lerp(colorA, colorB, val);
    }
}
