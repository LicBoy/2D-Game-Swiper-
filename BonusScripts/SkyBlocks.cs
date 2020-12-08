using System.Linq;
using UnityEngine;

public class SkyBlocks : MonoBehaviour
{
    public int health = 25;
    public bool isBreakable = false;
    public GameObject explosionAnim;
    public Color skyblocksStrongColor;
    public Color skyblocksWeakColor;

    private float counter = 0f;
    private float bonusDuration;
    public bool activated = false;
    private Color originalColor;
    private int lastTouched;
    private int curWaveHealth = 25;

    void Start()
    {
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
                GetComponentInParent<SkyBlocksAnimationsScript>().PlayDisappearAnimation();
            }
        }
    }

    public void ActivateBlocks(float duration, bool isBreak)
    {
        isBreakable = isBreak;
        ApplyCorrectColor();
        bonusDuration = duration;
        if (activated)
        {
            counter = 0;
            GetComponentInParent<SkyBlocksAnimationsScript>().RenewBonusTimeAnimation();
        }
            
        else
        {
            GetComponentInParent<SkyBlocksAnimationsScript>().PlayAppearIdleAnimation();
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
        FastBlink(Color.red, Color.clear);
        gameObject.GetComponent<SpriteRenderer>().color = originalColor;
        if (health <= 0)
            DestroyBlock();
    }

    public void DestroyBlock()
    {
        health = curWaveHealth;
        gameObject.GetComponent<SpriteRenderer>().color = originalColor;
        GameObject explodeObj = Instantiate(explosionAnim, transform.position, Quaternion.identity);
        Destroy(explodeObj, 3f);
        gameObject.SetActive(false);
        
    }

    public void ApplyCorrectColor()
    {
        originalColor = isBreakable ? skyblocksWeakColor : skyblocksStrongColor;
        GetComponent<SpriteRenderer>().color = originalColor;
    }

    public void CalculateAmountOfHealth(int curWave)
    {
        curWaveHealth = curWaveHealth + curWave / 2 * 5;
        health = curWaveHealth;
    }

    public void RemoveBonus()
    {
        curWaveHealth = 25;
        health = curWaveHealth;
        counter = bonusDuration;
    }

}
