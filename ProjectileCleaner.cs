using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCleaner : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<ProjectileBehaviour>())
        {
            Destroy(other.gameObject);
        }
    }
}
