using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialPopup popup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            popup.Activate();
            Destroy(this.gameObject);
        }
    }
}
