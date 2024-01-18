using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    public Transform target;
    public float addedSlingPower = 1.1f;
    AudioManager am;

    private void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Player") {
            StartCoroutine(Launch());
        }
    }

    IEnumerator Launch(){
        am.Play("stretch");
        GameManager.G.player.playerObject.transform.position = this.transform.position;
        GameManager.G.player.StopVelocity();
        GameManager.G.player.SwapState(PlayerState.MovementLocked);
        GameManager.G.player.health.ResetHealth();

        yield return new WaitForSeconds(1.2f);
        am.Play("whoosh");
        GameManager.G.player.SwapState(PlayerState.Active);
        Vector2 direction = (target.position - this.transform.position).normalized;
        GameManager.G.player.AddForce(direction * GameManager.G.player.movement.speed * addedSlingPower);
    }
}
