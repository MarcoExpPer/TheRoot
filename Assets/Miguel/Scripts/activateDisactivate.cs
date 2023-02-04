using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateDisactivate : MonoBehaviour
{
    [SerializeField]
    GameObject goToToggle;


    public FileData fileData;

    public void toggle()
    {
        goToToggle.SetActive(!goToToggle.activeSelf);
        ButtonToggleFileData a = goToToggle.GetComponent<ButtonToggleFileData>();
        a.transform.position = new Vector3(0, 0, 0);
        a.updateData(fileData);

    }

    public void activate()
    {
        goToToggle.SetActive(true);
        ButtonToggleFileData a = goToToggle.GetComponent<ButtonToggleFileData>();
        a.transform.position = new Vector3(0, 0, 0);
        a.updateData(fileData);
    }

    public void disactivate()
    {
        goToToggle.SetActive(false);
    }
}
