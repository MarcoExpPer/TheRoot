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
        a.updateData(fileData);

    }
}
