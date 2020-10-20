using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    private float spawnY = 6;
    private Rigidbody2D rb;
    private CircleCollider2D collider;
    public bool marked = false;

    public float gravScale = 0.5f;
    public float dieDelay = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        float randomisedScale = UnityEngine.Random.Range(0.5f, 1.2f);
        float randomisedX = UnityEngine.Random.Range(-2.5f, 2.5f);

        transform.localScale = new Vector3(randomisedScale, randomisedScale, randomisedScale);
        transform.position = new Vector3(randomisedX, spawnY, 0);

        collider = GetComponent<CircleCollider2D>();

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(-2f, 0));
        rb.gravityScale = gravScale * gameObject.transform.localScale.x;

    }

    // Update is called once per frame
    void Update()
    {
        if (marked)
        {
            FastBlink();
            dieDelay -= Time.deltaTime;
            if(dieDelay <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void FastBlink()
    {
        float val = Mathf.PingPong(Time.time * 20, 1);
        GetComponent<SpriteRenderer>().color = Color.Lerp(Color.blue, Color.yellow, val);
    }
}
