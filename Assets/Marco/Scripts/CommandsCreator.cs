using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsCreator : MonoBehaviour
{
    [SerializeField]
    float timeBetweenCmd = 3;
    float currentTime = 0;
    [SerializeField]
    bool isTimerActive = true;

    [SerializeField]
    CommandsQueue cmdQueue;
    // Start is called before the first frame update
    void Start()
    {
        if(cmdQueue == null)
        {
            Debug.Log("Cola vacia");
        }
        else
        {
            cmdQueue.OnQueueFilled += onQueueEventCallback;
        }
    }

    void onQueueEventCallback()
    {
        isTimerActive = false;
        Debug.Log("QUEUE FILLED");

    }

    int counter = 0;

    // Update is called once per frame
    void Update()
    {
        if (isTimerActive)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= timeBetweenCmd)
            {
                currentTime = 0;
                counter += 1;
                Command cmdToadd = new Command("Cuadrado", ECommand.ADD, "File " + counter, false);
                cmdQueue.addCommand(cmdToadd);
            }
        }
    }
}
