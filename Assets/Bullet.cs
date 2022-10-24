using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public string enemyTag = "Enemy";

    public float damage = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == enemyTag)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
