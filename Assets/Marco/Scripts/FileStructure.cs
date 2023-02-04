using UnityEngine;
using UnityEditor;

public class FileData
{
    public string nombre = "";
    public string date = "";
    public Color origen = Color.black;
    public int size = 1;

    public FileData() { }

    public FileData(string name, Color origin, int tamano)
    {
        nombre = name;
        origen = origin;
        size = tamano;
    }
}