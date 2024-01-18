using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectFade : MonoBehaviour
{

    PlayerHealth ph;
    bool isPlayer = false;
    

    private void Start()
    {
        //TODO: Figure Out if we actually need this
        if(isPlayer) ph = FindObjectOfType<PlayerHealth>();
    }

    public void FadeOut(float t, Text i)
    {
        StartCoroutine(FadeTextToZeroAlpha(t, i));
    }

    public void FadeIn(float t, Text i)
    {
        StartCoroutine(FadeTextToFullAlpha(t, i));
    }


    public void FadeOut(float t, Image i)
    {
        StartCoroutine(FadeImageToZeroAlpha(t, i));
    }

    public void FadeIn(float t, Image i)
    {
        StartCoroutine(FadeImageToFullAlpha(t, i));
    }

    public void FadeIn(float t, GameObject obj)
    {
        StartCoroutine(FadeObjectToFullAlpha(t, obj));
    }

    public void FadeOut(float t, GameObject obj)
    {
        StartCoroutine(FadeObjectToZeroAlpha(t, obj));
    }

    IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }


    IEnumerator FadeImageToFullAlpha(float t, Image i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    IEnumerator FadeImageToZeroAlpha(float t, Image i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a);
        while (i && i.color.a > 0.0f)
        {
            if (isPlayer)
            {
                //if (ph.fadeState != 4)
                {
                    i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
                    break;
                }
            }
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        /*
        if (isPlayer && ph.fadeState == 4)
        {
            ph.fadeState = 0;
        }
        */
    }

    IEnumerator FadeObjectToFullAlpha(float t, GameObject obj)
    {
        SpriteRenderer i = obj.GetComponent<SpriteRenderer>();
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i && i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    IEnumerator FadeObjectToZeroAlpha(float t, GameObject obj)
    {
        SpriteRenderer i = obj.GetComponent<SpriteRenderer>();
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
