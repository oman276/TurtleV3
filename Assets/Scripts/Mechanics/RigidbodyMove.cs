using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyMove : MonoBehaviour
{
    public float speed = 1f;
    Rigidbody2D rb;

    public void SetDirection(Vector3 newDir) {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = newDir.normalized * speed;
    }
}
