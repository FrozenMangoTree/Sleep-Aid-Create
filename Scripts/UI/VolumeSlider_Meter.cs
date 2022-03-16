using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class VolumeSlider_Meter : MonoBehaviour, IPointerUpHandler
{
    public SoundBar soundBar;
    public Slider volumeSlider;


    public void OnPointerUp(PointerEventData eventData)
    {
        /*if (volumeSlider.value != soundBar.VolumePrevious) */App.Instance.Save_An_UpdatedPreset();    
    }

}
