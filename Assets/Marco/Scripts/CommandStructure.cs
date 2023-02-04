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
    public cmdNotification notification;
    public FileData file;
    public FileData secondaryFile;
    public bool isSudo;

    public Command(ECommand commandType, cmdNotification notification, FileData file, bool isSudo)
    {
        this.commandType = commandType;
        this.file = file;
        this.notification = notification;
        this.secondaryFile = null;
        this.isSudo = isSudo;
    }

    public Command(ECommand commandType, cmdNotification notification, FileData file, FileData secondaryFile, bool isSudo)
    {
        this.commandType = commandType;
        this.notification = notification;
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

public class cmdNotification
{
    public string fileName;
    public string secondaryFileName;
    public ECommand op;

    public cmdNotification(string name, ECommand op)
    {
        this.fileName = name;
        this.op = op;
    }

    public cmdNotification(string name, string secondary,ECommand op)
    {
        this.fileName = name;
        this.secondaryFileName = secondary;
        this.op = op;
    }
}