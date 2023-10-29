using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMainMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public Transform lowerLeft;
    public Transform upperRight;
    public Transform zoneTarget;

    GameObject player;

    public bool transitioning = false;

    void Start() {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (!transitioning)
        {
            Vector3 temp = Vector3.Lerp(this.transform.position, new Vector3(player.transform.position.x,
                    player.transform.position.y, -10f), speed * Time.deltaTime);

            //Clamp X
            if (temp.x < lowerLeft.position.x) temp.x = lowerLeft.position.x;
            else if (temp.x > upperRight.position.x) temp.x = upperRight.position.x;

            //Clamp Y
            if (temp.y < lowerLeft.position.y && !transitioning) temp.y = lowerLeft.position.y;
            else if (temp.y > upperRight.position.y && !transitioning) temp.y = upperRight.position.y;

            this.transform.position = temp;
        }
        else{
            /*
            Vector3 temp = Vector3.Lerp(this.transform.position, new Vector3(zoneTarget.transform.position.x,
                    zoneTarget.transform.position.y, -10f), speed * Time.deltaTime);
            this.transform.position = temp;
            */
            //Left Is Closer
            if(Vector3.Distance(this.transform.position, new Vector3(lowerLeft.transform.position.x,
                lowerLeft.transform.position.y, -10f)) <= Vector3.Distance(this.transform.position, 
                new Vector3(upperRight.transform.position.x, upperRight.transform.position.y, -10f)) ){
                    //Left Is Closer
                Vector3 temp = Vector3.Lerp(this.transform.position, new Vector3(zoneTarget.transform.position.x,
                    zoneTarget.transform.position.y, -10f), speed * Time.deltaTime);
                this.transform.position = temp;
                if(this.transform.position.y >= lowerLeft.transform.position.y){
                    transitioning = false;
                }
            }
            else{
                Vector3 temp = Vector3.Lerp(this.transform.position, new Vector3(zoneTarget.transform.position.x,
                    zoneTarget.transform.position.y, -10f), speed * Time.deltaTime);
                this.transform.position = temp;
                if(this.transform.position.y <= upperRight.transform.position.y){
                    transitioning = false;
                }

            }

        }
    }

    public void swapZones(Transform _lowerLeft, Transform _upperRight, Transform _zoneTarget){
        transitioning = true;
        lowerLeft = _lowerLeft;
        upperRight = _upperRight;
        zoneTarget = _zoneTarget;
    }
}
