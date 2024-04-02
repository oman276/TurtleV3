using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float startTime;
    public TextMeshProUGUI timerText;

    bool started = false;

    float timer = 0;
    public float lastTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        string niceTime = string.Format("{0:0}:{1:00}", 0, 0);
        timerText.text = niceTime;
        timerText.color = new Color(155f / 255f, 155f / 255f, 155f / 255f);
    }

    private void Update()
    {
        if (started && GameManager.G.state == GameState.Playing) {
            //float timer = Time.time - startTime;
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            timerText.text = niceTime;
        }
    }

    public void StartTimer() {
        if (!started) {
            started = true;
            startTime = Time.time;
            timer = 0;
            timerText.color = new Color(0, 0, 0);
        }
    }

    public void EndTimer() {
        if (started) {
            started = false;
            lastTime = timer;
            timer = 0;
            timerText.text = string.Format("{0:0}:{1:00}", 0, 0);
            timerText.color = new Color(155f / 255f, 155f / 255f, 155f / 255f);
        }
    }

    public float GetTime() {
        return (started ? timer : lastTime);
    }

    public void resetTimer() {
        timer = 0;
    }
}
