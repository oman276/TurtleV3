using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int level = 1;
    public int enemiesInLevel = 1;

    public void EnemyDestroyed() {
        enemiesInLevel--;
        if (enemiesInLevel == 0) {
            StartCoroutine(Restart());
        }
    }

    IEnumerator Restart() {
        yield return new WaitForSeconds(1.5f);
        int newLevel = level;
        while (newLevel == level) {
            newLevel = Random.Range(1, 10);
        }
        string levelName = "Level" + newLevel;
        SceneManager.LoadScene(levelName);
    }

    public void LoadLevel1(){
        SceneManager.LoadScene("test 1");
    }
}
