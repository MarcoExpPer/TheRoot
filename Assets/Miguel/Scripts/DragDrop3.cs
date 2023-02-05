using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop3 : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [HideInInspector] public RectTransform rt;
    private Vector2 middlePoint = new Vector2(0f, 0f);

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
        if (rt.anchoredPosition.x > -540f + rt.sizeDelta.x / 2 && rt.anchoredPosition.x < 550f - rt.sizeDelta.x / 2 &&
            rt.anchoredPosition.y > -290f + rt.sizeDelta.y / 2 && rt.anchoredPosition.y < 330f - rt.sizeDelta.y / 2)
        {
            rt.anchoredPosition += EventData.delta;
            transform.SetAsLastSibling();
        }
        else
        {
            float despX = ((rt.anchoredPosition.x - middlePoint.x) <= 0f) ? 1f : -1f;
            float despY = ((rt.anchoredPosition.y - middlePoint.y) <= 0f) ? 1f : -1f;
            rt.anchoredPosition += new Vector2(despX, despY);
        }

    }

    public void OnEndDrag(PointerEventData EventData)
    {

    }

    public void OnPointerDown(PointerEventData EventData)
    {

    }
}
