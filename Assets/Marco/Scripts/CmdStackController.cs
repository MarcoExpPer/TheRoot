using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CmdStackController : MonoBehaviour
{
    [SerializeField]
    CmdQueue cmdQueue;

    RectTransform rt;

    int fontSize = 5;
    List<TextMeshPro> texts;
    // Start is called before the first frame update
    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        texts = new List<TextMeshPro>();

        if (rt != null)
        {
            int nRows = cmdQueue.queueMaxSize;
            float heightPerText = rt.rect.height / nRows;

            for (int i = 0; i < cmdQueue.queueMaxSize; i++)
            {
                //Create the text and add it to the canvas
                GameObject newGO = new GameObject("text " + i);
                newGO.transform.SetParent(transform);

                TextMeshPro myText = newGO.AddComponent<TextMeshPro>();
                myText.text = " (empty) ";
                myText.fontSize = fontSize;

                RectTransform textRt = myText.GetComponent<RectTransform>();

                //Set the new text position and size
                textRt.sizeDelta = new Vector2(100, heightPerText);

                textRt.anchorMin = new Vector2(0, 1);
                textRt.anchorMax = new Vector2(0, 1);
                textRt.pivot = new Vector2(0, 1);

                textRt.anchoredPosition = new Vector2(6, -heightPerText * i);


                ContentSizeFitter fitter = newGO.AddComponent<ContentSizeFitter>();
                fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;


                texts.Add(myText);
            }
        }

        if (cmdQueue == null)
        {
            Debug.Log("Cola vacia");
        }
        else
        {
            cmdQueue.OnQueueChanged += onQueueChanged;
        }
    }


    private void updateAllText()
    {
        for(int i = 0; i < cmdQueue.queueMaxSize; i++)
        {
            Command cmd = cmdQueue.getCommand(i);
            int index = cmdQueue.queueMaxSize - i - 1;

            if (cmd == null)
            {
                texts[index].text = " (empty) ";
            }
            else
            {
                texts[index].text = CmdQueue.printCommandInStack(cmd);
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
            texts[index].text = CmdQueue.printCommandInStack(cmd);
        }
    }

 
}
