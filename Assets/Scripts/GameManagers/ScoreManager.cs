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
                ("Josiah", 8f),
                ("Owen", 10f),
                ("Ryan", 12f),
                ("Hadas", 31f),
                ("Linda", 58f),
            };
            List<(string, float)> l2 = new List<(string, float)> {
                ("Owen", 16f),
                ("Josiah", 18f),
                ("Nicky", 20f),
                ("Jay", 45f),
                ("Hadas", 62f),
            };
            List<(string, float)> l3 = new List<(string, float)> {
                ("Josiah", 22f),
                ("Owen", 26f),
                ("Maximo", 30f),
                ("River", 32f),
                ("Nicole", 51f),
            };
            List<(string, float)> l4 = new List<(string, float)> {
                ("Ryan", 24f),
                ("Owen", 27f),
                ("River", 31f),
                ("Maximo", 35f),
                ("Nicole", 47f),
            };
            List<(string, float)> l5 = new List<(string, float)> {
                ("Maximo", 25f),
                ("Josiah", 29f),
                ("Ryan", 32f),
                ("Owen", 35f),
                ("Nicole", 44f),
            };
            List<(string, float)> l6 = new List<(string, float)> {
                ("Ryan", 15f),
                ("Owen", 17f),
                ("Josiah", 21f),
                ("River", 25f),
                ("Nicole", 33f),
            };
            List<(string, float)> l7 = new List<(string, float)> {
                ("Ryan", 20f),
                ("River", 24f),
                ("Maximo", 27f),
                ("Owen", 31f),
                ("Nicole", 39f),
            };
            List<(string, float)> l8 = new List<(string, float)> {
                ("Josiah", 22f),
                ("Maximo", 23f),
                ("Ryan", 24f),
                ("Nicole", 27f),
                ("Owen", 33f),
            };

            highScores.Insert(0, l1);
            highScores.Insert(1, l2);
            highScores.Insert(2, l3);
            highScores.Insert(3, l4);
            highScores.Insert(4, l5);
            highScores.Insert(5, l6);
            highScores.Insert(6, l7);
            highScores.Insert(7, l8);
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
