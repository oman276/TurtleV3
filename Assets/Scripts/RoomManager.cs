using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject virtualCam;

    public float whereIsTurtleCamera = 0f;


    private void OnTriggerEnter2D(Collider2D other) {

        if(other.CompareTag("Player") && !other.isTrigger) {

            GameObject cameraOfPlayer = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerSprite").gameObject.transform.Find("CameraFollow").gameObject;
            cameraOfPlayer.transform.localPosition = new Vector3(whereIsTurtleCamera, 0f, 0f);

            virtualCam.SetActive(true);
        
        }
    }

    private void OnTriggerExit2D(Collider2D other) {

        if(other.CompareTag("Player") && !other.isTrigger) {

            GameObject cameraOfPlayer = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerSprite").gameObject.transform.Find("CameraFollow").gameObject;

            virtualCam.SetActive(false);
        }
    }
}
