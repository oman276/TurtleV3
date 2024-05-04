using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BRIDGE_STATE { 
    UNBROKEN = 0,
    DMG1 = 1,
    DMG2 = 2,
    DMG3 = 3,
    BROKE = 4
};

public class ShakeBridge : MonoBehaviour
{
    // river stuff
    /*
    public GameObject[] rivers;

    public float speed = 30.0f; //how fast it shakes
    public float amount = 0.7f; //how much it shakes

    public float crumble_timer = 0.0f;

    public float come_back_timer = 0.0f;
    public float until_crumble = 3.0f;
    bool start_timer = false;

    bool start_respawn = false;
    */

    public GameObject sprite;
    bool isCurrentlyColliding = true;
    Animator sprAnim;
    BRIDGE_STATE state = BRIDGE_STATE.UNBROKEN;
    public float timeToBreak;
    public float timeToRespawn;
    bool destroying = false;

    void Start()
    {
        sprite = transform.GetChild(0).gameObject;
        //rivers = GameObject.FindGameObjectsWithTag("Water");
        sprAnim = sprite.gameObject.GetComponent<Animator>();
        sprAnim.SetInteger("damage_state", 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            isCurrentlyColliding = true;
            GameManager.G.currentLevel.BridgeEnter();
            if (!destroying) {
                destroying = true;
                StartCoroutine(DestroyCoroutine()); 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            isCurrentlyColliding = false;
            GameManager.G.currentLevel.BridgeExit();
        }
    }

    IEnumerator DestroyCoroutine() {
        sprAnim.SetInteger("damage_state", 1);
        yield return new WaitForSeconds(timeToBreak / 3f);
        sprAnim.SetInteger("damage_state", 2);
        yield return new WaitForSeconds(timeToBreak / 3f);
        sprAnim.SetInteger("damage_state", 3);
        yield return new WaitForSeconds(timeToBreak / 3f);
        FallDown();
    }

    void FallDown() { 
        if(isCurrentlyColliding) GameManager.G.currentLevel.BridgeExit();
        //sprite.gameObject.SetActive(false);
        GetComponent<BoxCollider2D>().enabled = false;
        if (Vector2.Distance(this.transform.position, GameManager.G.player.movement.transform.position) <= 25f)
        {
            GameManager.G.audio.Play("glass_break");
        }
        sprAnim.SetBool("is_active", false);
        Invoke("Reactivate", timeToRespawn);
        //sprAnim.SetInteger("damage_state", 0);
        destroying = false;
    }

    void Reactivate() {
        sprAnim.SetInteger("damage_state", 0);
        sprAnim.SetBool("is_active", true);
        sprite.gameObject.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = true;
        if (isCurrentlyColliding) {
            destroying = true;
        }
    }

    int StateToInt(BRIDGE_STATE b) {
        if (b == BRIDGE_STATE.UNBROKEN) return 0;
        else if (b == BRIDGE_STATE.DMG1) return 1;
        else if (b == BRIDGE_STATE.DMG2) return 2;
        else if (b == BRIDGE_STATE.DMG3) return 3;
        else return 4;
    }

    void Update()
    {

        //TODO REPLACE W UPDATE VERSION IF THIS IS TOO HARD
        /*
        if (start_respawn) {
        
            if (crumble_timer > until_crumble) { //comes back
                
                start_respawn = false;

                sprite.transform.localPosition = new Vector3(0,0,0);

                crumble_timer = 0f;

                if(isCurrentlyColliding) {
                    start_timer = true;
                }
                GetComponent<BoxCollider2D>().enabled = true;
                GameManager.G.currentLevel.BridgeExit();
                
            } else {
                crumble_timer += Time.deltaTime;
            }

        } else if(start_timer) {

            if (crumble_timer > until_crumble) { //crumbled
                
                start_timer = false;
                start_respawn = true;

                sprite.transform.localPosition = new Vector3(0,0,0);

                crumble_timer = 0f;
                amount = 0.7f;

                GetComponent<BoxCollider2D>().enabled = false;
                if (Vector2.Distance(this.transform.position, GameManager.G.player.movement.transform.position) <= 25f) {
                    GameManager.G.audio.Play("glass_break");
                }
                
                GameManager.G.currentLevel.BridgeEnter();

            } else {
                crumble_timer += Time.deltaTime;
            }
        }
        */
    }
}
