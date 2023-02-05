using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CloseNotify : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    TextMeshProUGUI text;


    public void close()
    {
        gameObject.SetActive(false);
    }

    public void open()
    {
        gameObject.SetActive(true);

    }
    public void updateText(string newText)
    {
        text.text = newText;
    }

}
