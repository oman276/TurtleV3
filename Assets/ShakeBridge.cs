using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeBridge : MonoBehaviour
{

    public float speed = 30.0f; //how fast it shakes
    public float amount = 0.7f; //how much it shakes

    public float crumble_timer = 0.0f;

    public float come_back_timer = 0.0f;
    public float until_crumble = 3.0f;
    bool start_timer = false;

    bool start_respawn = false;

    public GameObject sprite;

    bool isCurrentlyColliding = true;

    void Start()
    {
        sprite = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            start_timer = true;
            isCurrentlyColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            isCurrentlyColliding = false;
        }
    }

    // Update is called once per frame

    private IEnumerator Fall()
    {
        // Play the animation for getting suck in
        sprite.GetComponent<Animator>().SetInteger("bridgeState", 1);

        yield return new WaitForSeconds(0.25f);

    }

    void Update()
    {

        if (start_respawn) {
        
            if (crumble_timer > until_crumble) { //comes back

                sprite.GetComponent<Animator>().SetInteger("bridgeState", 0);
                
                start_respawn = false;

                sprite.transform.localPosition = new Vector3(0,0,0);

                crumble_timer = 0f;
                amount = 0.7f;

                if(isCurrentlyColliding) {
                    start_timer = true;
                }
                GetComponent<BoxCollider2D>().enabled = true;
                
            } else {
                crumble_timer += Time.deltaTime;
            }

        } else if(start_timer) {

            Vector3 shaker = new Vector3(Mathf.Sin(Time.time * speed) * amount, sprite.transform.localPosition.y, sprite.transform.localPosition.z);
            sprite.transform.localPosition = shaker;

            if (crumble_timer > until_crumble) { //crumbled

                StartCoroutine(Fall());
                
                start_timer = false;
                start_respawn = true;

                sprite.transform.localPosition = new Vector3(0,0,0);

                crumble_timer = 0f;
                amount = 0.7f;

                GetComponent<BoxCollider2D>().enabled = false;
                
            } else {
                crumble_timer += Time.deltaTime;
            }

            amount += 0.001f;
            
        }


    }
}
