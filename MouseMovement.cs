using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private float counterOfLive = 0f;
    private float curveWidthMultiplayer;
    private int ind = 0;
    private int oneSwipeKillsCounter = 0;
    private bool killedProj = false;
    private bool lineDurationBonusActive = false;
    private bool lineBiggerBonusActive = false;
    private bool mirrorLineBonusActive = false;
    private Gradient currentLineGradient;
    private Gradient defaultLineGradient;
    private Gradient bigLineGradient = new Gradient();
    private Gradient killedProjLineGradient = new Gradient();

    public float timeToLive = 0.5f;
    public GameObject gun;
    public GameObject mirrorLine;
    public Color[] bigLineColors = new Color[4];
    public Color[] killedProjColors = new Color[4];

    // Start is called before the first frame update
    void Start()
    {
        linerend = GetComponent<LineRenderer>();
        curveWidthMultiplayer = linerend.widthMultiplier;
        defaultLineGradient = linerend.colorGradient;
        currentLineGradient = defaultLineGradient;

        edgeCollider = GetComponent<EdgeCollider2D>();

        GradientColorKey[] bigLineColorKeys = new GradientColorKey[bigLineColors.Length];
        GradientColorKey[] killedProjColorKeys = new GradientColorKey[killedProjColors.Length];
        for (int i = 0; i < bigLineColorKeys.Length; i++)
        {
            bigLineColorKeys[i] = new GradientColorKey(bigLineColors[i], defaultLineGradient.colorKeys[i].time);
            killedProjColorKeys[i] = new GradientColorKey(killedProjColors[i], defaultLineGradient.colorKeys[i].time);
        }
              
        bigLineGradient.colorKeys = bigLineColorKeys;
        killedProjLineGradient.colorKeys = killedProjColorKeys;

        foreach(GradientColorKey colorkey in defaultLineGradient.colorKeys)
        {
            print(colorkey.color);
        }

        mirrorLine.SetActive(false);
        gun.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(lineDurationBonusActive)
        {
            counterOfLineDurBonus += Time.deltaTime;
            if (counterOfLineDurBonus > lineDurBonusDuration)
            {
                lineDurationBonusActive = false;
                counterOfLineDurBonus = 0;
                timeToLive /= 2;
            }
        }

        if (lineBiggerBonusActive)
        {
            counterOfLineBiggerBonus += Time.deltaTime;
            if (counterOfLineBiggerBonus > lineBiggerBonusDuration)
            {
                lineBiggerBonusActive = false;
                counterOfLineBiggerBonus = 0;
                curveWidthMultiplayer /= 2;
                edgeCollider.edgeRadius -= 0.5f;
                currentLineGradient = defaultLineGradient;
            }
        }

        if (mirrorLineBonusActive)
        {
            counterMirrorLineBonus += Time.deltaTime;
            if (counterMirrorLineBonus > mirrorLineBonusDuration)
            {
                print("Mirror end");
                mirrorLineBonusActive = false;
                counterMirrorLineBonus = 0;
                mirrorLine.SetActive(false);
            }
        }

        DrawLine();
    }


    private void DrawLine()
    {

        if (Input.GetMouseButton(0) && counterOfLive < timeToLive)
        {
            //linerend.SetPosition(1, GetMouseVector3());
            counter += Time.deltaTime;
            counterOfLive += Time.deltaTime;
            if (counter >= 0.1f) //Time before creating new vertice in Line
            {
                linerend.positionCount = ind + 1;
                linerend.SetPosition(ind, GetMouseVector3());
                ind += 1;
                counter = 0f;
            }

            Vector3[] newPos = new Vector3[linerend.positionCount];
            linerend.GetPositions(newPos);
            edgeCollider.points = ConvertArray(newPos);

            if (killedProj)
            {
                linerend.colorGradient = killedProjLineGradient;
            }
        }

        else if (Input.GetMouseButtonUp(0) || counterOfLive >= timeToLive)
        {
            killedProj = false;
            oneSwipeKillsCounter = 0;
            linerend.widthMultiplier = curveWidthMultiplayer;
            linerend.colorGradient = currentLineGradient;

            linerend.positionCount = 0;
            Vector2[] empty = new Vector2[2] {Vector2.zero, Vector2.zero};
            edgeCollider.points = empty;
            ind = 0;
            counterOfLive = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ProjectileBehaviour otherProjectileObject = other.GetComponent<ProjectileBehaviour>();
        if (otherProjectileObject != null)
        {
            otherProjectileObject.marked = true;
            killedProj = true;
            GunAnimation(other);
            oneSwipeKillsCounter += 1;
            linerend.widthMultiplier *= oneSwipeKillsCounter;
            linerend.widthMultiplier = Mathf.Clamp(linerend.widthMultiplier, curveWidthMultiplayer, curveWidthMultiplayer*3);
            GameController.instance.SendMessage("IncreaseKills", 1);
            //Destroy(other.gameObject);
        }
    }

    void GunAnimation(Collider2D proj)
    {
        gun.transform.position = linerend.GetPosition(0);
        Vector3 shootDir = linerend.GetPosition(1) - linerend.GetPosition(0);
        float angleRad = Mathf.Atan2(linerend.GetPosition(1).y - gun.transform.position.y, linerend.GetPosition(1).x - gun.transform.position.x);
        float angleDeg = (180 / Mathf.PI) * angleRad;
        gun.transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }

    public void ChangeLineDuration(float duration)
    {
        lineDurBonusDuration = duration;
        if (lineDurationBonusActive)
            counterOfLineDurBonus = 0;
            
        else
        {
            timeToLive *= 2;
            lineDurationBonusActive = true;
        }
    }

    public void MakeLineBigger(float duration)
    {
        lineBiggerBonusDuration = duration;
        if (lineBiggerBonusActive)
            counterOfLineBiggerBonus = 0;

        else
        {
            curveWidthMultiplayer *= 2;
            edgeCollider.edgeRadius += 0.5f;
            lineBiggerBonusActive = true;
            currentLineGradient = bigLineGradient;
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
            print("Mirror start");
            mirrorLine.SetActive(true);
            mirrorLineBonusActive = true;
        }
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
