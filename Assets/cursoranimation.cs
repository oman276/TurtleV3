using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursoranimation : MonoBehaviour
{
    public float speed = 5;
    public Vector3 initpos;
    // Start is called before the first frame update

    public Vector3 finalpos;

    public int direction = 1;

    void Start()
    {
        initpos = transform.position;
        finalpos = transform.position += new Vector3(10, 0, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        if(transform.position == finalpos) {
            direction = -1;
            speed = 15;
        }
        if(transform.position == initpos) {
            direction = 1;
            speed = 5;
        }

        if(direction == 1) {
            transform.position = Vector3.MoveTowards(transform.position, finalpos, step);
        } else {
            transform.position = Vector3.MoveTowards(transform.position, initpos, step);
        }
        
    }
}
