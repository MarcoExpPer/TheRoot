using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CmdStackController : MonoBehaviour
{
    [SerializeField]
    CmdQueue cmdQueue;

    [SerializeField]
    Image[] _colorOperations;

    [SerializeField]
    Image[] _operationImages;

    [SerializeField]
    Sprite[] _spritesToUse;

    [SerializeField]
    Color[] _operationColors;

    [SerializeField]
    Image _stackStatusImage;

    [SerializeField]
    Sprite[] _stackStatusImageFiles;

    RectTransform rt;

    int fontSize = 5;
    List<TextMeshPro> texts;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Image img in _operationImages)
        {
            img.gameObject.SetActive(false);
        }

        cmdQueue.OnQueueChanged += onQueueChanged;
        _stackStatusImage.sprite = _stackStatusImageFiles[0];
    }


    private void updateAllText()
    {
        for(int i = 0; i < cmdQueue.queueMaxSize; i++)
        {
            Command cmd = cmdQueue.getCommand(i);
            int index = cmdQueue.queueMaxSize - i - 1;

            if (cmd == null)
            {
                _operationImages[index].gameObject.SetActive(false);
                _colorOperations[index].color = _operationColors[4];
            }
            else
            {
                _colorOperations[index].color = _operationColors[(int)cmd.commandType];
                _operationImages[index].sprite = _spritesToUse[(int)cmd.commandType];
                _operationImages[index].gameObject.SetActive(true);
            }
        }
    }

    void onQueueChanged(int index, EQueueChangedAction action, Command cmd)
    {
        if (action == EQueueChangedAction.DEL_CMD)
        {
            updateAllText();
        }
        else
        {
            index = cmdQueue.queueMaxSize - index - 1;

            _colorOperations[index].color = _operationColors[(int)cmd.commandType];
            _operationImages[index].sprite = _spritesToUse[(int)cmd.commandType];
            _operationImages[index].gameObject.SetActive(true);

            //texts[index].text = CmdQueue.printCommandInStack(cmd);
        }
        if(cmdQueue.currentQueueSize<=1)
        {
            _stackStatusImage.sprite = _stackStatusImageFiles[0];
        }
        else if (cmdQueue.currentQueueSize <= 3)
        {
            _stackStatusImage.sprite = _stackStatusImageFiles[1];
        }
        else
        {
            _stackStatusImage.sprite = _stackStatusImageFiles[2];
        }
    }

 
}
