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
        GameManager.G.audio.waterPlaying = false;
        GameManager.G.audio.Stop("running_water");
        GameManager.G.audio.lavaPlaying = false;
        GameManager.G.audio.Stop("lava_sizzle");
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

    IEnumerator LoadInCoroutine(int index, bool isLevel)
    {
        GameManager.G.objectFade.FadeIn(levelFadeTime, fadeSprite);
        yield return new WaitForSeconds(levelFadeTime);
        yield return new WaitForSeconds(0.1f);
        GameManager.G.audio.waterPlaying = false;
        GameManager.G.audio.Stop("running_water");
        GameManager.G.audio.lavaPlaying = false;
        GameManager.G.audio.Stop("lava_sizzle");
        SceneManager.LoadScene(index);
        /*
        if (!isLevel)
        {
            switch (tag)
            {
                case "Menu":
                    GameManager.G.SwapState(GameState.MainMenu);
                    break;
            }
            SetupEndedSignal();
        }
        */
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
        GameManager.G.audio.Play("load_theme");
        StartCoroutine(LoadInCoroutine(tag, isLevel));
    }

    public void LoadLevel(int index, bool isLevel = true)
    {
        Time.timeScale = 1;
        GameManager.G.audio.Play("load_theme");
        StartCoroutine(LoadInCoroutine(index, isLevel));
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
