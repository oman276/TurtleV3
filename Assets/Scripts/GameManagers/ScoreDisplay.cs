using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public string levelName;
    TextMeshProUGUI item;

    private void Start()
    {
        
        ChangeScore();
    }

    public void ChangeScore() {
        item = GetComponent<TextMeshProUGUI>();

        if (GameManager.G.bestTimes.ContainsKey(levelName)) {
            float bestTime = GameManager.G.bestTimes[levelName];
            int minutes = Mathf.FloorToInt(bestTime / 60F);
            int seconds = Mathf.FloorToInt(bestTime - minutes * 60);

            item.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
        else{
            item.text = "NEW";
        }
    }
}
