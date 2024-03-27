using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight : MonoBehaviour
{
    public float speed = 5.0f;
    GameObject player;

    private void Start()
    {
        player = GameManager.G.player.playerObject;
    }

    void Update()
    {
        Vector3 temp = Vector3.Lerp(this.transform.position, new Vector3(player.transform.position.x,
                         player.transform.position.y, -4f), speed * Time.deltaTime);
        this.transform.position = temp;
    }
}
