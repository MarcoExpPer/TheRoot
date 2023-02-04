using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimizeButton : MonoBehaviour
{
   public void Minimize()
    {
        transform.parent.GetComponent<Window>().minimized = true;
        transform.parent.gameObject.SetActive(false);
    }
}
