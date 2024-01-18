using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum UIState { 
    MainMenu,
    LevelSelect,
    InGame,
    Pause,
    GameOver,
    PostGame
}

public class UIManager : MonoBehaviour
{
    public UIState state;

    //Main Menu

    //Level Select

    //General Game UI
    public GameObject TimerObjects;

    //Level Beat Screen
    public GameObject levelBeatScreen;
    public TextMeshProUGUI curTimeText;
    public TextMeshProUGUI bestTimeText;
    Color green = new Color(157f / 225f, 233f / 225f, 188f / 225f);
    Color gold = new Color(210f / 255f, 224f / 255f, 64f / 255f);

    public Timer gameTimer;

    public void SwapState(UIState newState) {
        if (newState == state) return;

        //Outgoing State Actions
        switch (state) {
            case UIState.InGame:
                TimerObjects.SetActive(false);
                break;
            case UIState.PostGame:
                levelBeatScreen.SetActive(false);
                break;
        }

        //Incoming State Actions
        switch (newState) {
            case UIState.InGame:
                TimerObjects.SetActive(true);
                gameTimer.timerText.text = string.Format("{0:0}:{1:00}", 0, 0);
                break;
            case UIState.PostGame:
                gameTimer.EndTimer();
                SetNewTime();
                levelBeatScreen.SetActive(true);
                break;
        }
        
        state = newState;
    }

    void SetNewTime() {
        int minutes = Mathf.FloorToInt(gameTimer.lastTime / 60F);
        int seconds = Mathf.FloorToInt(gameTimer.lastTime - minutes * 60);

        string niceTime = "Current Time: " + string.Format("{0:0}:{1:00}", minutes, seconds);
        curTimeText.text = niceTime;

        string bestBase = "Best Time: ";
        if (gameTimer.lastTime < GameManager.G.ActiveBestTime())
        {
            GameManager.G.bestTimes[GameManager.G.currentLevel.levelName] = gameTimer.lastTime;
            bestBase = "New Best Time: ";
            bestTimeText.color = gold;
        }
        else
        {
            bestTimeText.color = green;
        }

        minutes = Mathf.FloorToInt(GameManager.G.ActiveBestTime() / 60F);
        seconds = Mathf.FloorToInt(GameManager.G.ActiveBestTime() - minutes * 60);

        niceTime = bestBase + string.Format("{0:0}:{1:00}", minutes, seconds);
        bestTimeText.text = niceTime;
    }
}
