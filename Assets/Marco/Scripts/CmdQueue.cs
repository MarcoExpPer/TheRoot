using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class CmdQueue : MonoBehaviour
{
    [SerializeField]
    public int queueMaxSize = 10;

    public delegate void QueueFilledDelegate();
    public event QueueFilledDelegate OnQueueFilled;

    public delegate void QueueChanged(int index, EQueueChangedAction action, Command cmd);
    public event QueueChanged OnQueueChanged;

    public int currentQueueSize = 0;
    List<Command> queue = new List<Command>();

    public void addCommand(Command commandToAdd)
    {
        if(currentQueueSize < queueMaxSize)
        {
            queue.Add(commandToAdd);
            OnQueueChanged.Invoke(currentQueueSize, EQueueChangedAction.ADD_CMD, commandToAdd);

            currentQueueSize += 1;
        }
        else
        {
            OnQueueFilled();
        }
    }

    public Command popCommand()
    {
        if (currentQueueSize != 0)
        {
            currentQueueSize -= 1;
            Command removedCmd = queue[0];
            queue.RemoveAt(0);

            OnQueueChanged.Invoke(0, EQueueChangedAction.DEL_CMD, null);

            return removedCmd; 
        }
        else
        {
            return null;
        }
    }

    public Command getCommand(int index)
    {
        if (currentQueueSize != 0 && index < currentQueueSize)
        {
            return queue[index];
        }
        else
        {
            return null;
        }
    }

    public static string printCommandInStack(Command cmdToPrint)
    {
        return cmdToPrint.commandType + cmdToPrint.commandToFigure();
    }



}
