using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float startTime;
    public TextMeshProUGUI timerText;
    public GameManager gm;

    public GameObject endScreen;
    public TextMeshProUGUI curTimeText;
    public TextMeshProUGUI bestTimeText;
    bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        string niceTime = string.Format("{0:0}:{1:00}", 0, 0);
        timerText.text = niceTime;
        timerText.color = new Color(155f / 255f, 155f / 255f, 155f / 255f);
        gm = FindObjectOfType<GameManager>();
        endScreen.SetActive(false);
    }

    private void Update()
    {
        if (started) {
            float timer = Time.time - startTime;
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);

            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            timerText.text = niceTime;
        }
    }

    public void RestartLevel() {
        gm.LoadLevel1();
    }

    public void StartTimer() {
        if (!started) {
            started = true;
            startTime = Time.time;
            timerText.color = new Color(0, 0, 0);
        }
    }
}
