using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public GameObject nextLevelButton;

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex == 9) nextLevelButton.SetActive(false);
    }

    private void OnDisable()
    {
        nextLevelButton.SetActive(true);
    }

    public void LoadNextLevel() {
        if (SceneManager.GetActiveScene().buildIndex != 9) GameManager.G.load.LoadLevel(GameManager.G.currentLevel.nextLevel);
    }
}
