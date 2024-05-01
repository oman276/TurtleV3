using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialPopup : Activatable
{
    public Collider2D collider;
    public GameObject tutorialPopup;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collider);
        Activate();
    }

    public override void Activate() {
        base.Activate();
        StartCoroutine(ActiveAsync());
    }

    IEnumerator ActiveAsync() {
        yield return new WaitForSeconds(0.2f);
        GameManager.G.SwapState(GameState.Popup);
        tutorialPopup.SetActive(true);

    }

    public void CloseTutorial() {
        //GameManager.G.SwapState(GameState.Playing);
        GameManager.G.audio.Play("menu_click");
        GameManager.G.player.SwapState(PlayerState.Active);
        tutorialPopup.SetActive(false);
    }

}
