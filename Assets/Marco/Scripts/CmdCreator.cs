using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CmdCreator : MonoBehaviour
{
    float timeBetweenCmd = 3;
    float currentTime = 0;
    [SerializeField]
    bool isTimerActive = true;

    private float spaceFactor = 1;
    [SerializeField] private float probabilityConst = 1;

    private bool alreadyOneBadInstruction = false;

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

        Command cmdToAdd = new Command(ECommand.ADD, new cmdNotification("Archivo1", ECommand.ADD), new FileData("Archivo1", Color.black, 1), false);

        switch (gameManager.level)
        {
            case 1:
                //Nivel Tutorial
                //tareas predefinidas
                cmdToAdd = getLevelOneCommand();
                break;

            case 2:
                //Segundo nivel
                //Tareas aleatorias y añadimos sudo
                //cmdToAdd = getLevelTwoCommand();
                break;

            case 3:
                //Tercer nivel
                //Tareas aleatorias y añadimos un evento aleatorio
                break;

            default:

                break;
        }

        cmdQueue.addCommand(cmdToAdd);
        commandCount++;
    }


    Command getLevelOneCommand()
    {
        switch (commandCount)
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
                return new Command(ECommand.REPLACE, new cmdNotification("User", "User-32", ECommand.REPLACE),
               new FileData("User", Color.black, 1),
               new FileData("User-32", Color.black, 2), false);
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

    private ECommand GetCommandType() 
    {
        int commandType = 0;

        if (alreadyOneBadInstruction)
        {
            //No se puede hacer ni Delete, Replace ni Copy
            if (rootSizeMan.emptySlots == rootSizeMan.maxSlots)
            {
                commandType = 1;
            }
            //No puede ser ni Add ni Copy
            else if (rootSizeMan.emptySlots == 0)
            {
                commandType = Random.Range(2, 3);
            }
            else
            {
                commandType = GetAnyCommandTypeByHeuristic();
            }
        }
        else
        {
            commandType = GetAnyCommandTypeByHeuristic();
        }


        if (alreadyOneBadInstruction)
        {
            alreadyOneBadInstruction = false;
        }

        //Devolvemos el tipo de comando
        switch (commandType)
        {
            case 1:
                if (!alreadyOneBadInstruction && rootSizeMan.emptySlots == 0)
                {
                    alreadyOneBadInstruction = true;
                }
                return ECommand.ADD;
            case 2:
                if (!alreadyOneBadInstruction && rootSizeMan.emptySlots == rootSizeMan.maxSlots)
                {
                    alreadyOneBadInstruction = true;
                }
                return ECommand.DELETE;
            case 3: 
                if (!alreadyOneBadInstruction && rootSizeMan.emptySlots == rootSizeMan.maxSlots)
                {
                    alreadyOneBadInstruction = true;
                }
                return ECommand.REPLACE;
            case 4:
                if(!alreadyOneBadInstruction && (rootSizeMan.emptySlots == rootSizeMan.maxSlots || rootSizeMan.emptySlots == 0))
                {
                    alreadyOneBadInstruction = true;
                }
                return ECommand.COPY;
            default:
                Debug.Log("Aqui no deberias estar. CmdCreator::163 - switch con valor distinto de 1 a 4");
                return ECommand.ADD;
        }
    }

    private int GetAnyCommandTypeByHeuristic()
    {
        //Puede ser cualquiera
        //Se selecciona segun la heuristica
        //Si hay muchos huecos vacios es mas probable que aparezcan Adds y Copys
        //Si hay pocos huecos vacios es mas probable que aparezcan Deletes y Replaces
        float spaceFactor = rootSizeMan.emptySlots / rootSizeMan.maxSlots;

        List<int> posibleCommands = new List<int>() { 1, 2, 3, 4 };
        //Primero una fase donde se elimina una posibilidad basado en el factor de espacio
        //Si es muy alto, se eliminará una opcion de Delete o Replace o Copy
        //Y si es muy bajo se eliminará una opcion de Add o Copy
        //Si es un valor intermedio no se quitará ninguna opcion
        if (spaceFactor <= 0.35f)
        {
            if (Random.Range(1, 2) == 1)
            {
                posibleCommands.Remove(1);  //Quitamos Add
            }
            else
            {
                posibleCommands.Remove(4);  //Quitamos Copy
            }
        }
        else if (spaceFactor >= 0.65f)
        {
            posibleCommands.Remove(Random.Range(2, 4));
        }
        //Segunda fase, se elige entre las otras tres opciones con las mismas probabilidades
        return posibleCommands[Random.Range(0, posibleCommands.Count)];
    }

    private bool decideSudo()
    {
        //20% de no ser sudo
        if(Random.Range(1, 5) == 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}