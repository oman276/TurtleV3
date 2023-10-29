using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    public Transform target;

    GameObject player;
    Rigidbody2D rb;
    NewMovement nm;
    PlayerHealth ph;
    public float addedSlingPower = 1.1f;
    AudioManager am;

    private void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Player") {
            player = collision.gameObject;
            rb = player.GetComponent<Rigidbody2D>();
            nm = player.GetComponent<NewMovement>();
            ph = player.GetComponent<PlayerHealth>();
            StartCoroutine(Launch());
        }
    }

    IEnumerator Launch(){
        am.Play("stretch");
        player.transform.position = this.transform.position;
        rb.velocity = Vector2.zero;
        ph.canMove = false;

        yield return new WaitForSeconds(1.2f);
        am.Play("whoosh");
        ph.canMove = true;
        Vector2 direction = (target.position - this.transform.position).normalized;
        rb.AddForce(direction * nm.speed * addedSlingPower);
    }
}
