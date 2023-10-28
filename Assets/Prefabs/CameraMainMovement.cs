using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMainMovement : MonoBehaviour
{
    public float speed = 0.5f;
    public Transform lowerLeft;
    public Transform upperRight;

    GameObject player;

    void Start() {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        Vector3 temp = Vector3.Lerp(this.transform.position, new Vector3(player.transform.position.x,
                player.transform.position.y, -10f), speed * Time.deltaTime);
        
        //Clamp X
        if (temp.x < lowerLeft.position.x) temp.x = lowerLeft.position.x;
        else if (temp.x > upperRight.position.x) temp.x = upperRight.position.x;

        //Clamp Y
        if (temp.y < lowerLeft.position.y) temp.y = lowerLeft.position.y; 
        else if (temp.y > upperRight.position.y) temp.y = upperRight.position.y;
        
        this.transform.position = temp;
    }
}
