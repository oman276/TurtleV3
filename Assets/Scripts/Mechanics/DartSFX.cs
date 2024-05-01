using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartSFX : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") GameManager.G.audio.Play("dart_impact");
    }
}
