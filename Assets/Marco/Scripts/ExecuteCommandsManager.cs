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

        return false;
    }

    public void updateCommand(Command cmd)
    {
        cmdToExecute = cmd;
        activeBtn.updateCommand(cmd);
    }
}
