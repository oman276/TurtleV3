using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject selectables;

    public TextMeshProUGUI titleCard;
    string levelTag = "";
    int currentIndex;

    public ScoreDisplayV2 display;

    public void PlayGame() {
        GameManager.G.activeLevelIndex = currentIndex;
        if(levelTag != "") GameManager.G.load.LoadLevel(levelTag);
    }

    void UpdateTags(string _levelTag, int index, string publicName) {
        titleCard.text = publicName;
        levelTag = _levelTag;
        display.levelIndex = index;
        currentIndex = index;
        display.UpdateScores();
        selectables.SetActive(true);
    }

    private void OnDisable()
    {
        selectables.SetActive(false);
    }

    //Level Select Buttons
    public void Level1() {
        UpdateTags("tutorial", 0, "Tutorial");
    }
    public void Level2()
    {
        UpdateTags("test 1", 1, "Coal Walk");
    }
    public void Level3()
    {
        UpdateTags("RisingBumpers", 2, "Watch Out Below");
    }
    public void Level4()
    {
        UpdateTags("Level 2", 3, "Turtle Falls");
    }
    public void Level5()
    {
        UpdateTags("Rolling Balls", 4, "Rolling Stones");
    }
    public void Level6()
    {
        UpdateTags("Level6", 5, "Smoke and Slide");
    }
    public void Level7()
    {
        UpdateTags("spotlight", 6, "On The Run");
    }
    public void Level8()
    {
        UpdateTags("darts", 7, "Under Fire");
    }

}
