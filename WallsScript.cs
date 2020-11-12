using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DefineResolutionPosition()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<ProjectileBehaviour>())
        {
            Rigidbody2D projRb = other.GetComponent<Rigidbody2D>();
            projRb.velocity = new Vector2(-1.2f*projRb.velocity.x, projRb.velocity.y);
        }
    }
}
