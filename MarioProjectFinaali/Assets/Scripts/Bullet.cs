using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public void Die()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; 
        GetComponent<Rigidbody2D>().gravityScale = 1;

        Destroy(GetComponent<BoxCollider2D>());
        Destroy(gameObject, 3);
    }
}
