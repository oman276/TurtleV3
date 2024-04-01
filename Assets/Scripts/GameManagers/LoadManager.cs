using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public float levelFadeTime = 0.3f;
    public Image fadeSprite;

    IEnumerator LoadInCoroutine(string tag, bool isLevel) {
        GameManager.G.objectFade.FadeIn(levelFadeTime, fadeSprite);
        yield return new WaitForSeconds(levelFadeTime);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(tag);
        if (!isLevel) {
            switch (tag) {
                case "Menu":
                    GameManager.G.SwapState(GameState.MainMenu);
                    break;
            }
            SetupEndedSignal();
        }
    }
    
    IEnumerator LoadOutCoroutine() {
        GameManager.G.objectFade.FadeOut(levelFadeTime, fadeSprite);
        yield return new WaitForSeconds(levelFadeTime);
    }

    public void SetupEndedSignal() {
        StartCoroutine(LoadOutCoroutine());
    }

    public void LoadLevel(string tag, bool isLevel = true) {
        Time.timeScale = 1;
        StartCoroutine(LoadInCoroutine(tag, isLevel));
    }

    /*
    public void LoadLevel1() {
        LoadLevel("tutorial");
    }

    public void LoadLevel2()
    {
        LoadLevel("test 1");
    }

    public void LoadLevel3()
    {
        LoadLevel("RisingBumpers");
    }

    public void LoadLevel4()
    {
        LoadLevel("Level 2");
    }

    public void LoadLevel5()
    {
        LoadLevel("RollingBalls");
    }

    public void LoadLevel6()
    {
        LoadLevel("Level6");
    }

    public void LoadLevel7()
    {
        LoadLevel("spotlight");
    }

    public void LoadLevel8()
    {
        LoadLevel("Darts");
    }
    */
}
