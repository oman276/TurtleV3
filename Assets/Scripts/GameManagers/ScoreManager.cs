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
                ("Josiah", 8.99f),
                ("Owen", 10.99f),
                ("Ryan", 12.99f),
                ("Hadas", 31.99f),
                ("Linda", 58.99f),
            };
            List<(string, float)> l2 = new List<(string, float)> {
                ("Owen", 16.99f),
                ("Josiah", 18.99f),
                ("Nicky", 20.99f),
                ("Jay", 45.99f),
                ("Hadas", 62.99f),
            };
            List<(string, float)> l3 = new List<(string, float)> {
                ("Josiah", 22.99f),
                ("Owen", 26.99f),
                ("Maximo", 30.99f),
                ("River", 32.99f),
                ("Nicole", 51.99f),
            };
            List<(string, float)> l4 = new List<(string, float)> {
                ("Ryan", 24.99f),
                ("Owen", 27.99f),
                ("River", 31.99f),
                ("Maximo", 35.99f),
                ("Nicole", 47.99f),
            };
            List<(string, float)> l5 = new List<(string, float)> {
                ("Maximo", 25.99f),
                ("Josiah", 29.99f),
                ("Ryan", 32.99f),
                ("Owen", 35.99f),
                ("Nicole", 44.99f),
            };
            List<(string, float)> l6 = new List<(string, float)> {
                ("Ryan", 15.99f),
                ("Owen", 17.99f),
                ("Josiah", 21.99f),
                ("River", 25.99f),
                ("Nicole", 33.99f),
            };
            List<(string, float)> l7 = new List<(string, float)> {
                ("Ryan", 20.99f),
                ("River", 24.99f),
                ("Maximo", 27.99f),
                ("Owen", 31.99f),
                ("Nicole", 39.99f),
            };
            List<(string, float)> l8 = new List<(string, float)> {
                ("Josiah", 22.99f),
                ("Maximo", 23.99f),
                ("Ryan", 24.99f),
                ("Nicole", 27.99f),
                ("Owen", 33.99f),
            };
            List<(string, float)> l9 = new List<(string, float)> {
                ("Nicole", 59.99f),
                ("Owen", 67.99f),
                ("Ryan", 72.99f),
                ("Josiah", 87.99f),
                ("Elliot", 98.99f),
            };

            highScores.Insert(0, l1);
            highScores.Insert(1, l2);
            highScores.Insert(2, l3);
            highScores.Insert(3, l4);
            highScores.Insert(4, l5);
            highScores.Insert(5, l6);
            highScores.Insert(6, l7);
            highScores.Insert(7, l8);
            highScores.Insert(8, l9);
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
