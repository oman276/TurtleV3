using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    //Temporary: Make Less Terrible

    public void LoadLevel1() {
        SceneManager.LoadScene("tutorial");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("test 1");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("RisingBumpers");
    }

    public void LoadLevel4()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void LoadLevel5()
    {
        SceneManager.LoadScene("RollingBalls");
    }
}
