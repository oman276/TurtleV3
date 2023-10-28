using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingBlock : MonoBehaviour
{
    public GameObject spriteObj;
    public GameObject pm;
    bool playerOn = false;
    public bool playerInTrigger = false;

    //State 1: visible, active
    //State 2: pressed, shaking
    //State 3: invisible, recovering
    int state = 1;

    public float shakeIntensity = 0.1f;
    public float shakeDuration = 2f;
    public float shakeIncrease = 0.1f;
    public float regenTime = 5f;

    float timer;

    Vector3 originalPosition;
    private void Start()
    {
        originalPosition = spriteObj.transform.localPosition;
    }

    private void Update()
    {
        if (state == 2)
        {
            //Shake Loop
            if (Time.time - timer <= shakeDuration)
            {
                spriteObj.transform.localPosition = originalPosition +
                    (Random.insideUnitSphere * shakeIntensity *
                    (shakeDuration - (shakeDuration - (Time.time - timer))));
            }
            //End Loop
            else {
                state = 3;
                timer = Time.time;
                Color temp = spriteObj.GetComponentInChildren<SpriteRenderer>().color;
                temp.a = 0;
                spriteObj.GetComponentInChildren<SpriteRenderer>().color = temp;
                spriteObj.transform.localPosition = originalPosition;

                // if (playerOn) {
                //     pm.removeBridge();
                // }
                playerOn = false;
            }
        }
        else if (state == 3) {
            if (Time.time - timer >= regenTime)
            {
                state = 1;
                Color temp = spriteObj.GetComponentInChildren<SpriteRenderer>().color;
                temp.a = 1;
                spriteObj.GetComponentInChildren<SpriteRenderer>().color = temp;

                if (playerInTrigger) {
                    // pm.addBridge();
                    playerOn = true;

                    if (state == 1)
                    {
                        state = 2;
                        timer = Time.time;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("collide");
            playerInTrigger = true;
            if (state == 1 || state == 2)
            {
                // pm.addBridge();
                playerOn = true;
                
                if (state == 1) {
                    state = 2;
                    timer = Time.time;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("collide");
            playerInTrigger = false;

            if (playerOn)
            {
                // pm.removeBridge();
            }
            playerOn = false;
        }
    }
}
