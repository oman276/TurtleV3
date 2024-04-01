using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using System;

public enum GameState { 
    Playing,
    PreStart,
    Paused,
    Defeated,
    LevelBeat,
    MainMenu,
    LevelSelect
}

public class GameManager : MonoBehaviour
{
    public static GameManager G { get; private set; }

    //Player Info
    public PlayerManager player;

    //Camera Info
    public GameObject cameraObject;

    //Level Manager
    public LevelManager currentLevel;

    //UI Manager
    public UIManager ui;
    public GameObject mainMenuCamZone;
    public GameObject mainMenuCamTarget;

    public Dictionary<string, float> bestTimes = new Dictionary<string, float>();
    public ObjectFade objectFade;

    public AudioManager audio;
    public LoadManager load;
    public ScoreManager scores;
    public int activeLevelIndex = 0;

    private void Awake()
    {
        if (G != null && G != this)
        {
            Destroy(this);
        }
        else
        {
            G = this;
        }
    }

    public GameState state = GameState.MainMenu;

    public void SwapState(GameState newState)
    {
        if (newState == state) return;

        
        //Outgoing State setup
        switch (state)
        {
            case GameState.Defeated:
                ui.gameTimer.EndTimer();
                break;
            case GameState.LevelSelect:
                mainMenuCamZone.SetActive(false);
                break;
            case GameState.Paused:
                Time.timeScale = 1;
                player.SwapState(PlayerState.Active);
                break;
        }
        
        //Incoming State Setup
        switch (newState)
        {
            case GameState.LevelSelect:
                ui.SwapState(UIState.LevelSelect);
                break;
            case GameState.LevelBeat:
                ui.SwapState(UIState.PostGame);
                player.SwapState(PlayerState.PostGame);
                break;
            case GameState.PreStart:
                ui.SwapState(UIState.InGame);
                player.SwapState(PlayerState.PreGame);
                break;
            case GameState.MainMenu:
                ui.SwapState(UIState.MainMenu);
                player.SwapState(PlayerState.Disabled);
                mainMenuCamZone.GetComponent<CinemachineVirtualCamera>().Follow =
                    mainMenuCamTarget.transform;
                mainMenuCamZone.SetActive(true);
                break;
            case GameState.Paused:
                Time.timeScale = 0;
                ui.SwapState(UIState.Pause);
                player.SwapState(PlayerState.ManualPause);
                break;
            case GameState.Playing:
                ui.SwapState(UIState.InGame);
                break;
        }
        
        state = newState;
    }

    public void ReloadCurrentLevel() {
        load.LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void StartLevel(LevelManager lm) {
        currentLevel = lm;
        SwapState(GameState.PreStart);
        ui.SwapState(UIState.InGame);
        if (!bestTimes.ContainsKey(currentLevel.levelName)) {
            bestTimes.Add(currentLevel.levelName, 9999);
        }
        player.StopVelocity();
        player.playerObject.transform.position = currentLevel.playerSpawn.position;
        load.SetupEndedSignal();
    }

    public void BeatLevel() {
        SwapState(GameState.LevelBeat);
    }

    public float ActiveBestTime() {
        return GameManager.G.bestTimes[GameManager.G.currentLevel.levelName];
    }

    public void LoadLevelSelect() {
        SwapState(GameState.LevelSelect);
    }

    public void BackToMenu() {
        load.LoadLevel("Menu", false);
    }

    public void PrintBestTimes() {
        string s = "";
        foreach (var i in bestTimes) {
            s += i.Key + "  -  " + i.Value + "\n";
        }
        Debug.Log(s);
    }

    public void PauseGame() {
        if (state != GameState.Paused) SwapState(GameState.Paused);
        else SwapState(GameState.Playing);
    }
    
}