using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationInstructionController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    TextMeshProUGUI _texto;

    private string textString = "The Nantendo® company notifies you:\n\nWe have sent you a command to operate on the root, please verify\nthe details of the file(s) and operation before executing or discarding\nit.Your salary will depend on it.\n\n";

    private string level1NormalInfo= "Basic instructions:\n\n- File Operations can be differentiated by color and symbol:\n-> Green Plus Sign: ADD\n-> Red X Sign: DELETE\n-> Blue File Transfer Sign: REPLACE\n-> Yellow Duplication Sign: COPY\n\n";
    
    private string level2NewInfo = "New Info:\n\n- All Commands must have SUDO to enter The Root.SUDO commands have a STAR icon above the command icon.\n\n";

    private string level3NewInfo = "-Overriding previous new rule, now we're allowing non-SUDO commands to enter The Root, BUT, they must be scanned with the AntiVirus, which takes time to reveal if the file is infected or not.";

    private string currentFileText;

    private string currentInfoText;

    void Start()
    {
        currentInfoText = level1NormalInfo;
    }

    // Update is called once per frame
    //void Update()
    //{
    //   
    //}

    public void WriteNotification(cmdNotification notif)
    {
        string operation = "COPY";
        _texto.text = "" + textString + "<b>File:</b> " + notif.fileName + "\n\n";
        if (notif.op == ECommand.REPLACE)
        {
            _texto.text = "" + textString + "<b>File to be Replaced:</b> " + notif.fileName + "\n\n";
            _texto.text = _texto.text + "<b>File to Enter:</b> " + notif.secondaryFileName + "\n\n";
        }
        switch (notif.op)
        {
            case ECommand.ADD:
                operation = "ADD";
                break;
            case ECommand.DELETE:
                operation = "DELETE";
                break;
            case ECommand.REPLACE:
                operation = "REPLACE";
                break;
            default:
                break;
        }
        _texto.text = _texto.text + "<b>Operation:</b> " + operation;

        currentFileText = _texto.text;
    }

    public void WriteInfo(int level)
    {
        if (level == 2)
        {
            currentInfoText = currentInfoText + level2NewInfo;
        }
        if (level == 3)
        {
            currentInfoText = currentInfoText + level3NewInfo;
        }
    }

    public void SwitchToFileText()
    {
        _texto.text = currentFileText;
    }

    public void SwitchToInfoText()
    {
        _texto.text = currentInfoText;
    }

}
