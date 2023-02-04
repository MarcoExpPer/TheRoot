using UnityEngine;
using UnityEditor;

public enum ECommand
{
    ADD,
    DELETE,
    REPLACE,
    COPY
}

public enum EQueueChangedAction
{
    ADD_CMD,
    DEL_CMD,

}

public class Command
{
    //Variable declaration
    public ECommand commandType;
    public FileData file;
    public FileData secondaryFile;
    public bool isSudo;

    public Command(ECommand commandType, FileData file, bool isSudo)
    {
        this.commandType = commandType;
        this.file = file;
        this.secondaryFile = null;
        this.isSudo = isSudo;
    }

    public Command(ECommand commandType, FileData file, FileData secondaryFile, bool isSudo)
    {
        this.commandType = commandType;
        this.file = file;
        this.secondaryFile = secondaryFile;
        this.isSudo = isSudo;
    }


    public string commandToFigure()
    {
        switch (commandType)
        {
            case ECommand.ADD:
                return "+";
            case ECommand.COPY:
                return "[]";
            case ECommand.DELETE:
                return "()";
            case ECommand.REPLACE:
                return "/\\";
        }

        return "";
    }
}