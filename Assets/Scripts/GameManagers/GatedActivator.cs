using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GatedActivator : MonoBehaviour
{
    public bool activatorEnabled = true;
    
    public int levelToCheck;
    public bool doNotActivateOnLastLevel = false;


    private void OnEnable()
    {
        if ((SceneManager.GetActiveScene().buildIndex == 9 && doNotActivateOnLastLevel == true)
            || GameManager.G.scores.completed[levelToCheck] == false) this.gameObject.SetActive(false);
    }

}
