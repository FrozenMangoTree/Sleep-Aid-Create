using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SoundDetailsControlPanel : MonoBehaviour
{
    public Slider Volume_Slider, Pitch_Slider;
    public GameObject MuteButton;
    public float Volume, Pitch;
    public bool Mute;

    public bool InitialUIValueBypassFinished; //External check use by SoundBars
  

    public void SetValues_ToUseWithUI(float _Vol, float _Pitch, bool _Mute)
    {
        Volume = _Vol;
        Pitch = _Pitch;
        Mute = _Mute;
    }

    public void SetUIValues()
    {
        Volume_Slider.value = Volume;
        Pitch_Slider.value = Pitch;


        if (Mute)
        {
            MuteButton.transform.GetChild(0).gameObject.SetActive(true);
            MuteButton.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            MuteButton.transform.GetChild(0).gameObject.SetActive(false);
            MuteButton.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    void ClearUIValues()
    {
        Volume_Slider.value = 0f;
        Pitch_Slider.value = 0;

        Volume = 0f;
        Pitch =  0f;
        Mute = false;

    }

    IEnumerator JustEnabled_BypassInitialUIValueCallback()
    {
        InitialUIValueBypassFinished = false;
        yield return new WaitForSecondsRealtime(0.2f);
        InitialUIValueBypassFinished = true;
    }
    void OnEnable()
    {
        StartCoroutine(JustEnabled_BypassInitialUIValueCallback());
        SetUIValues();
    }
    //void OnDisable() => ClearUIValues();

}
