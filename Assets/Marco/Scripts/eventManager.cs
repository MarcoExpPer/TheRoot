using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class OurEvents
{
    abstract public bool isValid(FileData fileToCheck);

    abstract public string getDescription();

}

public class LimitOrigin : OurEvents
{
    bool isWhiteList;
    List<Color> colors;

    public LimitOrigin(List<Color> colors, bool iswhiteList)
    {
        this.colors = colors;
        this.isWhiteList = iswhiteList;
    }

    public override string getDescription()
    {
        if (isWhiteList)
        {
            return "From now on, we will only accept files form our trusted colored sources";
        }
        else
        {
            return "From now on, we wont trust some of our colored sources";
        }
    }

    override public bool isValid(FileData fileToCheck)
    {
        if (isWhiteList)
        {
            foreach (Color color in colors)
            {
                if (fileToCheck.origen != color)
                {
                    return false;
                }
            }
        }
        else
        {
            foreach (Color color in colors)
            {
                if (fileToCheck.origen == color)
                {
                    return false;
                }
            }
        }

        return true;
    }


}

public class LimitDate : OurEvents
{
    bool limitBeforeDate;
    DateTime date;

    public LimitDate(DateTime date, bool beforeDate)
    {
        this.date = date;
        this.limitBeforeDate = beforeDate;
    }

    override public bool isValid(FileData fileToCheck)
    {
        if (limitBeforeDate)
        {
            if (date < fileToCheck.date)
            {
                return false;
            }
            else return true;
        }
        else
        {
            if (date > fileToCheck.date)
            {
                return false;
            }
            else return true;
        }
    }

    public override string getDescription()
    {
        if (limitBeforeDate)
        {
            return "From now on, we wont accept outdated files older than " + date.ToString();
        }
        else
        {
            return "We have found some weird files from the future, dont accept any from after " + date.ToString();
        }
    }
}

public class LimitSize : OurEvents
{
    int sizeToLimit;
    bool limitSmallers;

    public LimitSize(int size, bool limitSmall)
    {
        this.sizeToLimit = size;
        this.limitSmallers = limitSmall;
    }
    override public bool isValid(FileData fileToCheck)
    {
        if (this.limitSmallers)
        {
            return fileToCheck.size < this.sizeToLimit;
        }
        else
        {
            return fileToCheck.size > this.sizeToLimit;
        }
    }

    public override string getDescription()
    {
        if (this.limitSmallers)
        {
            return "For some reason, small files are hurting our Root. Dont allow files lower than " + this.sizeToLimit + "into the root";
        }
        else
        {
            return "Our root is suffering, dont upload files higher than " + this.sizeToLimit;
        }
    }
}


public class eventManager : MonoBehaviour
{
    List<Color> colorsToUse = new List<Color>();
    List<OurEvents> eventList = new List<OurEvents>();

    public List<OurEvents> activeEvents = new List<OurEvents>();
    // Start is called before the first frame update
    void Start()
    {
        colorsToUse.Add(Color.red);
        colorsToUse.Add(Color.blue);
        colorsToUse.Add(Color.green);
        colorsToUse.Add(Color.yellow);

        refreshEvents();
    }

    public OurEvents addNewEvents()
    {
        int index = UnityEngine.Random.Range(0, eventList.Count - 1);

        OurEvents ev = eventList[index];
        eventList.RemoveAt(index);

        activeEvents.Add(ev);
        return ev;
    }

    public void refreshEvents()
    {
        eventList.Clear();
        activeEvents.Clear();

        //Preparar evento de color.
        int nColors = UnityEngine.Random.Range(0, 2);
        List<Color> colorsToFilter = new List<Color>();

        for (int i = 0; i < nColors; i++)
        {
            //Soy consciente de que el ultimo color nunca saldra escogido
            colorsToFilter.Add(colorsToUse[i]);
        }

        //0 significa whitelist, 1 significa blacklist
        eventList.Add(new LimitOrigin(colorsToFilter, UnityEngine.Random.Range(0, 1) == 0));

        //Preparar evento de fecha.
        DateTime dateToFIlter = FileData.GetRandomDate();
        eventList.Add(new LimitDate(dateToFIlter, UnityEngine.Random.Range(0, 1) == 0));


        eventList.Add(new LimitSize(UnityEngine.Random.Range(1, 2), UnityEngine.Random.Range(0, 1) == 0));

    }
}
