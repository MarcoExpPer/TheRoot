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

    public void Start()
    {
  
    }

    public void confirmCommand(bool execute)
    {
        Debug.Log("Execute Command: " + execute + " " + cmdToExecute.commandType);
        bool result;
        switch (cmdToExecute.commandType)
        {
            case ECommand.ADD:
                result = CheckAddOperation(execute);
                break;
            case ECommand.COPY:
                result = CheckCopyOperation(execute);
                break;
            case ECommand.DELETE:
                result = CheckRemoveOperation(execute);
                break;
            case ECommand.REPLACE:
                result = checkReplaceCommand(execute);
                break;
        }

        updateCommand(cmdCreator.getNextCommand());
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
        if(cmdToExecute.file.nombre == cmdToExecute.notification.fileName)
        {
            if(ECommand.ADD == cmdToExecute.notification.op)
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
                bool exists = false;
                for(int i = 0; i < rootSizemanager.maxSlots && !exists; i++)
                {
                    if (rootSizemanager.slots[i].slotData.fileData != null &&
                       rootSizemanager.slots[i].slotData.fileData.nombre == cmdToExecute.file.nombre)
                    {
                        exists = true;
                    }
                }

                if (exists)
                {
                    if (execute) {
                        Debug.Log("El archivo que se quiere copiar existe y el usuario quiere ejecutarlo");
                        rootSizemanager.addFile(cmdToExecute.secondaryFile, ESlotState.FILLED);
                        return true;
                    }
                    else
                    {
                        Debug.Log("El archivo que se quiere copiar existe y el usuario NO quiere ejecutarlo");
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
                rootSizemanager.addFile(cmdToExecute.secondaryFile, ESlotState.VIRUS);
                return false;
            }
            else
            {
                return true;
            }
        }
        else{
            if (execute)
            {
                Debug.Log("El archivo que se quiere copiar no concuerda en el nombre y el usuario quiere ejecutarlo");
                cmdToExecute.secondaryFile.size = 2;
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
                bool exists = false;
                for (int i = 0; i < rootSizemanager.maxSlots && !exists; i++)
                {
                    if (rootSizemanager.slots[i].slotData.fileData != null &&
                       rootSizemanager.slots[i].slotData.fileData.nombre == cmdToExecute.file.nombre)
                    {
                        exists = true;
                    }
                }

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
            if (ECommand.REPLACE == cmdToExecute.notification.op)
            {
                Debug.Log(cmdToExecute.file);

                //Comprobar si existe el archivo que se pretende replacear
                bool exists = false;
                for (int i = 0; i < rootSizemanager.maxSlots && !exists; i++)
                {
                    if (rootSizemanager.slots[i].slotData.fileData != null && 
                        rootSizemanager.slots[i].slotData.fileData.nombre == cmdToExecute.file.nombre)
                    {
                        exists = true;
                    }
                }

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
                Debug.Log("El archivo que se quiere replacear no concuerda en la operacion y el usuario quiere ejecutarlo");
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
        activeBtn.updateCommand(cmd);
    }
}
