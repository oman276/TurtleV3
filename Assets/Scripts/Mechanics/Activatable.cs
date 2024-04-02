using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour
{
    public bool activateOnStart = false;
    public bool activated = false;

    private void Start()
    {
        if (activateOnStart) Activate();
    }

    public virtual void Activate() {
        activated = true;
    }
}
