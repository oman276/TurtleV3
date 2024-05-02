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

    Color greenText = new Color(0.49019607843137253f, 0.9529411764705882f, 0.5098039215686274f);
    Color redText = new Color(0.9176470588235294f, 0.3176470588235294f, 0.26666666666666666f);

    public float lastHeldTime;

    public Coroutine respawnCoroutine = null;

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
                GameManager.G.ui.swipeToStart.GetComponent<TextMeshProUGUI>().text = GameManager.G.currentLevel.startText();
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
                GameManager.G.ui.swipeToStart.SetActive(false);
                health.ResetHealth();
                //respawnCoroutine = null;
                break;
        }

        //Incoming State Setup
        switch (newState) {
            case PlayerState.Dead:
                PlayerDeath();
                GameManager.G.ui.swipeToStart.GetComponent<TextMeshProUGUI>().color = redText;
                GameManager.G.ui.swipeToStart.GetComponent<TextMeshProUGUI>().text = "You Died!";
                GameManager.G.ui.swipeToStart.SetActive(true);
                GameManager.G.SwapState(GameState.Defeated);
                break;
            case PlayerState.Active:
                if(GameManager.G.state != GameState.Paused) GameManager.G.SwapState(GameState.Playing);
                break;
            case PlayerState.PostGame:
                movement.DeactivateUIElements();
                break;
            case PlayerState.PreGame:
                GameManager.G.ui.swipeToStart.GetComponent<TextMeshProUGUI>().color = greenText;
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
                lastHeldTime = Time.time;
                break;
            case PlayerState.Held:
                lastHeldTime = Time.time;
                break;
        }

        state = newState;
    }

    public bool isActive() {
        return state == PlayerState.Active || state == PlayerState.Held || state == PlayerState.FirstHeld;
    }

    public bool isHeld() { 
        return state == PlayerState.Held || state == PlayerState.FirstHeld;
    }

    void DisableSwipeText() {
        GameManager.G.ui.swipeToStart.GetComponent<TextMeshProUGUI>().text = "";
        GameManager.G.ui.swipeToStart.SetActive(false);
    }

    public void PlayerDeath() {
        trailRenderer.enabled = true;
        playerCollider.enabled = false;
        rb.velocity = Vector2.zero;
        playerSprite.SetActive(false);
        respawnCoroutine = StartCoroutine(RespawnSafely());
        //restartStartTime = Time.time;
        //Invoke("LoadNewScene", 2f);
    }

    IEnumerator RespawnSafely() {
        yield return new WaitForSeconds(2);
        LoadNewScene();
    }

    void LoadNewScene() {
        StopCoroutine(respawnCoroutine);
        respawnCoroutine = null;
        GameManager.G.ReloadCurrentLevel();
    }

    public void SetupNewScene() {
        //Fill with whatever we need for setup
        movement.FireParticles.SetActive(false);
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

    public void BounceBack(float percentOfMaxVel) {
        Vector2 direction = rb.velocity;
        StopVelocity();
        rb.AddForce(-direction.normalized * (percentOfMaxVel * movement.speed));
    }

    public void ReduceVelocity(float percent) {
        rb.AddForce(-rb.velocity.normalized * movement.speed * percent);
    }
}
