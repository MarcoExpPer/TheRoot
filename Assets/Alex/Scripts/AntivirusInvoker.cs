using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AntivirusInvoker : MonoBehaviour
{
    // Start is called before the first frame update

    bool _calledScanning = false;

    [SerializeField]
    AntivirusController _antivirus;

    [SerializeField]
    Button _fileImage;

    [SerializeField]
    Sprite _evilFile;

    [SerializeField]
    public FileData _file;


    // Update is called once per frame
    void Update()
    {
        if (_calledScanning)
        {
            bool finished = _antivirus.ScanHasFinished();
            if (finished)
            {
                _calledScanning = false;
                bool hasVirus = _antivirus.GetFileHasVirus();
                if (hasVirus)
                {
                    _fileImage.image.sprite = _evilFile;
                }
            }
        }
    }

    public void CallAntivirus()
    {
        _antivirus.ScanForVirus(_file);
        _calledScanning = true;
    }
}

