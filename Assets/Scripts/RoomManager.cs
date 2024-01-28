using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject virtualCam;

    public int whereIsTurtleCamera = 0;


    private void OnTriggerEnter2D(Collider2D other) {

        if(other.CompareTag("Player") && !other.isTrigger) {

            GameObject cameraOfPlayer = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerSprite").gameObject.transform.Find("CameraFollow").gameObject;
            cameraOfPlayer.transform.position = new Vector3(cameraOfPlayer.transform.position.x + whereIsTurtleCamera, cameraOfPlayer.transform.position.y, cameraOfPlayer.transform.position.z);

            virtualCam.SetActive(true);
        
        }
    }

    private void OnTriggerExit2D(Collider2D other) {

        if(other.CompareTag("Player") && !other.isTrigger) {

            GameObject cameraOfPlayer = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerSprite").gameObject.transform.Find("CameraFollow").gameObject;
            cameraOfPlayer.transform.position = new Vector3(cameraOfPlayer.transform.position.x, cameraOfPlayer.transform.position.y, cameraOfPlayer.transform.position.z);

            virtualCam.SetActive(false);
        }
    }
}
