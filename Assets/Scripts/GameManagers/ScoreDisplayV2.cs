using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplayV2 : MonoBehaviour
{
    public int levelIndex;
    public TextMeshProUGUI[] textLines;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScores();
    }

    public void UpdateScores() {
        int i = 0;
        foreach (TextMeshProUGUI t in textLines) {
            (string, float) result = GameManager.G.scores.highScores[levelIndex][i];
            t.text = (i+1) + ". " + result.Item1 + " ~ " + GameManager.G.scores.formatTime(result.Item2);
            ++i;
        }
    }

    
}
