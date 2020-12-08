using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorLineColliderScript : MonoBehaviour
{
    public bool isBlinking;
    private float counter;

    public int oneSwipeKillCounterMirror = 0;
    public float blinkDuration = 3f;
    public Gradient defaultColorGradient = new Gradient();
    public Gradient biggerLineGradient = new Gradient();
    public Gradient killedProjGradient = new Gradient();
    public Gradient disappearGradient = new Gradient();
    public StreakUIController streakController;

    void Start()
    {
        //GetComponent<LineRenderer>().colorGradient = defaultColorGradient;
    }

    void Update()
    {
        if(isBlinking)
        {
            DisappearBlink(GetComponent<LineRenderer>().colorGradient);
            if (counter > blinkDuration)
                counter = 0; 
            counter += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ProjectileBehaviour otherProjectileObject = other.GetComponent<ProjectileBehaviour>();
        if (otherProjectileObject != null && otherProjectileObject.health > 0)
        {
            if(otherProjectileObject.GetDamage(1))
            {
                GameController.instance.lineRenderer.gameObject.GetComponent<MouseMovement>().oneSwipeKillsCounter += 1;
                GameController.instance.IncreaseKills(1);

                if (GameController.instance.lineRenderer.gameObject.GetComponent<MouseMovement>().oneSwipeKillsCounter > 2)
                {
                    streakController.ShowStreak(GameController.instance.lineRenderer.gameObject.GetComponent<MouseMovement>().oneSwipeKillsCounter);
                }
            }
        }
    }

    public void ChangeColorToBigger()
    {
        GetComponent<LineRenderer>().colorGradient = biggerLineGradient;
    }

    public void ChangeColorToKilledProj()
    {
        GetComponent<LineRenderer>().colorGradient = killedProjGradient;
    }

    public void ChangeColorToDefault(bool isBigger)
    {
        if (isBigger)
            GetComponent<LineRenderer>().colorGradient = biggerLineGradient;
        else
            GetComponent<LineRenderer>().colorGradient = defaultColorGradient;
    }

    public void DisappearBlink(Gradient gradientWas)
    {
        float val = Mathf.PingPong(Time.time * 30, 1);
        if (val > 0.5)
            GetComponent<LineRenderer>().colorGradient = gradientWas;
        else
            GetComponent<LineRenderer>().colorGradient = disappearGradient;
    }

}
