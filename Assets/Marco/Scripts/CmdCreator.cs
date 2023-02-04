using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdCreator : MonoBehaviour
{
    float timeBetweenCmd = 3;
    float currentTime = 0;
    [SerializeField]
    bool isTimerActive = true;

    [SerializeField]
    RootSizeManager rootSizeMan;

    [SerializeField]
    CmdQueue cmdQueue;

    [SerializeField]
    GameManager gameManager;

    int commandCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        timeBetweenCmd = gameManager.levelToTimeBetweenCommands.GetValueOrDefault(gameManager.level);

        if (cmdQueue == null)
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

    // Update is called once per frame
    void Update()
    {
        if (isTimerActive)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= timeBetweenCmd)
            {
                
                addCommandToQueue();
                
            }
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            cmdQueue.popCommand();
        }
    }


    void addCommandToQueue()
    {
        currentTime = 0;
        timeBetweenCmd = gameManager.levelToTimeBetweenCommands.GetValueOrDefault(gameManager.level);

        Command cmdToadd = new Command(ECommand.ADD, new cmdNotification("Archivo1", ECommand.ADD), new FileData("Archivo1", Color.black, 1), false);

        switch (gameManager.level)
        {
            case 1:
                cmdToadd = getLevelOneCommand();
                break;

            case 2:
                break;

            case 3:

                break;

            default:

                break;
        }
        
        cmdQueue.addCommand(cmdToadd);
        commandCount++;
    }


    Command getLevelOneCommand()
    {
        switch (commandCount)
        {
            case 0:
                return new Command(ECommand.ADD, new cmdNotification("Archivo1", ECommand.ADD), 
                    new FileData("Archivo1", Color.black,1 ), false);
            case 1:
                return new Command(ECommand.COPY, new cmdNotification("Archivo1", ECommand.COPY), 
                    new FileData("Archivo1", Color.black, 1), 
                    new FileData("Archivo1 Copy", Color.black, 1), false);
            case 2:
                return new Command(ECommand.ADD, new cmdNotification("Archivo2", ECommand.ADD), 
                    new FileData("Archivo2", Color.black, 3), false);
            case 3:
                return new Command(ECommand.DELETE, new cmdNotification("Archivo1", ECommand.DELETE), 
                    new FileData("Archivo1", Color.black, 1), false);
            default:
                gameManager.level = 2;
                commandCount = 0;
                return new Command(ECommand.REPLACE, new cmdNotification("Archivo1 Copy", "Archivo3", ECommand.REPLACE), 
                    new FileData("Archivo1 Copy", Color.black, 1), 
                    new FileData("Archivo3", Color.black, 2), false);

        }
       
    }

    public Command getNextCommand()
    {
        Command cmd = cmdQueue.popCommand();
       
        if (cmd == null)
        {
            addCommandToQueue();
            cmd = cmdQueue.popCommand();
        }

        return cmd;
    }

}
