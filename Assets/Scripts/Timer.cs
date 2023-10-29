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
    public TextMeshProUGUI bestTimeText;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        float timer = Time.time - startTime;
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);

        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        timerText.text = niceTime;     
    }

    public void RestartLevel() {
        gm.LoadLevel1();
    }
}
