using UnityEngine;
using UnityEditor;

public class FileData
{
    public string nombre = "";
    public string date = "";
    public Color origen = Color.black;
    public int size = 1;
    public string NameToReplace = "";

    public FileData() { }

    public FileData(string name, Color origin, int tamano)
    {
        nombre = name;
        origen = origin;
        size = tamano;
        this.date = date;
    }
    public FileData(string name, Color origin, int tamano, string nameToReplace)
    {
        nombre = name;
        origen = origin;
        size = tamano;
        this.date = date;
        this.NameToReplace = nameToReplace;
    }
}

public class OurDate
{

}