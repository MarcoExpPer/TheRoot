using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ECommand
{
    ADD,
    DELETE,
    REPLACE,
    COPY
}

public class Command {
    //Variable declaration
    //Note: I'm explicitly declaring them as public, but they are public by default. You can use private if you choose.
    public string figure;
    public ECommand commandType;
    public string file;
    public bool isSudo;

    public Command(string figure, ECommand commandType, string file, bool isSudo)
    {
        this.figure = figure;
        this.commandType = commandType;
        this.file = file;
        this.isSudo = isSudo;
    }

}

public class CommandsQueue : MonoBehaviour
{
    [SerializeField]
    int queueMaxSize = 10;

    //[SerializeField]
    public delegate void QueueFilledDelegate();
    public event QueueFilledDelegate OnQueueFilled;

    public int currentQueueSize = 0;
    List<Command> queue = new List<Command>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {

    }

    public void addCommand(Command commandToAdd)
    {
        if(currentQueueSize < queueMaxSize)
        {
            currentQueueSize += 1;
            queue.Add(commandToAdd);
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

            return removedCmd; 
        }
        else
        {
            return null;
        }
    }

    public Command getCommand(int index)
    {
        if (currentQueueSize != 0)
        {
            return queue[index];
        }
        else
        {
            return null;
        }
    }

    public static string printCommand(Command cmdToPrint)
    {
        return cmdToPrint.commandType + " " + cmdToPrint.figure + " " + cmdToPrint.file + " " + cmdToPrint.isSudo;
    }



}
