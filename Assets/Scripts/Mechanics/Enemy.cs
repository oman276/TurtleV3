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
    public Transform[] spawnPoints;
    GameObject player;

    //Health
    int health;
    public int startHealth = 3;
    bool invFrames = false;
    bool alive = true;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("FireProjectile", projectileDelay, projectileDelay);
        player = GameManager.G.player.playerObject;
        health = startHealth;
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
        if (collision.gameObject.tag == "Player" && alive && !invFrames) {
            health--;
            GameManager.G.audio.Play("enemy_hit");
            if (health == 0) {
                GameManager.G.player.ReduceVelocity(0.5f);
                FindObjectOfType<CameraShake>().ShakeCamera(3.5f, 1f);
                Die(); 
            }
            else
            {
                StartCoroutine(Invulnerability());
                FindObjectOfType<CameraShake>().ShakeCamera(2f, 0.25f);
                GameManager.G.player.BounceBack(0.85f);
            }
        }
    }

    IEnumerator Invulnerability() {
        Time.timeScale = 0;
        invFrames = true;
        this.GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0.3f);
        yield return new WaitForSecondsRealtime(0.05f);
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        yield return new WaitForSecondsRealtime(0.05f);
        this.GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0.3f);
        yield return new WaitForSecondsRealtime(0.05f);
        invFrames = false;
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        Time.timeScale = 1;
    }

    void Die()
    {
        alive = false;
        GameObject exp = Instantiate(explosion, this.transform.position, this.transform.rotation);
        exp.transform.parent = null;
        GameManager.G.currentLevel.EnemyDefeated();
        GameManager.G.audio.Play("enemy_destroy");
        Destroy(this.gameObject);
    }

    void FireProjectile() {
        if (Vector3.Distance(player.transform.position, this.transform.position) <= 25f &&
            GameManager.G.player.state != PlayerState.Dead && GameManager.G.state == GameState.Playing)
        {
            foreach(var p in spawnPoints) {
                GameObject currentProj = Instantiate(bullet, p.position, Quaternion.identity);
                Vector3 direction = p.position - this.transform.position;
                currentProj.GetComponent<Rigidbody2D>().velocity = projectileForce * direction.normalized;
                GameManager.G.audio.Play("enemy_release");
            }
        }
    }
}
