using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum PlayerState { 
    PreGame,
    FirstHeld,
    Active,
    PostGame,
    Dead,
    MovementLocked,
    Held,
    ImpactPause,
    ManualPause,
    Disabled
}

public class PlayerManager : MonoBehaviour
{
    public NewMovement movement;
    public PlayerHealth health;
    public GameObject playerObject;
    public GameObject playerSprite;
    public LineRenderer directionLine;

    public PlayerState state = PlayerState.Disabled;

    Rigidbody2D rb;
    CircleCollider2D playerCollider;
    TrailRenderer trailRenderer;

    private void Start()
    {
        PlayerSetup();
    }

    void PlayerSetup() {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CircleCollider2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void SwapState(PlayerState newState) {
        if (newState == state) return;
        PlayerSetup();

        //Outgoing State takedown
        switch (state) {
            case PlayerState.FirstHeld:
                GameManager.G.ui.gameTimer.StartTimer();
                movement.DeactivateUIElements();
                GameManager.G.ui.swipeToStart.GetComponent<TextMeshProUGUI>().text = "Go!";
                GameManager.G.currentLevel.ActivateItems();
                Invoke("DisableSwipeText", 1f);
                break;
            case PlayerState.Held:
                movement.DeactivateUIElements();
                break;
            case PlayerState.PreGame:
                trailRenderer.enabled = true;
                break;
            case PlayerState.Disabled:
                playerSprite.SetActive(true);
                break;
            case PlayerState.Dead:
                health.ResetHealth();
                break;
        }

        //Incoming State Setup
        switch (newState) {
            case PlayerState.Dead:
                PlayerDeath();
                GameManager.G.SwapState(GameState.Defeated);
                break;
            case PlayerState.Active:
                GameManager.G.SwapState(GameState.Playing);
                break;
            case PlayerState.PostGame:
                movement.DeactivateUIElements();
                break;
            case PlayerState.PreGame:
                GameManager.G.ui.swipeToStart.GetComponent<TextMeshProUGUI>().text = "Swipe To Start";
                GameManager.G.ui.swipeToStart.SetActive(true);
                health.lavaCount = 0;
                trailRenderer.enabled = false;
                SetupNewScene();
                break;
            case PlayerState.Disabled:
                trailRenderer.enabled = false;
                playerSprite.SetActive(false);
                StopVelocity();
                break;
            case PlayerState.FirstHeld:
                GameManager.G.ui.swipeToStart.GetComponent<TextMeshProUGUI>().text = "Ready...";
                break;
        }

        state = newState;
    }

    public bool isActive() {
        return state == PlayerState.Active || state == PlayerState.Held || state == PlayerState.FirstHeld;
    }

    void DisableSwipeText() {
        GameManager.G.ui.swipeToStart.SetActive(false);
    }

    public void PlayerDeath() {
        trailRenderer.enabled = true;
        playerCollider.enabled = false;
        rb.velocity = Vector2.zero;
        playerSprite.SetActive(false);
        Invoke("LoadNewScene", 3f);
    }

    void LoadNewScene() {
        GameManager.G.ReloadCurrentLevel();
    }

    public void SetupNewScene() {
        //Fill with whatever we need for setup
        playerCollider.enabled = true;
        rb.velocity = Vector2.zero;
        playerSprite.SetActive(true);
    }

    public void StopVelocity() {
        if (rb) rb.velocity = Vector2.zero;
    }

    public void AddForce(Vector2 direction) {
        rb.AddForce(direction);
    }
}
