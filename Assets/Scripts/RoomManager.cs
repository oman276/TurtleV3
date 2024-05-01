using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject virtualCam;

    public float whereIsTurtleCamera = 0f;
    public bool containsWater = false;
    public bool containsLava = false;


    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.name);
        //if (other.gameObject.name == "Lava Base" || other.gameObject.name == "Rising Lava") Debug.Log("joink");

        if ((other.gameObject.name == "Lava Base" || other.gameObject.name == "Rising Lava") 
            && containsLava == false && !GameManager.G.audio.lavaPlaying) {
            containsLava = true;
            GameManager.G.audio.lavaPlaying = true;
            GameManager.G.audio.Play("lava_sizzle");
        }

        if(other.CompareTag("Player") && !other.isTrigger) {

            GameObject cameraOfPlayer = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerSprite").gameObject.transform.Find("CameraFollow").gameObject;
            cameraOfPlayer.transform.localPosition = new Vector3(whereIsTurtleCamera, 0f, 0f);

            virtualCam.SetActive(true);

            if (containsWater && !GameManager.G.audio.waterPlaying)
            {
                GameManager.G.audio.waterPlaying = true;
                GameManager.G.audio.Play("running_water");
            }
            else if (!containsWater)
            {
                GameManager.G.audio.waterPlaying = false;
                GameManager.G.audio.Stop("running_water");
            }

            if (containsLava && !GameManager.G.audio.lavaPlaying)
            {
                GameManager.G.audio.lavaPlaying = true;
                GameManager.G.audio.Play("lava_sizzle");
            }
            else if (!containsLava)
            {
                GameManager.G.audio.lavaPlaying = false;
                GameManager.G.audio.Stop("lava_sizzle");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {

        if(other.CompareTag("Player") && !other.isTrigger) {

            GameObject cameraOfPlayer = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerSprite").gameObject.transform.Find("CameraFollow").gameObject;

            virtualCam.SetActive(false);
        }
    }
}
