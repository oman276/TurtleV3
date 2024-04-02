using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyHorizontal : MonoBehaviour
{

    public Rigidbody2D rb;
    public float moveSpeed = 0.5f;

    public Transform leftBound;
    public Transform rightBound;

    public bool movingRight;

    // Start is called before the first frame update
    void Start()
    {
        if(rb == null) {
            rb = this.GetComponent<Rigidbody2D>();
        }

    }

    private void Update()
    {
        float time = Mathf.PingPong(Time.time * moveSpeed, 1);
        if (movingRight) time = 1 - time;
        Vector3 newPosition = Vector3.Lerp(leftBound.position, rightBound.position, time);
        rb.MovePosition(newPosition);
    }

}
