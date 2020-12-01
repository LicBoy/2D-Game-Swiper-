using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

public class MouseMovement : MonoBehaviour
{
    private LineRenderer linerend;
    private EdgeCollider2D edgeCollider;
    private float counter = 0f;
    private float counterOfLineDurBonus = 0;
    private float lineDurBonusDuration;
    private float counterOfLineBiggerBonus = 0;
    private float lineBiggerBonusDuration;
    private float counterMirrorLineBonus = 0;
    private float mirrorLineBonusDuration;
    private float counterVampirismBonus = 0;
    private float vampirismBonusDuration;
    private int vampirismHealAmount;
    private int lineDamage = 1;
    private float counterOfLive = 0f;
    private float curveWidthMultiplayer;
    private float defaultTimeToLive;
    private int ind = 0;
    private bool killedProj = false;
    private bool lineDurationBonusActive = false;
    private bool lineBiggerBonusActive = false;
    private bool mirrorLineBonusActive = false;
    private bool vampirismBonusActive = false;
    private Gradient currentLineGradient;
    private Gradient defaultLineGradient;

    public float timeToLive = 0.5f;
    public int oneSwipeKillsCounter = 0;
    public LineRenderer mirrorLine;
    public StreakUIController StreakUIController;
    public Gradient bigLineGradient = new Gradient();
    public Gradient killedProjLineGradient = new Gradient();

    // Start is called before the first frame update
    void Start()
    {
        linerend = GetComponent<LineRenderer>();
        curveWidthMultiplayer = linerend.widthMultiplier;
        defaultLineGradient = linerend.colorGradient;
        currentLineGradient = defaultLineGradient;
        defaultTimeToLive = timeToLive;

        edgeCollider = GetComponent<EdgeCollider2D>();

        mirrorLine.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(lineDurationBonusActive)
        {
            if (counterOfLineDurBonus > lineDurBonusDuration)
            {
                lineDurationBonusActive = false;
                counterOfLineDurBonus = 0;
                timeToLive = defaultTimeToLive;
            }
            counterOfLineDurBonus += Time.deltaTime;
        }

        if (lineBiggerBonusActive)
        {
            if (counterOfLineBiggerBonus > lineBiggerBonusDuration)
            {
                lineDamage = 1;
                lineBiggerBonusActive = false;
                counterOfLineBiggerBonus = 0;
                curveWidthMultiplayer /= 2;
                edgeCollider.edgeRadius -= 0.5f;
                linerend.colorGradient = defaultLineGradient;
                currentLineGradient = defaultLineGradient;
                mirrorLine.GetComponent<MirrorLineColliderScript>().ChangeColorToDefault(lineBiggerBonusActive);
            }
            counterOfLineBiggerBonus += Time.deltaTime;
        }

        if (mirrorLineBonusActive)
        {
            if (counterMirrorLineBonus > mirrorLineBonusDuration-3)
            {
                mirrorLine.GetComponent<MirrorLineColliderScript>().isBlinking = true;
                if (counterMirrorLineBonus > mirrorLineBonusDuration)
                {
                    mirrorLineBonusActive = false;
                    counterMirrorLineBonus = 0;
                    mirrorLine.GetComponent<MirrorLineColliderScript>().isBlinking = false;
                    mirrorLine.gameObject.SetActive(false);
                }
            }
            counterMirrorLineBonus += Time.deltaTime;
        }

        if (vampirismBonusActive)
        {
            if (counterVampirismBonus > vampirismBonusDuration)
            {
                vampirismBonusActive = false;
                counterVampirismBonus = 0;
            }
            counterVampirismBonus += Time.deltaTime;
        }

        DrawLine(mirrorLineBonusActive);
    }


    private void DrawLine(bool mirrorBonusActive)
    {
        if (Input.GetMouseButton(0) && counterOfLive < timeToLive)
        {
            //linerend.SetPosition(1, GetMouseVector3());
            if (counter >= 0.01f) //Time before creating new vertice in Line
            {
                linerend.positionCount = ind + 1;
                Vector3 mousePos = GetMouseVector3();
                linerend.SetPosition(ind, mousePos);

                if (mirrorBonusActive)
                {
                    mirrorLine.positionCount = ind + 1;
                    Vector3 mouseMirrorPos = mousePos;
                    mouseMirrorPos = new Vector3(-mouseMirrorPos.x, mouseMirrorPos.y, mouseMirrorPos.z);
                    mirrorLine.SetPosition(ind, mouseMirrorPos);
                }

                ind += 1;
                counter = 0f;
            }

            Vector3[] newPos = new Vector3[linerend.positionCount];
            linerend.GetPositions(newPos);
            edgeCollider.points = ConvertArray(newPos);

            if(mirrorBonusActive)
            {
                Vector3[] newPosMirror = new Vector3[mirrorLine.positionCount];
                mirrorLine.GetPositions(newPosMirror);
                mirrorLine.GetComponent<EdgeCollider2D>().points = ConvertArray(newPosMirror);
            }

            if (killedProj)
            {
                linerend.colorGradient = killedProjLineGradient;
                if (mirrorBonusActive)
                    mirrorLine.GetComponent<MirrorLineColliderScript>().ChangeColorToKilledProj();
            }

            counter += Time.deltaTime;
            counterOfLive += Time.deltaTime;
        }

        else if (Input.GetMouseButtonUp(0) || counterOfLive >= timeToLive)
        {
            killedProj = false;
            oneSwipeKillsCounter = 0;
            mirrorLine.gameObject.GetComponent<MirrorLineColliderScript>().oneSwipeKillCounterMirror = 0;
            linerend.widthMultiplier = curveWidthMultiplayer;
            linerend.colorGradient = currentLineGradient;

            linerend.positionCount = 0;
            Vector2[] empty = new Vector2[2] {Vector2.zero, Vector2.zero};
            edgeCollider.points = empty;
            ind = 0;
            counterOfLive = 0f;

            if (mirrorBonusActive)
            {
                mirrorLine.positionCount = 0;
                mirrorLine.GetComponent<EdgeCollider2D>().points = empty;
                mirrorLine.GetComponent<MirrorLineColliderScript>().ChangeColorToDefault(lineBiggerBonusActive);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ProjectileBehaviour otherProjectileObject = other.GetComponent<ProjectileBehaviour>();
        if (otherProjectileObject != null && other.gameObject != null)
        {
            killedProj = otherProjectileObject.GetDamage(lineDamage);
            if(killedProj)
            {
                oneSwipeKillsCounter += 1;
                GameController.instance.IncreaseKills(oneSwipeKillsCounter);
                linerend.widthMultiplier *= oneSwipeKillsCounter;
                linerend.widthMultiplier = Mathf.Clamp(linerend.widthMultiplier, curveWidthMultiplayer, curveWidthMultiplayer * 3);
                if (oneSwipeKillsCounter > 2)
                {
                    StreakUIController.ShowStreak(oneSwipeKillsCounter);
                    if (vampirismBonusActive)
                        GameController.instance.player.Heal(vampirismHealAmount);
                        
                }
            }
        }
    }

    public void ChangeLineDuration(float duration, bool isInfinite = false)
    {
        lineDurBonusDuration = duration;
        lineDurationBonusActive = true;
        if (lineDurationBonusActive)
            counterOfLineDurBonus = 0;
        if (!isInfinite)
        {
            timeToLive *= 2;
        }
        else
        {
            timeToLive = duration;
        }
    }

    public void MakeLineBigger(float duration)
    {
        lineDamage = 10;
        lineBiggerBonusDuration = duration;
        if (lineBiggerBonusActive)
            counterOfLineBiggerBonus = 0;

        else
        {
            print("Line bigger started");
            curveWidthMultiplayer *= 2;
            edgeCollider.edgeRadius += 0.3f;
            lineBiggerBonusActive = true;
            currentLineGradient = bigLineGradient;
            linerend.colorGradient = bigLineGradient;
            mirrorLine.GetComponent<MirrorLineColliderScript>().ChangeColorToBigger();
        }
    }

    public void CreateMirrorLine(float duration)
    {
        mirrorLineBonusDuration = duration;
        if (mirrorLineBonusActive)
        {
            counterMirrorLineBonus = 0;
        }
        else
        {
            mirrorLine.positionCount = 0;
            Vector2[] empty = new Vector2[2] { Vector2.zero, Vector2.zero };
            mirrorLine.GetComponent<EdgeCollider2D>().points = empty;
            mirrorLine.gameObject.SetActive(true);
            mirrorLineBonusActive = true;
        }
    }

    public void ActivateVampirism(float duration, int healAmount)
    {
        vampirismHealAmount = healAmount;
        vampirismBonusDuration = duration;
        if (vampirismBonusActive)
            counterOfLineBiggerBonus = 0;

        else
        {
            print("Vampirism started");
            vampirismBonusActive = true;
        }
    }

    public void RemoveAllBonuses()
    {
        lineBiggerBonusActive = false;
        lineDurationBonusActive = false;
        mirrorLineBonusActive = false;
        vampirismBonusActive = false;
    }

    private Vector3 GetMouseVector3()
    {
        Vector3 vec = new Vector3(0, 0, 0);
        vec.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        vec.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        return vec;
    }

    Vector2[] ConvertArray(Vector3[] v3)
    {
        Vector2[] v2 = new Vector2[v3.Length];
        for (int i = 0; i < v3.Length; i++)
        {
            Vector3 tempV3 = v3[i];
            v2[i] = new Vector2(tempV3.x, tempV3.y);
        }
        return v2;
    }

}
