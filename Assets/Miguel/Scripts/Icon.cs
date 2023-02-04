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

}
