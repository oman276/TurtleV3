using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HealthFadeState { 
    Off,
    Active,
    WaitingToRecharge,
    Recharging,
    FadeOut
}

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
    public HealthFadeState fadeState = HealthFadeState.Off;
    public float healthDelay = 2.5f;

    public int bridgeCount = 0;

    private void Start()
    {
        health = timeToDeath;
        healthSlider.maxValue = timeToDeath;
        healthSlider.minValue = 0.05f;
        healthSlider.value = health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Lava") {

            fadeState = HealthFadeState.Active;

            

            ++lavaCount;
        }

        if (collision.gameObject.tag == "CrumbleBlock") {
            fadeState = HealthFadeState.WaitingToRecharge;
            StartCoroutine("WaitToRefill");
            bridgeCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Lava")
        {
            --lavaCount;
            if (lavaCount == 0)
            {
                fadeState = HealthFadeState.WaitingToRecharge;
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
        if (lavaCount > 0 && GameManager.G.player.isActive() && bridgeCount <= 0)
        {
            Debug.Log("health is decreasing");
            background.color = new Color(background.color.r, background.color.g, background.color.b, 1);
            fill.color = new Color(fill.color.r, fill.color.g, fill.color.b, 1);
            fadeState = HealthFadeState.Active;
            health -= Time.deltaTime * healthDecayMultiplier;           
            if (health <= 0)
            {
                GameManager.G.player.SwapState(PlayerState.Dead);
            }
        }
        else if (GameManager.G.player.isActive())
        {
            if (fadeState == HealthFadeState.Recharging)
            {
                health += Time.deltaTime * reviveMultiplier;
                if (health >= timeToDeath)
                {
                    fadeState = HealthFadeState.FadeOut;
                    health = timeToDeath;
                    GameManager.G.objectFade.FadeOut(1.5f, background);
                    GameManager.G.objectFade.FadeOut(1.5f, fill);
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
            if (fadeState != HealthFadeState.WaitingToRecharge)
            {
                break;
            }
            yield return null;
        }
        if (fadeState == HealthFadeState.WaitingToRecharge)
        {
            fadeState = HealthFadeState.Recharging;
        }
    }

    public void ResetHealth() {
        health = timeToDeath;
    }
}
