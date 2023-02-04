using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FileDataWindow : MonoBehaviour
{


    [HideInInspector] public bool minimized = false;




    public void updateData(FileData file, ESlotState state)
    {
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();

        if (state == ESlotState.BLOCKED)
        {
            texts[0].text = "BLOCKED";
            texts[1].text = "BLOCKED";
            texts[2].text = "BLOCKED";
            texts[3].text = "BLOCKED";
        }
        else if (state == ESlotState.VIRUS)
        {
            texts[0].text = "VIRUS";
            texts[1].text = "VIRUS";
            texts[2].text = "VIRUS";
            texts[3].text = "VIRUS";
        }
        else
        {
            texts[0].text = file.nombre;
            texts[1].text = file.size.ToString();
            texts[2].text = file.date.ToString();
            texts[3].text = file.NameToReplace;
        }
    }
}
