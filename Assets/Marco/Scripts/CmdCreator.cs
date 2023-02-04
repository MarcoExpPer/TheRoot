using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdCreator : MonoBehaviour
{
    float timeBetweenCmd = 3;
    float currentTime = 0;
    [SerializeField]
    bool isTimerActive = true;

    private float spaceFactor = 1;
    [SerializeField] private float probabilityConst = 1;

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

        spaceFactor = rootSizeMan.emptySlots / rootSizeMan.maxSlots;

        float maxAddP = 0.25f + spaceFactor * probabilityConst;
        float maxRemP = maxAddP + 1/spaceFactor * probabilityConst;
        float maxModP = maxRemP + 1/spaceFactor * probabilityConst;
        float maxCopyP = maxModP + 0.25f;

        float p = Random.Range(0, maxCopyP);

        if (p >= 0 && p < maxAddP)
        {
            cmdToadd = getLevelOneCommand(0);
        }
        else if (p >= maxAddP && p < maxRemP)
        {
            cmdToadd = getLevelOneCommand(1);
        }
        else if (p >= maxRemP && p < maxModP)
        {
            cmdToadd = getLevelOneCommand(2);
        }
        else if (p >= maxModP && p <= maxCopyP)
        {
            cmdToadd = getLevelOneCommand(3);
        }

        cmdQueue.addCommand(cmdToadd);
        commandCount++;
    }


    Command getLevelOneCommand(int type)
    {
        string fileName = "";
        int fileSize = 0;

        switch (type)
        {
            case 0:
                return new Command(ECommand.ADD, new cmdNotification("System32", ECommand.ADD),
                    new FileData("System32", Color.black, 1), false);
            case 1:
                return new Command(ECommand.ADD, new cmdNotification("User", ECommand.ADD),
                    new FileData("User", Color.black, 3), false);
            case 2:
                return new Command(ECommand.COPY, new cmdNotification("System32", ECommand.COPY),
                    new FileData("System32", Color.black, 1),
                    new FileData("System32-copia", Color.black, 1), false);

            case 3:
                return new Command(ECommand.DELETE, new cmdNotification("System32", ECommand.DELETE),
                    new FileData("System32", Color.black, 1), false);
            default:
                gameManager.UpdateLevel(2);
                commandCount = 0;
                return new Command(ECommand.ADD, new cmdNotification("System32", ECommand.ADD),
                    new FileData("System32", Color.black, 1), false);


                /*
                 * MIGUEL
                 *
                case 0:
                    //crear tarea ADD
                    int n = Random.Range(1, 9);
                    fileName = "Archivo" + n.ToString();
                    return new Command(ECommand.ADD, new cmdNotification(fileName, ECommand.ADD), 
                        new FileData(fileName, Color.black, Random.Range(1, 3)), false);
                case 1:
                    //Creamos una tarea DELETE
                    List<int> p = new List<int>();
                    for (int i = 0; i < rootSizeMan.slots.Count; i++){

                        if (rootSizeMan.slots[i].slotData.state != ESlotState.EMPTY)
                        {
                            p.Add(i);
                        }
                    }

                    //si no hay ningun archivo pues se genera un nombre aleatorio y listo
                    if (p.Count != 0)
                    {
                        int a = p[Random.Range(0, p.Count)];
                        fileName = rootSizeMan.slots[a].slotData.fileData.nombre;
                        fileSize = rootSizeMan.slots[a].slotData.fileData.size;
                    }
                    else
                    {
                        int n2 = Random.Range(1, 9);
                        fileName = "Archivo" + n2.ToString();
                        fileSize = Random.Range(1, 3);
                    }

                    return new Command(ECommand.DELETE, new cmdNotification(fileName, ECommand.DELETE),
                        new FileData(fileName, Color.black, fileSize), false);
                case 2:
                    //Crear una tarea 
                    p = new List<int>();
                    for (int i = 0; i < rootSizeMan.slots.Count; i++)
                    {
                        if (rootSizeMan.slots[i].slotData.state != ESlotState.EMPTY)
                        {
                            p.Add(i);
                        }
                    }

                    //si no hay ningun archivo pues se genera un nombre aleatorio y listo
                    if (p.Count != 0)
                    {
                        int a = p[Random.Range(0, p.Count)];
                        fileName = rootSizeMan.slots[a].slotData.fileData.nombre;
                        fileSize = rootSizeMan.slots[a].slotData.fileData.size;
                    }
                    else
                    {
                        int n2 = Random.Range(1, 9);
                        fileName = "Archivo" + n2.ToString();
                        fileSize = Random.Range(1, 3);
                    }


                    return new Command(ECommand.REPLACE, new cmdNotification(fileName, ECommand.REPLACE), 
                        new FileData(fileName, Color.black, fileSize), 
                        new FileData("New" + fileName, Color.black, fileSize + Random.Range(-1, 1)), false);
                case 3:
                    //Crear una tarea COPY
                    p = new List<int>();
                    for (int i = 0; i < rootSizeMan.slots.Count; i++)
                    {
                        if (rootSizeMan.slots[i].slotData.state != ESlotState.EMPTY)
                        {
                            p.Add(i);
                        }
                    }
                    //si no hay ningun archivo pues se genera un nombre aleatorio y listo
                    if (p.Count != 0)
                    {
                        int a = p[Random.Range(0, p.Count)];
                        fileName = rootSizeMan.slots[a].slotData.fileData.nombre;
                        fileSize = rootSizeMan.slots[a].slotData.fileData.size;
                    }
                    else
                    {
                        int n2 = Random.Range(1, 9);
                        fileName = "Archivo" + n2.ToString();
                        fileSize = Random.Range(1, 3);
                    }


                    return new Command(ECommand.COPY, new cmdNotification(fileName, ECommand.COPY), 
                        new FileData(fileName, Color.black, fileSize),
                        new FileData(fileName + "Copy", Color.black, fileSize), false);
                default:

                    gameManager.level = 2;
                    commandCount = 0;
                    return new Command(ECommand.REPLACE, new cmdNotification("Archivo1 Copy", "Archivo3", ECommand.REPLACE), 
                        new FileData("Archivo1 Copy", Color.black, 1), 
                        new FileData("Archivo3", Color.black, 2), false);

                    Debug.Log("No deberias estar aqui");
                    return null;
                    */



                /*
                 * MARCO 
                 * 
                case 0:
                    return new Command(ECommand.ADD, new cmdNotification("System32", ECommand.ADD),
                        new FileData("System32", Color.black, 1), false);
                case 1:
                    return new Command(ECommand.ADD, new cmdNotification("User", ECommand.ADD),
                        new FileData("User", Color.black, 3), false);
                case 2:
                    return new Command(ECommand.COPY, new cmdNotification("System32", ECommand.COPY),
                        new FileData("System32", Color.black, 1),
                        new FileData("System32-copia", Color.black, 1), false);

                case 3:
                    return new Command(ECommand.DELETE, new cmdNotification("System32", ECommand.DELETE),
                        new FileData("System32", Color.black, 1), false);
                default:
                    gameManager.UpdateLevel(2);
                    commandCount = 0;



                    return new Command(ECommand.ADD, new cmdNotification("System32", ECommand.ADD),
                        new FileData("System32", Color.black, 1), false);
                case 1:
                    return new Command(ECommand.ADD, new cmdNotification("User", ECommand.ADD),
                        new FileData("User", Color.black, 3), false);
                case 2:
                    return new Command(ECommand.COPY, new cmdNotification("System32", ECommand.COPY),
                        new FileData("System32", Color.black, 1),
                        new FileData("System32-copia", Color.black, 1), false);

                case 3:
                    return new Command(ECommand.DELETE, new cmdNotification("System32", ECommand.DELETE),
                        new FileData("System32", Color.black, 1), false);
                default:
                    gameManager.UpdateLevel(2);
                    commandCount = 0;
                    return new Command(ECommand.REPLACE, new cmdNotification("User", "User-32", ECommand.REPLACE),
                        new FileData("User", Color.black, 1),
                        new FileData("User-32", Color.black, 2), false);

                */
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
