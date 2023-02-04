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
    void Start()
    {
        
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
            _texto.text = _texto.text + "<b>File to Replace:</b> " + notif.secondaryFileName + "\n\n";
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
    }
}
