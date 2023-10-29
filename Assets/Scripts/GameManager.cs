using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int level = 1;
    public int enemiesInLevel = 1;

    public float bestTime = 276f;

    Timer timerObj;

    //public TextMeshProUGUI timerText;

    public void EnemyDestroyed(){
        enemiesInLevel--;
        if (enemiesInLevel <= 0) {
            //StartCoroutineRestart());
            EndGame();
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

    void EndGame() {
        timerObj = FindObjectOfType<Timer>();
        if ((Time.time - timerObj.startTime) < bestTime) {
            bestTime = (Time.time - timerObj.startTime);
        }

        GameObject.Find("Timer").SetActive(false);
        PlayerHealth ph = FindObjectOfType<PlayerHealth>();
        ph.canMove = false;

        int minutes = Mathf.FloorToInt(bestTime / 60F);
        int seconds = Mathf.FloorToInt(bestTime - minutes * 60);

        string niceTime = "Best Time: " + string.Format("{0:0}:{1:00}", minutes, seconds);
        timerObj.endScreen.SetActive(true);
        timerObj.bestTimeText.text = niceTime;
        
    }

    public void LoadLevel1(){
        enemiesInLevel++;
        SceneManager.LoadScene("test 1");
    }
}