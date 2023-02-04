using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FileDataWindow : MonoBehaviour
{
    [HideInInspector] public bool minimized = false;

    [SerializeField]
    TextMeshProUGUI text;

    public void updateData(FileData fileData, ESlotState state)
    {
        if (state == ESlotState.BLOCKED)
        {
            text.text = "Archivo bloqueado";
        }
        else if (state == ESlotState.VIRUS)
        {
            text.text = "VIRUS VIRUS VIRUS VIRUS VIRUS VIRUS   vVIRUSVIRUSVIRUSVIRUSVIRUSVIRUSVIRUSVIRUSVIRUSVIRUSVIRUS\n VIRUS";
        }
        else
        {
            text.text =
               "<b> " + fileData.nombre + "</b> \n" +
               "Size: " + fileData.size.ToString() + "\n" +
               "Date: " + fileData.date.ToString() + "\n";

            if (fileData.NameToReplace != null && fileData.NameToReplace != "")
            {
                text.text += "File that will replace the previous file: " + fileData.NameToReplace;
            }
        }
    }
}
