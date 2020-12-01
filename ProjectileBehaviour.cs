using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    private float spawnY = 6;
    private Rigidbody2D rb;
    private CircleCollider2D collider;
    private Animation animation;

    public float gravScale = 0.5f;
    public int health = 1;
    public int damage = 5;

    public GameObject shortExplosion;
    public GameObject mediumExplosion;

    // Start is called before the first frame update
    void Start()
    {
        float randomisedScale = UnityEngine.Random.Range(0.75f, 1.25f);
        float randomisedX = UnityEngine.Random.Range(-1.5f, 1.5f);

        transform.localScale = new Vector3(randomisedScale, randomisedScale, randomisedScale);
        transform.position = new Vector3(randomisedX, spawnY, 0);

        collider = GetComponent<CircleCollider2D>();
        animation = GetComponent<Animation>();

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(-2f, 0));
        rb.gravityScale = gravScale * gameObject.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 50f * Time.deltaTime);
    }

    public bool GetDamage(int amount)
    {
        health -= amount;
        if(health > 0)
        {
            animation.PlayQueued("ProjectileGotDamageAnim");
            animation.PlayQueued("ProjectileReturnColorAnim");
        }
        if (health <= 0)
        {
            collider.enabled = false;
            animation.Play("ProjectileKilledAnim");
        }
        return health <= 0;
    } //Mouse swipe deals 1 damage to proj, so return bool to determine if proj was killed. Neeeded for MouseMovement and MirrorLine scripts


    public void ReturnOriginalColor()
    {
        GetComponent<SpriteRenderer>().color = GameController.instance.GetComponent<ProjectilesGenerator>().GetDefendedAsteroidColor();
    }

    public void DestroyProjectile()
    {
        GameObject explosion = transform.localScale.x < 0.65 ? shortExplosion : mediumExplosion;
        explosion = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosion, 0.5f);
        Destroy(gameObject);
    }
}
