using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rt;


    private void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
    }


    public void OnBeginDrag(PointerEventData EventData)
    {

    }

    public void OnDrag(PointerEventData EventData) 
    {
        rt.anchoredPosition += EventData.delta;
        transform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData EventData) 
    { 

    }

    public void OnPointerDown(PointerEventData EventData)
    {

    }
}
