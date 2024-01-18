using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LevelManager : MonoBehaviour
{
    public int numberOfEnemies = 1;
    public string levelName = "test 1";
    public Transform playerSpawn;
    public Transform cameraSpawn;
    public CameraZone startingCamZone;

    private void Start()
    {
        GameManager.G.StartLevel(this);
    }

    public void EnemyDefeated() {
        numberOfEnemies--;
        if (numberOfEnemies == 0) {
            GameManager.G.BeatLevel();
        }
    }
}
