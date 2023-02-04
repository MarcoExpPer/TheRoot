using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{

    [HideInInspector] public GameObject window;

    public void OpenWindow()
    {
        window.SetActive(true);
        window.GetComponent<Window>().minimized = false;
    }

    public void CloseWindow()
    {
        window.SetActive(false);
        window.GetComponent<Window>().minimized = true;
    }

    public void ToggleWindow()
    {
        if (window.activeSelf)
        {
            CloseWindow();
        }
        else
        {
            OpenWindow();
        }
    }

}
