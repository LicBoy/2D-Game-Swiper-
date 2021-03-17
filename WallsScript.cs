using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<ProjectileBehaviour>())
        {
            Rigidbody2D projRb = other.GetComponent<Rigidbody2D>();
            projRb.velocity = new Vector2(-1.2f*projRb.velocity.x, projRb.velocity.y);
        }
    }
}
