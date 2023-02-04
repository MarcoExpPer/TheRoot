using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class currentActiveCommandBtn : MonoBehaviour
{
    
    Command command;

    [SerializeField]
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateCommand(Command cmd)
    {
        command = cmd;
        text.text = cmd.commandType + " " + cmd.file.nombre;
    }

}
