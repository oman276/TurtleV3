using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    public CameraMainMovement camMovement;
    public GameObject cameraObject;
    public Camera mainCamera;
    public GameObject cameraTilt;

    //Level Manager
    public LevelManager currentLevel;

    //UI Manager
    public UIManager ui;

    public Dictionary<string, float> bestTimes = new Dictionary<string, float>();

    public ObjectFade objectFade;

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

    //Figure out how to manage scene loading after this is done

    public void SwapState(GameState newState)
    {
        if (newState == state) return;

        
        //Outgoing State setup
        switch (state)
        {
            case GameState.Defeated:
                ui.gameTimer.EndTimer();
                break;
        }
        

        //Incoming State Setup
        switch (newState)
        {
            case GameState.LevelBeat:
                ui.SwapState(UIState.PostGame);
                player.SwapState(PlayerState.PostGame);
                break;
            case GameState.PreStart:
                ui.SwapState(UIState.InGame);
                player.SwapState(PlayerState.PreGame);
                break;
        }
        
        state = newState;
    }

    public void ReloadCurrentLevel() {
        SwapState(GameState.PreStart);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartLevel(LevelManager lm) {
        currentLevel = lm;
        SwapState(GameState.PreStart);
        ui.SwapState(UIState.InGame);
        if (!bestTimes.ContainsKey(currentLevel.levelName)) {
            bestTimes.Add(currentLevel.levelName, 276);
        }
        //cameraObject.transform.position = currentLevel.cameraSpawn.position;
        camMovement.swapZones(currentLevel.startingCamZone.lowerLeft, currentLevel.startingCamZone.upperRight,
            currentLevel.startingCamZone.zoneTarget);
        player.StopVelocity();
        player.playerObject.transform.position = currentLevel.playerSpawn.position;
    }

    public void BeatLevel() {
        SwapState(GameState.LevelBeat);
        //Timer setup stuff
    }

    public float ActiveBestTime() {
        return GameManager.G.bestTimes[GameManager.G.currentLevel.levelName];
    }
}