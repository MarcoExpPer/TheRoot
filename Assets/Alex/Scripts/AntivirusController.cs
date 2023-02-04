using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntivirusController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    RectTransform _imageTransform;
    [SerializeField]
    RectTransform _start;
    [SerializeField]
    RectTransform _end;

    FileData _fileToScan;

    private bool _isScanning;
    private bool _hasFinishedScanning = false;

    public float _speed = 100.0f;

    void Start()
    {
        _imageTransform.gameObject.SetActive(false);
        _hasFinishedScanning = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Distance(_imageTransform.localPosition, _end.localPosition));
        if (Vector3.Distance(_imageTransform.localPosition,_end.localPosition) >= 3.0f && _isScanning) 
        {
            _imageTransform.localPosition = _imageTransform.localPosition - new Vector3(0.0f, Time.deltaTime * _speed, 0.0f);
        }
        else
        {
            _isScanning = false;
            _hasFinishedScanning = true;
            _imageTransform.gameObject.SetActive(false);
        }

    }

    public void ScanForVirus(FileData fileToScan)
    {
        _isScanning = true;
        _fileToScan = fileToScan;
        _hasFinishedScanning = false;
        _imageTransform.gameObject.SetActive(true);
        //return fileToScan.isVirus;
    }

    public bool ScanHasFinished()
    {
        return _hasFinishedScanning;
    }

    public bool GetFileHasVirus()
    {
        _hasFinishedScanning = false;
        _imageTransform.localPosition = _start.localPosition;
        return _fileToScan.isVirus;
    }

}
