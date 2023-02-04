using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class activateDisactivate : MonoBehaviour
{
    [SerializeField]
    GameObject goToToggle;

    [SerializeField]
    Button btn;
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
        if (!goToToggle.activeSelf)
        {
            goToToggle.SetActive(true);
            ButtonToggleFileData a = goToToggle.GetComponent<ButtonToggleFileData>();
            a.transform.position = new Vector3(0, 0, 0);
            a.updateData(fileData);
        }
    }

    public void disactivate()
    {
        if (goToToggle.activeSelf)
            goToToggle.SetActive(false);
    }

    public void UpdateFileData(FileData newfile)
    {
        fileData = newfile;

        var colors = btn.colors;
        colors.normalColor = newfile.origen;
        colors.disabledColor = newfile.origen;
        colors.highlightedColor = newfile.origen;
        colors.selectedColor = newfile.origen;

        btn.colors = colors;

    }
}
