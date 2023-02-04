using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESlotState
{
    EMPTY,
    FILLED,
    VIRUS,
    BLOCKED
}

public class SlotData
{
    public ESlotState state = ESlotState.EMPTY;
    public FileData fileData;

    public SlotData() { }
    public SlotData(FileData filed, ESlotState st)
    {
        state = st;
        fileData = filed;
    }
}

public class RootSizeManager : MonoBehaviour
{
    public int maxSlots = 8;
    int emptySlots;

    [SerializeField]
    public List<RootFileBtn> slots;

    //Cuando hace overflow
    public delegate void RootOverflow();
    public event RootOverflow OnRootOverflow;

    //Cuando elimina algo y no hay nada que eliminar
    public delegate void RootWrongDelete();
    public event RootWrongDelete OnWrongDelete;


    //Siemrpe que el estado del root cambia
    public delegate void RootChanged();
    public event RootChanged OnRootChanged;

    // Start is called before the first frame update
    void Start()
    {
        emptySlots = maxSlots;
    }

    int count = 0;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.B))
        {
            FileData newFile = new FileData("nombre " + count, Color.red, 1);

            addFile(newFile);



            count++;
        }
    }


    void addFile(FileData fileToAdd)
    {
        int sizeToAdd = fileToAdd.size;

        if (sizeToAdd > emptySlots)
        {
            if (OnRootOverflow != null)
                OnRootOverflow.Invoke();

            for (int i = 0; i < maxSlots && sizeToAdd > 0; i++)
            {
                if (slots[i].slotData.state == ESlotState.FILLED)
                {
                    slots[i].changeSlotData(new SlotData(null, ESlotState.BLOCKED));
                    sizeToAdd -= 1;
                    emptySlots -= 1;
                }
            }

        }
        else
        {
            for (int i = 0; i < maxSlots && sizeToAdd > 0; i++)
            {
                if (slots[i].slotData.state == ESlotState.EMPTY)
                {
                    slots[i].changeSlotData(new SlotData(fileToAdd, ESlotState.FILLED));
                    sizeToAdd -= 1;
                    emptySlots -= 1;
                }
            }
        }

        if (OnRootChanged != null)
            OnRootChanged.Invoke();
    }

    void deleteFIle(string fileName)
    {
        bool hasDeleted = false;
        for (int i = 0; i < maxSlots; i++)
        {
            if (slots[i].slotData.fileData != null && slots[i].slotData.fileData.nombre == fileName)
            {
                hasDeleted = true;
                slots[i].changeSlotData(new SlotData(null, ESlotState.EMPTY));
                emptySlots += 1;
            }
        }

        if (!hasDeleted)
        {
            if (OnWrongDelete != null)
                OnWrongDelete.Invoke();

            for (int i = 0; i < maxSlots; i++)
            {
                Debug.Log(i);
                if (slots[i].slotData.state == ESlotState.FILLED)
                {
                    slots[i].changeSlotData(new SlotData(null, ESlotState.BLOCKED));
                    emptySlots -= 1;
                }
            }
        }

        if (OnRootChanged != null)
            OnRootChanged.Invoke();

    }
}
