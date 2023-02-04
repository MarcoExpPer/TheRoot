using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExecuteDiscardTrayManager : MonoBehaviour
{
    [SerializeField]
    GameObject _buttonTray;

    [SerializeField]
    Button _trayOpener;
    void Start()
    {
        _buttonTray.SetActive(false);
    }

    // Update is called once per frame
    // Update()
    //{
    //    
    //}

    public void OpenTray()
    {
        _buttonTray.SetActive(true);
        _trayOpener.interactable = false;
    }

    public void CloseTray()
    {
        _buttonTray.SetActive(false);
        _trayOpener.interactable = true;
    }
}
