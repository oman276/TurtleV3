using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int lavaCount = 0;
    public float timeToDeath = 2.5f;
    float healthDecayMultiplier = 3.6f;
    float health;
    public float reviveMultiplier = 0.4f;
    public Slider healthSlider;
    public Image fill;
    public Image background;
    ObjectFade objectFade;
    //0 - off
    //1 - active
    //2 - waiting to recharge
    //3 - recharging
    //4 - fade out
    public int fadeState = 0;
    public float healthDelay = 2.5f;
    public bool canMove = true;

    public int bridgeCount = 0;

    Rigidbody2D rb;
    SpriteRenderer playerSprite;

    private void Start()
    {
        objectFade = FindObjectOfType<ObjectFade>();

        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();

        health = timeToDeath;
        healthSlider.maxValue = timeToDeath;
        healthSlider.minValue = 0;
        healthSlider.value = health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "OOB") {

            fadeState = 1;

            background.color = new Color(background.color.r, background.color.g, background.color.b, 1);
            fill.color = new Color(fill.color.r, fill.color.g, fill.color.b, 1);

            ++lavaCount;
        }

        if (collision.gameObject.tag == "CrumbleBlock") {
            Debug.Log("Crumble");
            fadeState = 2;
            StartCoroutine("WaitToRefill");
            bridgeCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "OOB")
        {
            --lavaCount;
            if (lavaCount == 0)
            {
                fadeState = 2;
                StartCoroutine("WaitToRefill");
            }
        }

        if (collision.gameObject.tag == "CrumbleBlock") {
            --bridgeCount;
        }

    }

    private void FixedUpdate()
    {
        //Reduce or increase health
        if ((lavaCount > 0 && canMove) && bridgeCount <= 0)
        {
            health -= Time.deltaTime * healthDecayMultiplier;           
            if (health <= 0)
            {
                this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                rb.velocity = Vector2.zero;
                canMove = false;
                playerSprite.enabled = false;
                Invoke("Respawn", 3f);
            }
        }
        else if (canMove)
        {
            if (fadeState == 3)
            {
                health += Time.deltaTime * reviveMultiplier;
                if (health >= timeToDeath)
                {
                    fadeState = 4;
                    health = timeToDeath;
                    objectFade.FadeOut(1.5f, background);
                    objectFade.FadeOut(1.5f, fill);
                }
            }
        }
        healthSlider.value = health;
    }

    IEnumerator WaitToRefill()
    {
        float startTime = Time.time;
        while (Time.time - startTime < healthDelay)
        {
            if (fadeState != 2)
            {
                break;
            }
            yield return null;
        }
        if (fadeState == 2)
        {
            fadeState = 3;
        }
    }

    void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetHealth() {
        health = timeToDeath;
    }
}
