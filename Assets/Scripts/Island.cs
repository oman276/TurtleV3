using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Island : MonoBehaviour
{
    public GameObject[] rivers;

    // Start is called before the first frame update
    void Start()
    {
        rivers = GameObject.FindGameObjectsWithTag("Water");

        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {

            LayerMask mask = LayerMask.GetMask("Nothing");

            foreach (GameObject river in rivers) {
                river.GetComponent<AreaEffector2D>().colliderMask = mask;
            }
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {

            LayerMask mask = LayerMask.GetMask("Nothing");

            foreach (GameObject river in rivers) {
                river.GetComponent<AreaEffector2D>().colliderMask = mask;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
