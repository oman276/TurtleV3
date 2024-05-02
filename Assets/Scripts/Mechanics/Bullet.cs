using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float force = 1f;

    public bool immuneToWall = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            NewMovement nm = collision.gameObject.GetComponent<NewMovement>();
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();          
            rb.velocity = Vector2.zero;
            Vector2 thisPos = this.transform.position;
            Vector2 newVector = (this.GetComponent<Collider2D>().ClosestPoint(collision.gameObject.transform.position)
                - thisPos).normalized;
            GameManager.G.audio.Play("hit");
            rb.AddForce(newVector * (nm.speed * force + 1));
        }
        if (collision.tag == "World Boundary" && immuneToWall) { /* do nothing */ }
        else if (collision.tag != "Enemy" && collision.tag != "Lava" && collision.tag != "Bumper") {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "World Boundary") immuneToWall = false;
    }
}
