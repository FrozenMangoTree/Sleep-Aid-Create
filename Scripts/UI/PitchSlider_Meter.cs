using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PitchSlider_Meter : MonoBehaviour, IPointerUpHandler
{
    public SoundBar soundBar;
    public Slider pitchSlider;


    public void OnPointerUp(PointerEventData eventData)
    {
        if (pitchSlider.value != soundBar.PitchPrevious) App.Instance.Save_An_UpdatedPreset();
    }

  
}
