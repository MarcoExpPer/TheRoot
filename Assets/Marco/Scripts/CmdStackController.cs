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

    public FMOD.Studio.EventInstance instace;

    public FMOD.Studio.EventInstance instace2;

    public FMOD.Studio.EventInstance instace3;
    RectTransform rt;

    int fontSize = 5;
    List<TextMeshPro> texts;

    int _playedMusic = 0; //0 initial, 1 music speeder, 2 musicspeeder 2
    // Start is called before the first frame update
    void Start()
    {
        foreach (Image img in _operationImages)
        {
            img.gameObject.SetActive(false);
        }

        _stackStatusImage.sprite = _stackStatusImageFiles[0];
        instace = FMODUnity.RuntimeManager.CreateInstance("event:/Maintheme");
        instace2 = FMODUnity.RuntimeManager.CreateInstance("event:/Maintheme1_2");
        instace3 = FMODUnity.RuntimeManager.CreateInstance("event:/Mainthemev1_4");
        instace.start();

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
                //Debug.Log(cmd.commandType);
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
            //Debug.Log(cmd.commandType);
            index = cmdQueue.queueMaxSize - index - 1;

            _colorOperations[index].color = _operationColors[(int)cmd.commandType];
            _operationImages[index].sprite = _spritesToUse[(int)cmd.commandType];
            _operationImages[index].gameObject.SetActive(true);

            //texts[index].text = CmdQueue.printCommandInStack(cmd);
        }
        if(cmdQueue.currentQueueSize<=1)
        {
            if (_playedMusic != 0)
            {
                instace2.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                instace3.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                instace.start();
                _playedMusic = 0;
            }
            _stackStatusImage.sprite = _stackStatusImageFiles[0];
        }
        else if (cmdQueue.currentQueueSize <= 3)
        {
            if(_playedMusic != 1)
            {
                instace.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                instace3.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                instace2.start();
                _playedMusic = 1;
            }
            _stackStatusImage.sprite = _stackStatusImageFiles[1];
        }
        else
        {
            if (_playedMusic != 2)
            {
                instace.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                instace2.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                instace3.start();
                _playedMusic = 2;
            }
            _stackStatusImage.sprite = _stackStatusImageFiles[2];
        }
    }

 
}
