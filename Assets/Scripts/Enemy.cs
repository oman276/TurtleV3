using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject explosion;
    public GameObject bullet;
    public float projectileForce = 3f;
    public float projectileDelay = 1.7f;
    Rigidbody2D rb;
    public float rotateSpeed = 50f;
    ObjectFade objectFade;
    GameManager gameManager;
    public Transform spawnPoint;
    GameObject player;
    Rigidbody2D rb_player;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        objectFade = FindObjectOfType<ObjectFade>();

        InvokeRepeating("FireProjectile", projectileDelay, projectileDelay);

        player = GameObject.Find("Player");
        rb_player = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 targetPos = (Vector2)player.transform.position + (rb.velocity.normalized * 25);
        targetPos.z = 0;
        Vector3 spritePos = this.transform.position;
        targetPos.x = targetPos.x - spritePos.x;
        targetPos.y = targetPos.y - spritePos.y;

        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        angle = angle + 90f;
        float step = rotateSpeed * Time.deltaTime;
        //this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90f));
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation,
            Quaternion.Euler(new Vector3(0, 0, angle)), step);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            FindObjectOfType<GameManager>().EnemyDestroyed();
            Destroy(this.gameObject);
        }
    }

    void FireProjectile() {
        GameObject currentProj = Instantiate(bullet, spawnPoint.position, Quaternion.identity);
        //Vector3 direction = player.transform.position - this.transform.position;
        Vector3 direction = -this.transform.up;
        currentProj.GetComponent<Rigidbody2D>().velocity = projectileForce * direction.normalized;
    }
}
