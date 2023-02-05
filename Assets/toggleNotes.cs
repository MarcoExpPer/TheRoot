using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleNotes : MonoBehaviour
{

    [SerializeField]
    GameObject go;

    public void toggle()
    {

        if (go.activeSelf)
        {
            go.SetActive(true);
        }
        else
        {
            go.SetActive(false);
        }
    }


}
