using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    public Transform target;
    public float addedSlingPower = 1.1f;

    public GameObject directionalArrow;

    private void Start()
    {
        if (directionalArrow) {
            Vector3 direction = target.position - transform.position;  // Get direction to target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  // Calculate rotation angle
            directionalArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));  // Set rotation, adjust by -90 if needed
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Player") {
            StartCoroutine(Launch());
        }
    }

    IEnumerator Launch(){
        GameManager.G.audio.Play("stretch");
        GameManager.G.player.playerObject.transform.position = this.transform.position;
        GameManager.G.player.StopVelocity();
        GameManager.G.player.SwapState(PlayerState.MovementLocked);
        GameManager.G.player.health.ResetHealth();

        yield return new WaitForSeconds(1.2f);
        GameManager.G.audio.Play("whoosh");
        GameManager.G.player.SwapState(PlayerState.Active);
        Vector2 direction = (target.position - this.transform.position).normalized;
        //Debug.Log(direction + " " + GameManager.G.player.transform.position);
        GameManager.G.player.AddForce(direction * GameManager.G.player.movement.speed * addedSlingPower);
    }
}
