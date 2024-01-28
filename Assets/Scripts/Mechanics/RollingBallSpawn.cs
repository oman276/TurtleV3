using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBallSpawn : MonoBehaviour
{
    public float intervalToWait = 4f;
    public float initDelay = 4f;
    public GameObject ball;
    public Transform spawnpoint;

    private void Start()
    {
        InvokeRepeating("SpawnBall", initDelay, intervalToWait);
    }
    
    void SpawnBall() {
        GameObject instance = Instantiate(ball, spawnpoint.position, Quaternion.identity);
        RigidbodyMove rm = instance.GetComponent<RigidbodyMove>();
        rm.SetDirection(spawnpoint.position - transform.position);
    }
}
