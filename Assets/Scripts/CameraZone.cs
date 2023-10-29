using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraZone : MonoBehaviour
{
    public Transform lowerLeft;
    public Transform upperRight;
    public Transform zoneTarget;
    public float orthogSize = 11f;

    GameObject tutorialText;
    NewMovement nm;
    CameraMainMovement cmm;

    private void Start()
    {
        nm = FindObjectOfType<NewMovement>();
        cmm = FindObjectOfType<CameraMainMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            nm.startOrthoSize = orthogSize;
            cmm.lowerLeft = lowerLeft;
            cmm.upperRight = upperRight;
            cmm.zoneTarget = zoneTarget;
            cmm.transitioning = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

        }
    }

}
