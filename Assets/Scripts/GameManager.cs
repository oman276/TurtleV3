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

    Color green = new Color(157f / 225f, 233f / 225f, 188f / 225f);
    Color gold = new Color(210f / 255f, 224f / 255f, 64f / 255f);

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
        
        PlayerHealth ph = FindObjectOfType<PlayerHealth>();
        ph.canMove = false;

        timerObj = FindObjectOfType<Timer>();
        float currentTime = Time.time - timerObj.startTime;

        

        int minutes = Mathf.FloorToInt(currentTime / 60F);
        int seconds = Mathf.FloorToInt(currentTime - minutes * 60);

        string niceTime = "Current Time: " + string.Format("{0:0}:{1:00}", minutes, seconds);
        timerObj.curTimeText.text = niceTime;

        string bestBase = "Best Time: ";
        timerObj.endScreen.SetActive(true);

        if (currentTime < bestTime)
        {
            bestTime = currentTime;
            bestBase = "New Best Time: ";
            timerObj.bestTimeText.color = gold;
        }
        else{
            timerObj.bestTimeText.color = green;
        }
        minutes = Mathf.FloorToInt(bestTime / 60F);
        seconds = Mathf.FloorToInt(bestTime - minutes * 60);

        niceTime = bestBase + string.Format("{0:0}:{1:00}", minutes, seconds);
        timerObj.bestTimeText.text = niceTime;

        GameObject.Find("Timer").SetActive(false);
        timerObj.endScreen.SetActive(true);
    }

    public void LoadLevel1(){
        enemiesInLevel++;
        SceneManager.LoadScene("level1");
    }
}