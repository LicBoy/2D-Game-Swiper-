using System.Linq;
using UnityEngine;

public class SkyBlocks : MonoBehaviour
{
    public int health = 25;
    public bool isBreakable = false;

    protected float counter = 0f;
    protected float bonusDuration;
    protected bool activated = false;
    protected Color originalColor;
    private int lastTouched;

    void Start()
    {
        originalColor = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.SetActive(false);
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
                activated = false;
                health = 25;
                gameObject.GetComponent<SpriteRenderer>().color = originalColor;
                gameObject.SetActive(false);
            }
        }
    }

    public void ActivateBlocks(float duration, bool isBreak)
    {
        isBreakable = isBreak;
        bonusDuration = duration;
        if (activated)
        {
            gameObject.SetActive(true);
            counter = 0;
        }
            
        else
        {
            activated = true;
            gameObject.SetActive(true);
        }
    }

    void FastBlink(Color colorA, Color colorB)
    {
        float val = Mathf.PingPong(Time.time * 10, 1);
        gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(colorA, colorB, val);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(isBreakable)
        {
            ProjectileBehaviour projObj = other.gameObject.GetComponent<ProjectileBehaviour>();
            if (projObj != null && lastTouched != other.gameObject.GetInstanceID())
            {
                lastTouched = other.gameObject.GetInstanceID();
                GetDamage(projObj.damage);
            }
        }
    }

    public void GetDamage(int val)
    {
        health -= val;
        if (health <= 0)
            DestroyBlock();
    }

    public void DestroyBlock()
    {
        health = 25;
        gameObject.GetComponent<SpriteRenderer>().color = originalColor;
        gameObject.SetActive(false);
    }
}
