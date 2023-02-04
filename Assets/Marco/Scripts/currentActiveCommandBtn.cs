using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class currentActiveCommandBtn : MonoBehaviour
{
    
    Command command;

    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    Button _fileButton;

    [SerializeField]
    Image _commandImage;

    [SerializeField]
    GameObject _sudoImage;

    [SerializeField]
    Sprite[] _fileSpritesToUse;

    [SerializeField]
    Sprite[] _commandSpritesToUse;
    // Start is called before the first frame update
    void Start()
    {
        _sudoImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateCommand(Command cmd)
    {
        command = cmd;
        _fileButton.image.sprite = _fileSpritesToUse[0];

        _commandImage.sprite = _commandSpritesToUse[(int)(cmd.commandType)];
        _commandImage.SetNativeSize();

        //text.text = cmd.commandType + " " + cmd.file.nombre;
    }

}
