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
    public Transform spawnPoint;
    GameObject player;

    bool alive = true;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("FireProjectile", projectileDelay, projectileDelay);
        player = GameManager.G.player.playerObject;
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
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation,
            Quaternion.Euler(new Vector3(0, 0, angle)), step);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && alive) {
            alive = false;
            GameObject exp = Instantiate(explosion, this.transform.position, this.transform.rotation);
            exp.transform.parent = null;
            GameManager.G.currentLevel.EnemyDefeated();
            GameManager.G.audio.Play("enemy_destroy");
            Destroy(this.gameObject);
        }
    }

    void FireProjectile() {
        if (Vector3.Distance(player.transform.position, this.transform.position) <= 25f &&
            GameManager.G.player.state != PlayerState.Dead && GameManager.G.state == GameState.Playing)
        {
            GameObject currentProj = Instantiate(bullet, spawnPoint.position, Quaternion.identity);
            //Vector3 direction = player.transform.position - this.transform.position;
            Vector3 direction = -this.transform.up;
            currentProj.GetComponent<Rigidbody2D>().velocity = projectileForce * direction.normalized;
            GameManager.G.audio.Play("enemy_release");
        }
    }
}
