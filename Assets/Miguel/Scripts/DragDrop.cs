using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Canvas canvas;
    public RectTransform rt;


    private void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        //canvas = rt.parent.GetComponent<Canvas>();

    }


    public void OnBeginDrag(PointerEventData EventData)
    {

    }

    public void OnDrag(PointerEventData EventData) 
    {
        rt.anchoredPosition += EventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData EventData) 
    { 

    }

    public void OnPointerDown(PointerEventData EventData)
    {

    }
}
