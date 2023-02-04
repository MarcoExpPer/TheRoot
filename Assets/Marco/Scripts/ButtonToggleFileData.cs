using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonToggleFileData : MonoBehaviour
{
    // Start is called before the first frame update
   
    [SerializeField]
    TextMeshProUGUI text;

    public void updateData(FileData fileData)
    {
        text.text =
            "<b> " + fileData.nombre + "</b> \n" +
            "Size: " + fileData.size.ToString() + "\n" +
            "Date: " + fileData.date.ToString() + "\n";
        
        if(fileData.NameToReplace != null && fileData.NameToReplace != "")
        {
            text.text += "File to Replace: " + fileData.NameToReplace;
        }
    }
}
