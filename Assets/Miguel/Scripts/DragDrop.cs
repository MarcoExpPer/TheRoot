using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private RectTransform rt;


    private void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        canvas = transform.parent.GetComponent<Canvas>();

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
