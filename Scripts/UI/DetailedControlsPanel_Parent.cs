using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetailedControlsPanel_Parent : MonoBehaviour, IPointerDownHandler
{
   // public GameObject soundBars;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        //  soundBars.SetActive(true);
        App.Instance.Save_An_UpdatedPreset();
        gameObject.SetActive(false);
    }
}
