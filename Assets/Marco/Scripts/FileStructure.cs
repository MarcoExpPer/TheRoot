using UnityEngine;
using UnityEditor;
using System;

public class FileData
{
    public string nombre = "";
    public DateTime date; 
    public Color origen = Color.black;
    public int size = 1;
    public string NameToReplace = "";

    public FileData() { }

    public FileData(string name, Color origin, int tamano)
    {
        nombre = name;
        origen = origin;
        size = tamano;
        this.date = GetRandomDate();
    }
    public FileData(string name, Color origin, int tamano, string nameToReplace)
    {
        nombre = name;
        origen = origin;
        size = tamano;
        this.date = GetRandomDate();
        this.NameToReplace = nameToReplace;
    }

    public static DateTime GetRandomDate()
    {
        DateTime today = DateTime.Today;
        DateTime endDate = today.AddYears(3);
        DateTime startDate = today.AddYears(-3);

        TimeSpan timeSpann = endDate - startDate;
        TimeSpan newSpan = new TimeSpan(0, UnityEngine.Random.Range(0, (int)timeSpann.TotalMinutes), 0);
        return startDate + newSpan;
    }
}

