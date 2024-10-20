using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public void FullScreen()
    {
        Screen.SetResolution(1920,1080, true);
    }

    public void Window()
    {
        Screen.SetResolution(1600, 900, false);
    }
}
