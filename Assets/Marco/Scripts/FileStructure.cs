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
    public bool isVirus = false;

    public FileData() { }

    public FileData(string name, Color origin, int tamano, bool isVirus = false)
    {
        nombre = name;
        origen = origin;
        size = tamano;
        this.date = GetRandomDate();
        this.isVirus = isVirus;
    }
    public FileData(string name, Color origin, int tamano, string nameToReplace, bool isVirus = false)
    {
        nombre = name;
        origen = origin;
        size = tamano;
        this.date = GetRandomDate();
        this.NameToReplace = nameToReplace;
        this.isVirus = isVirus;
    }

    public FileData(FileData data)
    {
        nombre = data.nombre;
        origen = data.origen;
        size = data.size;
        this.date = data.date;
        this.NameToReplace = "";
        this.isVirus = data.isVirus;
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

