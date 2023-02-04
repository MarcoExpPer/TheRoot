using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootFileBtn : MonoBehaviour
{
    [SerializeField]
    public SlotData slotData;

    [SerializeField]
    public Sprite FilledSprite;
    [SerializeField]
    public Sprite EmptySprite;
    [SerializeField]
    public Sprite VirusSprite;
    [SerializeField]
    public Sprite BlockedSprite;

    [SerializeField]
    public Image image;


    // Start is called before the first frame update
    void Start()
    {
        image.sprite = EmptySprite;
        slotData = new SlotData();
    }

    public void changeSlotData(SlotData newData)
    {
        switch (newData.state)
        {
            case ESlotState.BLOCKED:
                image.sprite = BlockedSprite;
                break;
            case ESlotState.EMPTY:
                image.sprite = EmptySprite;
                break;
            case ESlotState.FILLED:
                image.sprite = FilledSprite;
                break;
            case ESlotState.VIRUS:
                image.sprite = VirusSprite;
                break;
        }

        slotData = newData;
    }

    public FileData getSlotFileData()
    {
        return slotData.fileData;
    }

    public void testOnClick()
    {
        if(slotData == null || slotData.fileData == null)
        {
            Debug.Log("no data");
        }
        else
        {
            Debug.Log(slotData.fileData.nombre);
        }
        
    }
}