using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class CmdCreator : MonoBehaviour
{
    float timeBetweenCmd = 3;
    float currentTime = 0;
    [SerializeField]
    bool isTimerActive = true;

    [SerializeField] private float probabilityConst = 1;

    private string[] sust = new string[21] { "File", "Text", "Mail", "Engine", "Unreal", "Game", "Troyan", 
        "Music", "Disc", "Bug", "Key", "Unity", "Root", "Fort", "Burger",
        "Number", "Fish", "Appliance", "Book", "Property", "Memory"};
    private string[] adjs = new string[21] { "Importan", "Super", "Better", "Worst", "Bad", "Last", "New",
        "Ultimate", "Red", "Yellow", "White", "Clueless", "Pointless", "Ready",
        "Blue", "Detailed", "Royal", "Serious", "Old", "Chill", "Rooted"};
    private string[] exts = new string[21] { "exe", "txt", "png", "jpg", "rar", "zip", "docx",
        "gif", "mp4", "bat", "dll", "sys", "tar", "pdf",
        "ico", "obj", "json", "xlsx", "mp3", "img", "root"};

    private bool alreadyOneBadInstruction = false;

    private int numCommandToNextLevel = 4;

    [SerializeField]
    RootSizeManager rootSizeMan;

    [SerializeField]
    CmdQueue cmdQueue;

    [SerializeField]
    GameManager gameManager;

    public int commandCount = 0;

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

        for (int i = 0; i < 40; i++)
        {
            Debug.Log(GenerateName());
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
                cmdToAdd = getLevelTwoCommand();
                break;

            case 3:
                //Tercer nivel
                //Tareas aleatorias y añadimos un evento aleatorio
                cmdToAdd = getLevelThreeCommand();
                break;

            default:
                //SEGUIMOS BOTANDO EVENTOS LEVEL 3, PERO CON MENOS TIEMPO DE VISION? NO SE
                cmdToAdd = getLevelThreeCommand();
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
                new FileData("System32", GenerateColor(), 1), false);
            case 1:
                return new Command(ECommand.ADD, new cmdNotification("User", ECommand.ADD),
                new FileData("User", GenerateColor(), 3), false);
            case 2:
                return new Command(ECommand.COPY, new cmdNotification("System32", ECommand.COPY),
                new FileData("System32", GenerateColor(), 1),
                new FileData("System32-copia", GenerateColor(), 1), false);
            case 3:
                return new Command(ECommand.DELETE, new cmdNotification("System32", ECommand.DELETE),
                new FileData("System32", GenerateColor(), 1), false);
            default:
                gameManager.UpdateLevel(2);
                numCommandToNextLevel = Random.Range(5, 11);
                commandCount = 0;
                return new Command(ECommand.REPLACE, new cmdNotification("User", "User-32", ECommand.REPLACE),
               new FileData("User", GenerateColor(), 1, "User-32"),
               new FileData("User-32", GenerateColor(), 2), false);
        }

    }

    private Command getLevelTwoCommand()
    {
        if (numCommandToNextLevel > 0)
        {
            ECommand commandType = GetCommandType();
            string fileName = "";
            Color fileColor = Color.red;
            int fileSize = 1;
            bool fileSudo = false;

            string fileName2 = "";
            Color fileColor2 = Color.red;
            int fileSize2 = 1;
            bool fileSudo2 = false;

            FileData dataToDeleteOrReplaceOrCopy;

            numCommandToNextLevel--;

            bool correctName = decideCorrectness();
            bool correctOp = decideCorrectness();

            if (numCommandToNextLevel == 0)
            {
                gameManager.UpdateLevel(3);
            }

            if (commandType == ECommand.ADD)
            {
                fileName = GenerateName();
                fileColor = GenerateColor();
                fileSize = GenerateSize();
                fileSudo = decideSudo();
                if(!correctName)
                {
                    return new Command(ECommand.ADD, new cmdNotification(GenerateName(), ECommand.ADD),
                    new FileData(fileName, fileColor, fileSize), fileSudo);
                }
                if (!correctOp)
                {
                    return new Command(ECommand.ADD, new cmdNotification(fileName, ECommand.DELETE),
                    new FileData(fileName, fileColor, fileSize), fileSudo);
                }
                return new Command(ECommand.ADD, new cmdNotification(fileName, ECommand.ADD),
                new FileData(fileName, fileColor, fileSize), fileSudo);
            }
            else if(commandType == ECommand.DELETE)
            {
                List<int> p = new List<int>();
                for (int i = 0; i < rootSizeMan.slots.Count; i++)
                {
                    if (rootSizeMan.slots[i].slotData.state == ESlotState.FILLED)
                    {
                        p.Add(i);
                    }
                }

                //si no hay ningun archivo pues se genera un nombre aleatorio y listo
                if (p.Count != 0)
                {
                    int a = p[Random.Range(0, p.Count)];
                    fileName = rootSizeMan.slots[a].slotData.fileData.nombre;
                    //fileColor = rootSizeMan.slots[a].slotData.fileData.origen;
                    //fileSize = rootSizeMan.slots[a].slotData.fileData.size;
                    fileSudo = decideSudo();

                    dataToDeleteOrReplaceOrCopy = new FileData(rootSizeMan.slots[a].slotData.fileData);
                }
                else
                {
                    fileName = GenerateName();
                    fileColor = GenerateColor();
                    fileSize = GenerateSize();
                    fileSudo = decideSudo();

                    dataToDeleteOrReplaceOrCopy = new FileData(fileName, fileColor, fileSize);
                }
                if (!correctName)
                {
                    return new Command(ECommand.DELETE, new cmdNotification(GenerateName(), ECommand.DELETE),
                    dataToDeleteOrReplaceOrCopy, fileSudo);
                }
                if (!correctOp)
                {
                    return new Command(ECommand.DELETE, new cmdNotification(fileName, ECommand.ADD),
                    dataToDeleteOrReplaceOrCopy, fileSudo);
                }
                return new Command(ECommand.DELETE, new cmdNotification(fileName, ECommand.DELETE),
                dataToDeleteOrReplaceOrCopy, fileSudo);
            }
            else if (commandType == ECommand.REPLACE)
            {
                List<int> p = new List<int>();
                for (int i = 0; i < rootSizeMan.slots.Count; i++)
                {
                    if (rootSizeMan.slots[i].slotData.state == ESlotState.FILLED)
                    {
                        p.Add(i);
                    }
                }
                //si no hay ningun archivo pues se genera un nombre aleatorio y listo
                if (p.Count != 0)
                {
                    int a = p[Random.Range(0, p.Count)];
                    fileName = rootSizeMan.slots[a].slotData.fileData.nombre;
                    //fileColor = rootSizeMan.slots[a].slotData.fileData.origen;
                    //fileSize = rootSizeMan.slots[a].slotData.fileData.size;
                    fileSudo = decideSudo();

                    dataToDeleteOrReplaceOrCopy = new FileData(rootSizeMan.slots[a].slotData.fileData);
                }
                else
                {
                    fileName = GenerateName();
                    fileColor = GenerateColor();
                    fileSize = GenerateSize();
                    fileSudo = decideSudo();

                    dataToDeleteOrReplaceOrCopy = new FileData(fileName, fileColor, fileSize);
                }

                fileName2 = GenerateName();
                fileColor2 = GenerateColor();
                fileSize2 = GenerateSize();
                fileSudo2 = fileSudo;
                if (!correctName)
                {
                    return new Command(ECommand.REPLACE,
                    new cmdNotification(GenerateName(), fileName2, ECommand.REPLACE),
                    dataToDeleteOrReplaceOrCopy,
                    new FileData(fileName2, fileColor2, fileSize2,fileName,false), fileSudo2);
                }
                if (!correctOp)
                {
                    return new Command(ECommand.REPLACE,
                    new cmdNotification(fileName, fileName2, ECommand.COPY),
                    dataToDeleteOrReplaceOrCopy,
                    new FileData(fileName2, fileColor2, fileSize2,fileName,false), fileSudo2);
                }
                return new Command(ECommand.REPLACE, 
                new cmdNotification(fileName,fileName2, ECommand.REPLACE),
                dataToDeleteOrReplaceOrCopy,
                new FileData(fileName2, fileColor2, fileSize2,fileName,false), fileSudo2);
            }
            else if (commandType == ECommand.COPY)
            {
                List<int> p= new List<int>();
                for (int i = 0; i < rootSizeMan.slots.Count; i++)
                {
                    if (rootSizeMan.slots[i].slotData.state == ESlotState.FILLED)
                    {
                        p.Add(i);
                    }
                }
                //si no hay ningun archivo pues se genera un nombre aleatorio y listo
                if (p.Count != 0)
                {
                    int a = p[Random.Range(0, p.Count)];
                    fileName = rootSizeMan.slots[a].slotData.fileData.nombre;
                    //fileColor = rootSizeMan.slots[a].slotData.fileData.origen;
                    //fileSize = rootSizeMan.slots[a].slotData.fileData.size;
                    fileSudo = decideSudo();

                    dataToDeleteOrReplaceOrCopy = new FileData(rootSizeMan.slots[a].slotData.fileData);

                }
                else
                {
                    fileName = GenerateName();
                    fileColor = GenerateColor();
                    fileSize = GenerateSize();
                    fileSudo = decideSudo();

                    dataToDeleteOrReplaceOrCopy = new FileData(fileName, fileColor, fileSize);
                }

                if (!correctName)
                {
                    return new Command(ECommand.COPY, new cmdNotification(GenerateName(), ECommand.COPY),
                    dataToDeleteOrReplaceOrCopy,
                    new FileData(dataToDeleteOrReplaceOrCopy.nombre + "Copy", fileColor, fileSize), fileSudo);
                }
                if (!correctOp)
                {
                    return new Command(ECommand.COPY, new cmdNotification(fileName, ECommand.ADD),
                    dataToDeleteOrReplaceOrCopy,
                    new FileData(dataToDeleteOrReplaceOrCopy.nombre + "Copy", fileColor, fileSize), fileSudo);
                }
                return new Command(ECommand.COPY, new cmdNotification(fileName, ECommand.COPY),
                    dataToDeleteOrReplaceOrCopy,
                    new FileData(dataToDeleteOrReplaceOrCopy.nombre + "Copy", fileColor, fileSize), fileSudo);
            }
        }
        return null;
    }

    private Command getLevelThreeCommand()
    {
        ECommand commandType = GetCommandType();
        string fileName = "";
        Color fileColor = Color.red;
        int fileSize = 1;
        bool fileSudo = false;
        bool isVirus = false;

        string fileName2 = "";
        Color fileColor2 = Color.red;
        int fileSize2 = 1;
        bool fileSudo2 = false;
        bool isVirus2 = false;

        FileData dataToDeleteOrReplaceOrCopy;

        numCommandToNextLevel--;

        bool correctName = decideCorrectness();
        bool correctOp = decideCorrectness();

        if (commandType == ECommand.ADD)
        {
            fileName = GenerateName();
            fileColor = GenerateColor();
            fileSize = GenerateSize();
            fileSudo = decideSudoLevel3();
            if(!fileSudo)
            {
                isVirus = decideVirus();
            }
            if (!correctName)
            {
                return new Command(ECommand.ADD, new cmdNotification(GenerateName(), ECommand.ADD),
                new FileData(fileName, fileColor, fileSize, isVirus), fileSudo);
            }
            if (!correctOp)
            {
                return new Command(ECommand.ADD, new cmdNotification(fileName, ECommand.DELETE),
                new FileData(fileName, fileColor, fileSize, isVirus), fileSudo);
            }
            return new Command(ECommand.ADD, new cmdNotification(fileName, ECommand.ADD),
            new FileData(fileName, fileColor, fileSize, isVirus), fileSudo);
        }
        else if (commandType == ECommand.DELETE)
        {
            List<int> p = new List<int>();
            for (int i = 0; i < rootSizeMan.slots.Count; i++)
            {
                if (rootSizeMan.slots[i].slotData.state == ESlotState.FILLED)
                {
                    p.Add(i);
                }
            }

            //si no hay ningun archivo pues se genera un nombre aleatorio y listo
            if (p.Count != 0)
            {
                int a = p[Random.Range(0, p.Count)];
                fileName = rootSizeMan.slots[a].slotData.fileData.nombre;
                //fileColor = rootSizeMan.slots[a].slotData.fileData.origen;
                //fileSize = rootSizeMan.slots[a].slotData.fileData.size;
                fileSudo = decideSudoLevel3();

                dataToDeleteOrReplaceOrCopy = new FileData(rootSizeMan.slots[a].slotData.fileData);
            }
            else
            {
                fileName = GenerateName();
                fileColor = GenerateColor();
                fileSize = GenerateSize();
                fileSudo = decideSudoLevel3();
                if (!fileSudo)
                {
                    isVirus = decideVirus();
                }

                dataToDeleteOrReplaceOrCopy = new FileData(fileName, fileColor, fileSize, isVirus);
            }
            if (!correctName)
            {
                return new Command(ECommand.DELETE, new cmdNotification(GenerateName(), ECommand.DELETE),
                dataToDeleteOrReplaceOrCopy, fileSudo);
            }
            if (!correctOp)
            {
                return new Command(ECommand.DELETE, new cmdNotification(fileName, ECommand.ADD),
                dataToDeleteOrReplaceOrCopy, fileSudo);
            }
            return new Command(ECommand.DELETE, new cmdNotification(fileName, ECommand.DELETE),
            dataToDeleteOrReplaceOrCopy, fileSudo);
        }
        else if (commandType == ECommand.REPLACE)
        {
            List<int> p = new List<int>();
            for (int i = 0; i < rootSizeMan.slots.Count; i++)
            {
                if (rootSizeMan.slots[i].slotData.state == ESlotState.FILLED)
                {
                    p.Add(i);
                }
            }
            //si no hay ningun archivo pues se genera un nombre aleatorio y listo
            if (p.Count != 0)
            {
                int a = p[Random.Range(0, p.Count)];
                fileName = rootSizeMan.slots[a].slotData.fileData.nombre;
                //fileColor = rootSizeMan.slots[a].slotData.fileData.origen;
                //fileSize = rootSizeMan.slots[a].slotData.fileData.size;
                fileSudo = decideSudoLevel3();

                dataToDeleteOrReplaceOrCopy = new FileData(rootSizeMan.slots[a].slotData.fileData);
            }
            else
            {
                fileName = GenerateName();
                fileColor = GenerateColor();
                fileSize = GenerateSize();
                fileSudo = decideSudoLevel3();

                dataToDeleteOrReplaceOrCopy = new FileData(fileName, fileColor, fileSize);
            }

            fileName2 = GenerateName();
            fileColor2 = GenerateColor();
            fileSize2 = GenerateSize();
            fileSudo2 = fileSudo;
            if(!fileSudo2)
            {
                    isVirus = decideVirus();
            }
            if (!correctName)
            {
                return new Command(ECommand.REPLACE,
                new cmdNotification(GenerateName(), fileName2, ECommand.REPLACE),
                dataToDeleteOrReplaceOrCopy,
                new FileData(fileName2, fileColor2, fileSize2, dataToDeleteOrReplaceOrCopy.nombre, isVirus), fileSudo2);
            }
            if (!correctOp)
            {
                return new Command(ECommand.REPLACE,
                new cmdNotification(fileName, fileName2, ECommand.COPY),
                dataToDeleteOrReplaceOrCopy,
                new FileData(fileName2, fileColor2, fileSize2, dataToDeleteOrReplaceOrCopy.nombre, isVirus), fileSudo2);
            }
            return new Command(ECommand.REPLACE,
            new cmdNotification(fileName, fileName2, ECommand.REPLACE),
            dataToDeleteOrReplaceOrCopy,
            new FileData(fileName2, fileColor2, fileSize2, dataToDeleteOrReplaceOrCopy.nombre, isVirus), fileSudo2);
        }
        else //copy command
        {
            List<int> p = new List<int>();
            for (int i = 0; i < rootSizeMan.slots.Count; i++)
            {
                if (rootSizeMan.slots[i].slotData.state == ESlotState.FILLED)
                {
                    p.Add(i);
                }
            }
            //si no hay ningun archivo pues se genera un nombre aleatorio y listo
            if (p.Count != 0)
            {
                int a = p[Random.Range(0, p.Count)];
                fileName = rootSizeMan.slots[a].slotData.fileData.nombre;
                //fileColor = rootSizeMan.slots[a].slotData.fileData.origen;
                //fileSize = rootSizeMan.slots[a].slotData.fileData.size;
                fileSudo = decideSudoLevel3();

                dataToDeleteOrReplaceOrCopy = new FileData(rootSizeMan.slots[a].slotData.fileData);
            }
            else
            {
                fileName = GenerateName();
                fileColor = GenerateColor();
                fileSize = GenerateSize();
                fileSudo = decideSudoLevel3();

                dataToDeleteOrReplaceOrCopy = new FileData(fileName, fileColor, fileSize);
            }

            if (!correctName)
            {
                return new Command(ECommand.COPY, new cmdNotification(GenerateName(), ECommand.COPY),
                dataToDeleteOrReplaceOrCopy,
                new FileData(dataToDeleteOrReplaceOrCopy.nombre + "Copy", fileColor, fileSize), fileSudo);
            }
            if (!correctOp)
            {
                return new Command(ECommand.COPY, new cmdNotification(fileName, ECommand.ADD),
                dataToDeleteOrReplaceOrCopy,
                new FileData(dataToDeleteOrReplaceOrCopy.nombre + "Copy", fileColor, fileSize), fileSudo);
            }
            return new Command(ECommand.COPY, new cmdNotification(fileName, ECommand.COPY),
                dataToDeleteOrReplaceOrCopy,
                new FileData(dataToDeleteOrReplaceOrCopy.nombre + "Copy", fileColor, fileSize), fileSudo);
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

        int emptySlots = 0;
        int uselessSlots = 0;
        for (int i = 0; i < rootSizeMan.slots.Count; i++)
        {
            if(rootSizeMan.slots[i].slotData == null || rootSizeMan.slots[i].slotData.state == ESlotState.EMPTY)
            {
                emptySlots++;
            }
            else if(rootSizeMan.slots[i].slotData.state == ESlotState.BLOCKED 
                || rootSizeMan.slots[i].slotData.state == ESlotState.VIRUS)
            {
                uselessSlots++;
            }
        }

        if (alreadyOneBadInstruction)
        {
            //No se puede hacer ni Delete, Replace ni Copy
            if (emptySlots == rootSizeMan.maxSlots)
            {
                commandType = 1;
            }
            //No puede ser ni Add ni Copy
            else if (emptySlots == 0)
            {
                commandType = Random.Range(2, 3);
            }
            else
            {
                commandType = GetAnyCommandTypeByHeuristic(emptySlots, uselessSlots);
            }
        }
        else
        {
            commandType = GetAnyCommandTypeByHeuristic(emptySlots, uselessSlots);
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

    private int GetAnyCommandTypeByHeuristic(int emptySlots, int uselessSlots)
    {
        //Puede ser cualquiera
        //Se selecciona segun la heuristica
        //Si hay muchos huecos vacios es mas probable que aparezcan Adds y Copys
        //Si hay pocos huecos vacios es mas probable que aparezcan Deletes y Replaces
        float spaceFactor = emptySlots / rootSizeMan.maxSlots - uselessSlots;

        List<int> posibleCommands = new List<int>() { 1, 2, 3, 4 };
        //Primero una fase donde se elimina una posibilidad basado en el factor de espacio
        //Si es muy alto, se eliminará una opcion de Delete o Replace o Copy
        //Y si es muy bajo se eliminará una opcion de Add o Copy
        //Si es un valor intermedio no se quitará ninguna opcion
        if (spaceFactor <= 0.30f)
        {
            if (Random.Range(1, 3) == 1)
            {
                posibleCommands.Remove(1);  //Quitamos Add
            }
            else
            {
                posibleCommands.Remove(4);  //Quitamos Copy
            }
        }
        else if (spaceFactor >= 0.70f)
        {
            posibleCommands.Remove(Random.Range(2, 5));
        }
        //Segunda fase, se elige entre las otras tres opciones con las mismas probabilidades

        return posibleCommands[Random.Range(0, posibleCommands.Count)];
    }

    private bool decideSudo()
    {
        //20% de no ser sudo
        if(Random.Range(1, 6) == 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool decideSudoLevel3()
    {
        //33.33% de no ser sudo
        if (Random.Range(1, 4) == 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool decideVirus()
    {
        //33.33% de tener virus.. por ahora
        if (Random.Range(1, 3) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool decideCorrectness()
    {
        //10% de que no coincidan
        if (Random.Range(1, 11) == 2)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    private string GenerateName()
    {
        string name = "";
        //20% de tener "_" entre cada palabra
        bool hasGuionBajo = (Random.Range(1, 6) == 1);

        //33% de tener un adjetivo al principio
        if (Random.Range(1, 4) == 1)
        {
            name += adjs[Random.Range(0, adjs.Length)];
            if (hasGuionBajo)
            {
                name += "_";
            }
        }
        //Añadimos un sustantivo
        name += sust[Random.Range(0, sust.Length)];
        //50% de añadir otro adjetivo
        if (Random.Range(1, 3) == 1)
        {
            if (hasGuionBajo)
            {
                name += "_";
            }
            name += adjs[Random.Range(0, adjs.Length)];
        }
        //Añadimos la extension
        name += "." + exts[Random.Range(0, exts.Length)];

        return name;
    }

    private int GenerateSize()
    {
        List<int> sizes = new List<int>() { 1, 1, 1, 2, 2, 3 };
        return sizes[Random.Range(0, sizes.Count)];
    }

    private Color GenerateColor()
    {
        List<Color> colors = new List<Color>() { Color.blue, Color.green, Color.red, Color.yellow };
        return colors[Random.Range(0, colors.Count)];

    }

}