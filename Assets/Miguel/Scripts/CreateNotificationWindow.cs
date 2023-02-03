using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNotificationWindow : MonoBehaviour
{

    [SerializeField] private GameObject Icon;
    private Transform WindowsIconBar;

    // Start is called before the first frame update
    void Start()
    {
        WindowsIconBar = transform.parent.GetChild(0);

        createWindowsBarIcon();
    }

    private void createWindowsBarIcon()
    {
        GameObject icon = Instantiate(Icon, WindowsIconBar);
        icon.GetComponent<Icon>().window = gameObject;
    }

}
