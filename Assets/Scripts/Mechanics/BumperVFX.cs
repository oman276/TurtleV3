using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperVFX : MonoBehaviour
{
    float startScale;
    Vector3 increasedScale;
    Vector3 normalScale;

    private void Start()
    {
        startScale = transform.localScale.x;
        increasedScale = new Vector3(startScale * 1.2f, startScale * 1.2f, startScale * 1.2f);
        normalScale = new Vector3(startScale, startScale, startScale);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            StartCoroutine(Animate());
        }
    }

    IEnumerator Animate() {
        yield return new WaitForSeconds(0.05f);
        transform.localScale = increasedScale;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = normalScale;
    }
}
