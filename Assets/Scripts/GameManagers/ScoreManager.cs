using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // highScores[level][score] -> (name, time)
    public List<List<(string, float)>> highScores;
    public string playerName = "Demo Player";

    // Start is called before the first frame update
    void Start()
    {
        //High Scores Setup
        {
            highScores = new List<List<(string, float)>>();
            
            //High Scores Initialization
            List<(string, float)> l1 = new List<(string, float)> {
                ("L1 Test1", 35f),
                ("L1 Test2", 45f),
                ("L1 Test3", 55f),
                ("L1 Test4", 65f),
                ("L1 Test5", 75f),
            };
            List<(string, float)> l2 = new List<(string, float)> {
                ("L1 Test1", 115f),
                ("L1 Test2", 125f),
                ("L1 Test3", 135f),
                ("L1 Test4", 145f),
                ("L1 Test5", 155f),
            };

            highScores.Insert(0, l1);
            highScores.Insert(1, l2);
            highScores.Insert(2, l1);
            highScores.Insert(3, l2);
            highScores.Insert(4, l1);
            highScores.Insert(5, l2);
            highScores.Insert(6, l1);
            highScores.Insert(7, l2);
        }
    }

    //Returns new placement index (if 5, not placed)
    public int InsertNewScore(int level, float time)
    {
        List<(string, float)> current = highScores[level];
        int i = 0;
        while (i < 5)
        {
            (string, float) data = current[i];
            if (time <= data.Item2)
            {
                //Insert here
                current.Insert(i, (playerName, time));
                current.RemoveAt(5);
                break;
            }
            ++i;
        }
        return i;
    }

    public string formatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

}
