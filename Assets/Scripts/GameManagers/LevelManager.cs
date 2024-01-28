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

    public MoveToTarget[] itemsToActivate;

    public static GameObject[] rivers;

    private void Start()
    {
        rivers = GameObject.FindGameObjectsWithTag("Water");
        GameManager.G.StartLevel(this);

        CinemachineVirtualCamera[] cams = FindObjectsOfType<CinemachineVirtualCamera>(true);
        foreach (CinemachineVirtualCamera c in cams) c.Follow = GameManager.G.player.playerObject.transform;
    }

    public void EnemyDefeated() {
        numberOfEnemies--;
        if (numberOfEnemies == 0) {
            GameManager.G.BeatLevel();
        }
    }

    public void ActivateItems() {
        foreach (var i in itemsToActivate) i.StartWithDelay();
    }
}
