using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBallSpawn : Activatable
{
    public float minInterval = 4f;
    public float maxInterval = 4f;
    public GameObject ball;
    public Transform spawnpoint;

    public float distanceFromPlayer = 15f;

    float timer = 0f;
    float timeToCall;

    public override void Activate()
    {
        base.Activate();
        timeToCall = Random.Range(minInterval, maxInterval);
    }

    private void Update()
    {
        if (activated)
        {
            timer += Time.deltaTime;
            if (timer >= timeToCall)
            {
                timer = 0f;
                timeToCall = Random.Range(minInterval, maxInterval);
                if (Vector2.Distance(this.gameObject.transform.position, GameManager.G.player.transform.position)
                    <= distanceFromPlayer)
                {
                    SpawnBall();
                }
            }
        }
    }

    void SpawnBall() {
        GameObject instance = Instantiate(ball, spawnpoint.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 180));
        RigidbodyMove rm = instance.GetComponent<RigidbodyMove>();
        rm.SetDirection(spawnpoint.position - transform.position);
    }
}
