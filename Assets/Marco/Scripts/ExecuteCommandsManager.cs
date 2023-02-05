using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteCommandsManager : MonoBehaviour
{
    Command cmdToExecute;

    [SerializeField]
    CmdCreator cmdCreator;

    [SerializeField]
    RootSizeManager rootSizemanager;

    [SerializeField]
    currentActiveCommandBtn activeBtn;

    [SerializeField]
    GameManager gameMan;

    [SerializeField]
    activateDisactivate fileInMiddleOfTheScreen;

    AudioSource audioSource;
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void confirmCommand(bool execute)
    {
        Debug.Log("Execute Command: " + execute + " " + cmdToExecute.commandType + " " + cmdToExecute.isSudo);
        bool correct = false;

        if (checkTam())
        {
            if (cmdToExecute.commandType == cmdToExecute.notification.op &&
                cmdToExecute.file.nombre == cmdToExecute.notification.fileName)
            {
                if (checkEvents())
                {
                    if (!gameMan.lookForSudo)
                    {
                        if (checkOp(execute))
                        {
                            //es correcto ejecutar la operacion, ver si se ejecuto
                            correct = execute;
                            if (!correct)
                            {
                                addBlocked(cmdToExecute.file);
                            }
                        }
                        else
                        {
                            if (execute)
                            {
                                addVirus(cmdToExecute.file);
                            }
                            else
                            {
                                //Todo correcto
                                correct = true;
                            }
                        }

                    }
                    else if (!gameMan.virusCanHappen)
                    {
                        //Level 2

                        if (cmdToExecute.isSudo)
                        {
                            if (checkOp(execute))
                            {
                                //Todo correcto
                                correct = execute;
                                if (!correct)
                                {
                                    addBlocked(cmdToExecute.file);
                                }
                            }
                            else
                            {
                                Debug.Log("Failed OP CHECK");
                                if (execute)
                                {
                                    addVirus(cmdToExecute.file);
                                }
                                else
                                {
                                    //Todo correcto
                                    correct = true;
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("Command Not Sudo");
                            if (execute)
                            {
                                addVirus(cmdToExecute.file);
                            }
                            else
                            {
                                //Todo correcto
                                correct = true;
                            }
                        }
                    }
                    else
                    {
                        //Level 3+
                        if (cmdToExecute.isSudo)
                        {
                            if (checkOp(execute))
                            {
                                //Todo correcto
                                correct = execute;
                                if (!correct)
                                {
                                    addBlocked(cmdToExecute.file);
                                }
                            }
                            else
                            {
                                if (execute)
                                {
                                    addVirus(cmdToExecute.file);
                                }
                                else
                                {
                                    //Todo correcto
                                    correct = true;
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("Command Not SUDO");
                            bool hasVirus = false;
                            switch (cmdToExecute.commandType)
                            {
                                case ECommand.COPY:
                                case ECommand.DELETE:
                                    break;

                                case ECommand.ADD:
                                    hasVirus = cmdToExecute.file.isVirus;
                                    break;
                                case ECommand.REPLACE:
                                    hasVirus = cmdToExecute.secondaryFile.isVirus;
                                    break;
                            }
                            Debug.Log("Has virus: " + hasVirus);
                            if (hasVirus)
                            {
                                if (execute)
                                {
                                    addVirus(cmdToExecute.file);
                                }
                                else
                                {
                                    //Todo correcto
                                    correct = true;
                                }
                            }
                            else
                            {
                                if (checkOp(execute))
                                {
                                    //Todo correcto
                                    correct = execute;
                                    if (!correct)
                                    {
                                        addBlocked(cmdToExecute.file);
                                    }
                                }
                                else
                                {
                                    if (execute)
                                    {
                                        addVirus(cmdToExecute.file);
                                    }
                                    else
                                    {
                                        //Todo correcto
                                        correct = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("Failed Event");
                    if (execute)
                    {
                        addVirus(cmdToExecute.file);
                    }
                    else
                    {
                        //Todo correcto
                        correct = true;
                    }
                }
            }
            else
            {
                Debug.Log("Fallo de validacion de op y nombre");
                if (execute)
                {
                    addVirus(cmdToExecute.file);
                }
                else
                {
                    //Todo correcto
                    correct = true;
                }
            }
        }
        else
        {
            Debug.Log("Check de tamano failed");
            if (execute)
            {
                addBlocked(cmdToExecute.file);
            }
            else
            {
                //Todo correcto
                correct = true;
            }
        }

        Debug.Log("Es correcto: " + correct);
        if (correct)
        {
            if (execute)
            {
                switch (cmdToExecute.commandType)
                {
                    case ECommand.COPY:
                        rootSizemanager.addFile(cmdToExecute.secondaryFile, ESlotState.FILLED);
                        break;
                    case ECommand.DELETE:
                        rootSizemanager.deleteFIle(cmdToExecute.file.nombre);
                        break;

                    case ECommand.ADD:
                        rootSizemanager.addFile(cmdToExecute.file, ESlotState.FILLED);
                        break;
                    case ECommand.REPLACE:
                        Debug.Log("Replace Command Execution");
                        rootSizemanager.deleteFIle(cmdToExecute.file.nombre);
                        rootSizemanager.addFile(cmdToExecute.secondaryFile, ESlotState.FILLED);
                        break;
                }
            }

            gameMan.increasePoints(gameMan.level * 15);
        }

        updateCommand(cmdCreator.getNextCommand());
    }

    public bool checkOp(bool execute)
    {
        if (cmdToExecute.commandType != ECommand.ADD)
        {
            return checkIfFileNameExists(cmdToExecute.file.nombre);
        }

        return true;
    }
    public void addBlocked(FileData file)
    {
        file.nombre = "";
        file.size = 1;
        rootSizemanager.addFile(file, ESlotState.BLOCKED);
    }

    public void addVirus(FileData file)
    {
        file.nombre = "";
        file.size = 2;
        rootSizemanager.addFile(file, ESlotState.VIRUS);
    }

    public bool checkTam()
    {
        switch (cmdToExecute.commandType)
        {
            case ECommand.ADD:
                return rootSizemanager.emptySlots >= cmdToExecute.file.size;

            case ECommand.COPY:
                return rootSizemanager.emptySlots >= cmdToExecute.file.size;

            case ECommand.DELETE:
                return true;

            case ECommand.REPLACE:
                int emptySpace = rootSizemanager.emptySlots - cmdToExecute.secondaryFile.size;

                for (int i = 0; i < rootSizemanager.slots.Count; i++)
                {
                    if (rootSizemanager.slots[i].slotData != null && rootSizemanager.slots[i].slotData.fileData != null &&
                        rootSizemanager.slots[i].slotData.fileData.nombre == cmdToExecute.file.nombre)
                    {
                        emptySpace += 1;
                    }
                }

                return emptySpace >= 0;
        }

        return false;
    }

    public bool genericTests(bool execute)
    {
        if (gameMan.lookForSudo) //Si estamos bsucando sudo, los no-sudo no deben pasar
        {
            if (gameMan.virusCanHappen) //Si puede haber virus, los no-sudo sin virus pueden pasar
            {
                if (cmdToExecute.isSudo)    //Si el comando es sudo y se ejecuta, todo correcto
                {
                    if (execute)
                        return true;
                    else
                    {
                        cmdToExecute.file.nombre = "";
                        cmdToExecute.file.size = 1;

                        rootSizemanager.addFile(cmdToExecute.file, ESlotState.BLOCKED);
                        return false;
                    }
                }
                else
                {
                    if (cmdToExecute.file.isVirus)  //Si no es sudo y tiene virus, si se ejecuta es malo
                    {
                        if (execute)
                        {
                            cmdToExecute.file.nombre = "";
                            cmdToExecute.file.size = 2;

                            rootSizemanager.addFile(cmdToExecute.file, ESlotState.VIRUS);
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else //El caso en el que no puede haber viruses, asi que los no-sudo 100% fuera
            {
                Debug.Log("No tiene virus");
                if (cmdToExecute.isSudo)
                {
                    if (execute)
                        return true;
                    else
                    {
                        cmdToExecute.file.nombre = "";
                        cmdToExecute.file.size = 1;

                        rootSizemanager.addFile(cmdToExecute.file, ESlotState.BLOCKED);
                        return false;
                    }

                }
                else
                {
                    if (execute)
                    {
                        cmdToExecute.file.nombre = "";
                        cmdToExecute.file.size = 2;

                        rootSizemanager.addFile(cmdToExecute.file, ESlotState.VIRUS);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        } //Si no estamos buscando por sudo, todo correcto
        return true;
    }

    public bool checkEvents()
    {
        for (int i = 0; i < gameMan.eventMan.activeEvents.Count; i++)
        {
            if(cmdToExecute.commandType == ECommand.REPLACE)
            {
                if (!gameMan.eventMan.activeEvents[i].isValid(cmdToExecute.secondaryFile, cmdToExecute.commandType))
                {
                    return false;
                }
            }
            else
            {
                if (!gameMan.eventMan.activeEvents[i].isValid(cmdToExecute.file, cmdToExecute.commandType))
                {
                    return false;
                }
            }
            
        }

        return true;
    }

    public void Update()
    {
        if (cmdToExecute == null && cmdCreator != null)
        {
            updateCommand(cmdCreator.getNextCommand());
        }
    }

    public void executeCommand()
    {
        confirmCommand(true);
    }

    public void discardCommand()
    {
        confirmCommand(false);
    }

    public bool CheckAddOperation(bool execute)
    {

        if (cmdToExecute.file.nombre == cmdToExecute.notification.fileName)
        {
            if (ECommand.ADD == cmdToExecute.notification.op)
            {
                //Si es un comando correcto y el usuario decide ejecutarlo
                if (execute)
                {
                    rootSizemanager.addFile(cmdToExecute.file, ESlotState.FILLED);
                    return true;
                }
                //Si es un comando correcto, pero el usuario decide descartar este comando
                else
                {
                    cmdToExecute.file.nombre = "";
                    cmdToExecute.file.size = 1;
                    rootSizemanager.addFile(cmdToExecute.file, ESlotState.BLOCKED);
                    return false;
                }
            }
            else//Si el fichero tiene algun fallo
            {
                //Si el usuario decide ejecutar un archivo no valido, mete un virus
                if (execute)
                {
                    cmdToExecute.file.size = 2;
                    cmdToExecute.file.nombre = "";
                    rootSizemanager.addFile(cmdToExecute.file, ESlotState.VIRUS);
                    return false;
                }
                //Si el jugador descarta un comando erroneo, ha tomado la decision correcta
                else
                {
                    return true;
                }
            }
        }
        else//Si el fichero tiene algun fallo
        {
            //Si el usuario decide ejecutar un archivo no valido, mete un virus
            if (execute)
            {
                cmdToExecute.file.size = 2;
                cmdToExecute.file.nombre = "";
                rootSizemanager.addFile(cmdToExecute.file, ESlotState.VIRUS);
                return false;
            }
            //Si el jugador descarta un comando erroneo, ha tomado la decision correcta
            else
            {
                return true;
            }
        }


    }

    public bool CheckCopyOperation(bool execute)
    {
        if (cmdToExecute.file.nombre == cmdToExecute.notification.fileName)
        {
            if (ECommand.COPY == cmdToExecute.notification.op)
            {
                //Comprobar si existe el archivo que se pretende copiar
                bool exists = checkIfFileNameExists(cmdToExecute.file.nombre);

                if (exists)
                {
                    if (execute)
                    {
                        Debug.Log("El archivo que se quiere copiar existe y el usuario quiere ejecutarlo");
                        rootSizemanager.addFile(cmdToExecute.secondaryFile, ESlotState.FILLED);
                        return true;
                    }
                    else
                    {
                        Debug.Log("El archivo que se quiere copiar existe y el usuario NO quiere ejecutarlo");
                        cmdToExecute.file.nombre = "";
                        cmdToExecute.file.size = 1;
                        rootSizemanager.addFile(cmdToExecute.file, ESlotState.BLOCKED);
                        return false;
                    }
                }
                //El archivo a copiar no existe en la root
                else
                {
                    if (execute)
                    {
                        Debug.Log("El archivo que se quiere copiar no existe en la root y el usuario quiere ejecutarlo");
                        cmdToExecute.secondaryFile.size = 2;
                        cmdToExecute.file.nombre = "";
                        rootSizemanager.addFile(cmdToExecute.secondaryFile, ESlotState.VIRUS);
                        return false;
                    }
                    else
                    {
                        Debug.Log("El archivo que se quiere copiar no existe en la root y el usuario NO quiere ejecutarlo");
                        return true;
                    }
                }
            }
            if (execute)
            {
                Debug.Log("El archivo que se quiere copiar no concuerda en la operacion y el usuario quiere ejecutarlo");
                cmdToExecute.secondaryFile.size = 2;
                cmdToExecute.secondaryFile.nombre = "";
                rootSizemanager.addFile(cmdToExecute.secondaryFile, ESlotState.VIRUS);
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (execute)
            {
                Debug.Log("El archivo que se quiere copiar no concuerda en el nombre y el usuario quiere ejecutarlo");
                cmdToExecute.secondaryFile.size = 2;
                cmdToExecute.secondaryFile.nombre = "";
                rootSizemanager.addFile(cmdToExecute.secondaryFile, ESlotState.VIRUS);
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public bool CheckRemoveOperation(bool execute)
    {
        if (cmdToExecute.file.nombre == cmdToExecute.notification.fileName)
        {
            if (ECommand.DELETE == cmdToExecute.notification.op)
            {
                //Comprobar si existe el archivo que se pretende eliminar
                bool exists = checkIfFileNameExists(cmdToExecute.file.nombre);

                if (exists)
                {
                    if (execute)
                    {
                        Debug.Log("El archivo que se quiere eliminar existe y el usuario quiere ejecutarlo");
                        rootSizemanager.deleteFIle(cmdToExecute.file.nombre);
                        return true;
                    }
                    else
                    {
                        Debug.Log("El archivo que se quiere eliminar existe y el usuario NO quiere ejecutarlo");
                        cmdToExecute.file.nombre = "";
                        cmdToExecute.file.size = 1;
                        rootSizemanager.addFile(cmdToExecute.file, ESlotState.BLOCKED);
                        return false;
                    }
                }
                //El archivo a copiar no existe en la root
                else
                {
                    if (execute)
                    {
                        Debug.Log("El archivo que se quiere eliminar no existe en la root y el usuario quiere ejecutarlo");
                        cmdToExecute.file.nombre = "";
                        cmdToExecute.file.size = 1;
                        rootSizemanager.addFile(cmdToExecute.file, ESlotState.BLOCKED);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            if (execute)
            {
                Debug.Log("El archivo que se quiere eliminar no concuerda en la operacion y el usuario quiere ejecutarlo");
                cmdToExecute.file.nombre = "";
                cmdToExecute.file.size = 1;
                rootSizemanager.addFile(cmdToExecute.file, ESlotState.BLOCKED);
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (execute)
            {
                cmdToExecute.file.nombre = "";
                cmdToExecute.file.size = 1;
                rootSizemanager.addFile(cmdToExecute.file, ESlotState.BLOCKED);
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public bool checkReplaceCommand(bool execute)
    {
        if (cmdToExecute.file.nombre == cmdToExecute.notification.fileName)
        {
            Debug.Log("Nombre = a lo otro");
            if (ECommand.REPLACE == cmdToExecute.notification.op)
            {
                Debug.Log("Comando igual a la notificacion");

                //Comprobar si existe el archivo que se pretende replacear
                bool exists = checkIfFileNameExists(cmdToExecute.file.nombre);
                Debug.Log("El archivo existe? " + exists);

                if (exists)
                {
                    if (execute)
                    {
                        Debug.Log("El archivo que se quiere replacear existe y el usuario quiere ejecutarlo");
                        rootSizemanager.deleteFIle(cmdToExecute.file.nombre);
                        rootSizemanager.addFile(cmdToExecute.secondaryFile, ESlotState.FILLED);
                        return true;
                    }
                    else
                    {
                        Debug.Log("El archivo que se quiere replacear existe y el usuario NO quiere ejecutarlo");
                        cmdToExecute.file.nombre = "";
                        cmdToExecute.file.size = 1;
                        rootSizemanager.addFile(cmdToExecute.file, ESlotState.BLOCKED);
                        return false;
                    }
                }
                //El archivo a copiar no existe en la root
                else
                {
                    if (execute)
                    {
                        Debug.Log("El archivo que se quiere replacear no existe en la root y el usuario quiere ejecutarlo");
                        cmdToExecute.file.nombre = "";
                        cmdToExecute.file.size = 1;
                        rootSizemanager.addFile(cmdToExecute.file, ESlotState.BLOCKED);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (execute)
                {
                    Debug.Log("El archivo que se quiere replacear no concuerda en la operacion y el usuario quiere ejecutarlo");
                    cmdToExecute.file.nombre = "";
                    cmdToExecute.file.size = 1;
                    rootSizemanager.addFile(cmdToExecute.file, ESlotState.BLOCKED);
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        else
        {
            if (execute)
            {
                cmdToExecute.file.nombre = "";
                cmdToExecute.file.size = 1;
                rootSizemanager.addFile(cmdToExecute.file, ESlotState.BLOCKED);
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public void updateCommand(Command cmd)
    {
        cmdToExecute = cmd;
        if(cmd.commandType == ECommand.REPLACE)
        {
            fileInMiddleOfTheScreen.UpdateFileData(cmd.secondaryFile);
        }
        else
        {
            fileInMiddleOfTheScreen.UpdateFileData(cmd.file);
        }
        activeBtn.updateCommand(cmd);
    }

    private bool checkIfFileNameExists(string name)
    {
        for (int i = 0; i < rootSizemanager.maxSlots; i++)
        {
            if (rootSizemanager.slots[i].slotData.fileData != null &&
               rootSizemanager.slots[i].slotData.fileData.nombre == cmdToExecute.file.nombre)
            {
                return true;
            }
        }
        return false;
    }
}
