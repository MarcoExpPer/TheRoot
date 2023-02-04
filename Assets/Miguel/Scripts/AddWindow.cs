using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class AddWindow : MonoBehaviour
{
    [SerializeField] private GameObject windowPrefab;
    [SerializeField] private GameObject postItPrefab;
    private Canvas canvas;

    private int numWindows = 0;

    private void Start()
    {
        canvas = transform.parent.GetComponent<Canvas>() ;
    }

    public void createWindow()
    {
        if (numWindows < 4)
        {
            Instantiate(windowPrefab, canvas.transform);
            numWindows++;
        }
        else
        {
            CreatePostIt();
        }
        
    }

    private void CreatePostIt()
    {
        Instantiate(postItPrefab, canvas.transform);
    }
}
