using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject explosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            FindObjectOfType<GameManager>().EnemyDestroyed();
            Destroy(this.gameObject);
        }
    }
}
