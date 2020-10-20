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
    private float counterOfLive = 0f;
    private int ind = 0;
    private int oneSwipeKillsCounter = 0;
    private bool killedProj = false;


    public float timeToLive = 1f;
    public GameObject gun;

    // Start is called before the first frame update
    void Start()
    {
        linerend = GetComponent<LineRenderer>();
        linerend.startColor = Color.yellow; linerend.endColor = Color.gray;

        edgeCollider = GetComponent<EdgeCollider2D>();
        gun.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        DrawLine();
    }


    private void DrawLine()
    {

        if (Input.GetMouseButton(0) && counterOfLive < timeToLive)
        {
            //linerend.SetPosition(1, GetMouseVector3());
            counter += Time.deltaTime;
            counterOfLive += Time.deltaTime;
            if (counter >= 0.1f)
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
                linerend.startColor = Color.blue; linerend.endColor = Color.green;
            }
        }

        else if (Input.GetMouseButtonUp(0) || counterOfLive >= timeToLive)
        {
            if(oneSwipeKillsCounter != 0)
            {
                print(oneSwipeKillsCounter);
            }
            killedProj = false;
            oneSwipeKillsCounter = 0;
            linerend.startColor = Color.yellow; linerend.endColor = Color.gray;

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
            StartCoroutine(GunAnimation(other));
            oneSwipeKillsCounter += 1;
            GameController.instance.SendMessage("IncreaseKills", 1);
            //Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.GetComponent<ProjectileBehaviour>())
        {
            //gun.SetActive(false);
        }
    }

    public int CountAmountOfProjs(Collider2D obj)
    {
        /*
        Collider2D[] colls = Physics2D.OverlapPointAll((Vector2)obj.transform.position);
        int lenOfColls = colls.Length;
        for (int i = 0; i < lenOfColls; i++)
        {
            colls[i].GetComponent<ProjectileBehaviour>().marked = true;
            colls[i].name = i.ToString();
        } */
        //GameController.instance.SendMessage("IncreaseKills", lenOfColls);
        return 1;
    }

    IEnumerator GunAnimation(Collider2D proj)
    {
        gun.SetActive(true);
        gun.transform.position = linerend.GetPosition(0);
        float angleRad = Mathf.Atan2(proj.transform.position.y - gun.transform.position.y, proj.transform.position.x - gun.transform.position.x);
        float angleDeg = (180 / Mathf.PI) * angleRad;
        gun.transform.rotation = Quaternion.Euler(0, 0, angleDeg);
        yield return new WaitForSeconds(0.1f);
        gun.SetActive(false);
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
