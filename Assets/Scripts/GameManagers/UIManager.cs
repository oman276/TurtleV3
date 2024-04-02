using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum UIState { 
    MainMenu,
    LevelSelect,
    InGame,
    Pause,
    GameOver,
    PostGame,
    Popup,
}

public class UIManager : MonoBehaviour
{
    public UIState state = UIState.MainMenu;

    //Main Menu
    public GameObject mainMenuUI;

    //Level Select
    public GameObject levelSelectUI;
    public ScoreDisplay[] scores;

    //General Game UI
    public GameObject TimerObjects;
    public GameObject shadow;
    public GameObject swipeToStart;
    public Image swipeLine;
    public GameObject pauseButton;
    public GameObject pauseMenu;
    [HideInInspector]
    public Vector3 swipeStart;

    //Level Beat Screen
    public GameObject levelBeatScreen;
    public TextMeshProUGUI curTimeText;
    public TextMeshProUGUI bestTimeText;
    Color green = new Color(157f / 225f, 233f / 225f, 188f / 225f);
    Color gold = new Color(210f / 255f, 224f / 255f, 64f / 255f);

    public Timer gameTimer;

    public GameObject levelSelectScroll;
    Vector3 levelSelectBaseTrans;

    private void Start()
    {
        levelSelectBaseTrans = levelSelectScroll.transform.position;
    }

    public void SwapState(UIState newState) {
        if (newState == state) return;

        //Outgoing State Actions
        switch (state) {
            case UIState.MainMenu:
                mainMenuUI.SetActive(false);
                break;
            case UIState.InGame:
                TimerObjects.SetActive(false);
                pauseButton.SetActive(false);
                shadow.GetComponent<Image>().enabled = false;
                break;
            case UIState.PostGame:
                levelBeatScreen.SetActive(false);
                break;
            case UIState.LevelSelect:
                levelSelectUI.SetActive(false);
                break;
            case UIState.Pause:
                pauseMenu.SetActive(false);
                break;
            case UIState.Popup:
                TimerObjects.SetActive(true);
                pauseButton.SetActive(true);
                swipeToStart.SetActive(true);
                break;
        }

        //Incoming State Actions
        switch (newState) {
            case UIState.MainMenu:
                mainMenuUI.SetActive(true);
                break;
            case UIState.LevelSelect:
                levelSelectScroll.transform.position = levelSelectBaseTrans;
                foreach (ScoreDisplay sc in scores) {
                    sc.ChangeScore();
                }
                levelSelectUI.SetActive(true);
                break;
            case UIState.InGame:
                TimerObjects.SetActive(true);
                pauseButton.SetActive(true);
                shadow.GetComponent<Image>().enabled = true;
                gameTimer.timerText.text = string.Format("{0:0}:{1:00}", 0, 0);
                break;
            case UIState.PostGame:
                gameTimer.EndTimer();
                SetNewTime();
                levelBeatScreen.SetActive(true);
                break;
            case UIState.Pause:
                pauseMenu.SetActive(true);
                break;
            case UIState.Popup:
                TimerObjects.SetActive(false);
                pauseButton.SetActive(false);
                swipeToStart.SetActive(false);
                break;
        }
        
        state = newState;
    }

    void SetNewTime() {
        string niceTime = "Current Time: " + GameManager.G.scores.formatTime(gameTimer.lastTime);
        curTimeText.text = niceTime;

        int scoreResult = GameManager.G.scores.InsertNewScore(GameManager.G.activeLevelIndex, gameTimer.lastTime);
        string resultText = "";
        bestTimeText.color = green;

        switch (scoreResult) {
            case 0:
                bestTimeText.color = gold;
                resultText = "New #1 Time! Wow!";
                break;
            case 5:
                resultText = "No record beat... yet!";
                break;
            default:
                resultText = "New #" + (scoreResult + 1) + " Time!";
                break;
        }
        bestTimeText.text = resultText;
    }
}
