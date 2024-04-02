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

    public Camera levelCam;
    public bool lavaRising = false;

    private void Start()
    {
        rivers = GameObject.FindGameObjectsWithTag("Water");
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
}
