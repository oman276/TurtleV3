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
        item = GetComponent<TextMeshProUGUI>();
        ChangeScore();
    }

    public void ChangeScore() { 
    }
}
