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

    [SerializeField]
    Button _infoButton;

    public bool newInfo = true;

    [SerializeField]
    public GameObject newInfoIcon;

    void Start()
    {
        _buttonTray.SetActive(false);
        _notificationObject.SetActive(false);
        newInfoIcon.SetActive(true);
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
        if(newInfo)
        {
            _infoButton.Select();
            _infoButton.onClick.Invoke();
            newInfo = !newInfo;
            newInfoIcon.SetActive(false);
        }
        else
        {
            _currentFileButton.Select();
            _currentFileButton.onClick.Invoke();
        }
        //_notificationObject.transform.SetSiblingIndex(0);
    }

    public void CloseNotif()
    {
        _notificationObject.SetActive(false);
        _notificationOpener.interactable = true;
        //_notificationObject.transform.SetSiblingIndex(1);
    }
}
