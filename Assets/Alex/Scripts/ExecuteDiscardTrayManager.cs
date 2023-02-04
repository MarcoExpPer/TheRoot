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

    [SerializeField]
    GameObject _notificationObject;

    [SerializeField]
    Button _notificationOpener;

    [SerializeField]
    Button _currentFileButton;
    void Start()
    {
        _buttonTray.SetActive(false);
        _notificationObject.SetActive(false);
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
        //_buttonTray.transform.SetSiblingIndex(0);
    }

    public void CloseTray()
    {
        _buttonTray.SetActive(false);
        _trayOpener.interactable = true;
        //_buttonTray.transform.SetSiblingIndex(1);
    }

    public void OpenNotif()
    {
        _notificationObject.SetActive(true);
        _notificationOpener.interactable = false;
        _currentFileButton.Select();
        _currentFileButton.onClick.Invoke();
        //_notificationObject.transform.SetSiblingIndex(0);
    }

    public void CloseNotif()
    {
        _notificationObject.SetActive(false);
        _notificationOpener.interactable = true;
        //_notificationObject.transform.SetSiblingIndex(1);
    }
}
