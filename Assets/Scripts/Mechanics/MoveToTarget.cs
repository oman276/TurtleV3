using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    public Transform target;
    public float speed;

    public bool beginsMoving = false;
    public float startDelay = 0;

    bool active;

    private void Start()
    {
        /*
        if (beginsMoving) {
            Invoke("Activate", startDelay);
        }
        */
    }

    public void StartWithDelay() {
        Invoke("Activate", startDelay);
    }

    public void Activate() {
        active = true;
    }

    private void Update()
    {
        if (active) {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

}
