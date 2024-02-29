using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : Activatable
{
    public Transform target;
    public float speed;

    bool activatedInternal = false;
    public float startDelay = 0;

    public override void Activate()
    {
        base.Activate();
        StartCoroutine(Delay());
    }

    IEnumerator Delay() {
        yield return new WaitForSeconds(startDelay);
        activatedInternal = true;
    }

    private void Update()
    {
        if (activatedInternal) {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

}
