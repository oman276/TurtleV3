using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class LevelManager : MonoBehaviour
{
    public int numberOfEnemies = 1;
    public string levelName = "test 1";
    public Transform playerSpawn;
    public Transform cameraSpawn;

    public Activatable[] itemsToActivate;

    public static GameObject[] rivers;
    List<AreaEffector2D> effectors;

    public Camera levelCam;
    public bool lavaRising = false;

    int bridgeCount = 0;

    private void Start()
    {
        rivers = GameObject.FindGameObjectsWithTag("Water");
        effectors = new List<AreaEffector2D>();
        foreach (GameObject river in rivers)
        {
            effectors.Add(river.GetComponent<AreaEffector2D>());
        }
        GameManager.G.StartLevel(this);

        CinemachineVirtualCamera[] cams = FindObjectsOfType<CinemachineVirtualCamera>(true);
        foreach (CinemachineVirtualCamera c in cams) c.Follow = GameManager.G.player.playerObject.transform.Find("PlayerSprite").transform.Find("CameraFollow").transform;
    }

    public void EnemyDefeated() {
        numberOfEnemies--;
        if (numberOfEnemies == 0) {
            GameManager.G.BeatLevel();
        }
    }

    public void ActivateItems() {
        foreach (Activatable i in itemsToActivate) {
            if(!i.activated) i.Activate();
        };
    }

    public string startText() {
        return (lavaRising ? "Rising Lava Incoming!" : "Go!");
    }

    public void BridgeExit() {
        bridgeCount--;
        if (bridgeCount == 0) BridgeOnOff(false);
    }

    public void BridgeEnter() {
        if (bridgeCount == 0) BridgeOnOff(true);
        bridgeCount++;
    }

    void BridgeOnOff(bool activate) {
        LayerMask mask;
        if (activate)
        {
            mask = LayerMask.GetMask("Nothing");
        }
        else {
            mask = LayerMask.GetMask("Player");
        }

        foreach (AreaEffector2D e in effectors)
        {
            e.colliderMask = mask;
        }
    }
}
