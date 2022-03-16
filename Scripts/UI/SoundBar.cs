using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundBar : MonoBehaviour
{
    public int ID;

    public bool Muted;

    public float Volume, Pitch, VolumePrevious, PitchPrevious;
    public int SoundChosen;
    public string SoundChosen_Name;

    public int LastSoundPlayed;

    public GameObject MuteButton_SoundControl;
    public SoundDetailsControlPanel soundDetailsControlPanel;
    public TMP_Dropdown soundDropDown;
    public Slider VolumeMeter_Slider, VolumeMeter_Slider_SOLO, PitchMeter_Slider;

    public GameObject quickPitch, quickVolume, quickVolume_SOLO;


    //Tbc later....
    public List<Sprite> soundImages = new List<Sprite>();
    public Image soundImage;
    public TMP_Dropdown soundOptionsDropDown;
    bool optionsDropDownSetupFinished;

    #region Methods


    public void InitializeKeyFields(float _volume, float _pitch, int _soundChosen, bool _muted)
    {
        Volume = _volume;
        Pitch = _pitch;
        SoundChosen = _soundChosen;
        Muted = _muted;

        SoundChosen_Name = Find_Sound_Name(SoundChosen);


        StartCoroutine(InitializeUIValues());
    }
    IEnumerator InitializeUIValues()
    {
        yield return new WaitForSecondsRealtime(0.05f); //can always increase time more if we run into latency bugs with frames manager dropping...


        soundDropDown.value = SoundChosen;
        VolumeMeter_Slider.value = Volume;
        VolumeMeter_Slider_SOLO.value = Volume;

        PitchMeter_Slider.value = Pitch;

        ToggleMuteButton(Muted);

        if (!optionsDropDownSetupFinished) Setup_SoundDropdownOptions();

      //  soundImage.sprite = soundImages[SoundChosen];
    }


    //ALREADY Based on Sound Bar ID
    public string Find_Sound_Name(int SoundToFind)
    {
        string soundName = default;
        switch(ID)
        {
            case 0:
            switch(SoundToFind)
            {
                    case 0:
                        soundName = "Summer Burn";
                        break;
                    case 1:
                        soundName = "Winter Rain";
                        break;
                    case 2:
                        soundName = "Spring Prance";
                        break;
                }
                break;
            case 1:
                switch (SoundToFind)
                {
                    case 0:
                        soundName = "Light Chimes";
                        break;
                    case 1:
                        soundName = "Heavy Harp";
                        break;
                    case 2:
                        soundName = "Light Harp";
                        break;
                }
                break;
            case 2:
                switch (SoundToFind)
                {
                    case 0:
                        soundName = "Light Drums";
                        break;
                    case 1:
                        soundName = "Intense Drum Solo";
                        break;
                    case 2:
                        soundName = "Rock Drums Solo";
                        break;
                }
                break;
            case 3:
                switch (SoundToFind)
                {
                    case 0:
                        soundName = "RainDrops";
                        break;
                    case 1:
                        soundName = "Thunder";
                        break;
                    case 2:
                        soundName = "Rain on Tin";
                        break;
                }
                break;

        }

        return soundName;
    }

    public void OnPointerClick_ScrollDropDown_ToSelection(string data)
    {
        GameObject ddl = GameObject.Find("Dropdown List");
        GameObject vp = ddl.GetComponentsInChildren<RectTransform>()[1].gameObject;
        GameObject content = vp.GetComponentsInChildren<RectTransform>()[1].gameObject;

        RectTransform rt = content.GetComponent<RectTransform>();

        int index = soundOptionsDropDown.value;
        // 75 is the height of an item in my dropdown
        rt.position = rt.position + Vector3.up * index * 75;
    }

    void Setup_SoundDropdownOptions()
    {
        soundOptionsDropDown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        switch (ID)
        {
            case 0:
                foreach (var item in SoundManager.Instance.SoundsBar1)
                {
                    TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData() { text = item.Name/*, image = Backgrounds[backgroundSelection]*/ };
                    options.Add(option);
                }
           break;
            case 1:
                foreach (var item in SoundManager.Instance.SoundsBar2)
                {
                    TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData() { text = item.Name/*, image = Backgrounds[backgroundSelection]*/ };
                    options.Add(option);
                }
                break;
            case 2:
                foreach (var item in SoundManager.Instance.SoundsBar3)
                {
                    TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData() { text = item.Name/*, image = Backgrounds[backgroundSelection]*/ };
                    options.Add(option);
                }
                break;
            case 3:
                foreach (var item in SoundManager.Instance.SoundsBar4)
                {
                    TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData() { text = item.Name/*, image = Backgrounds[backgroundSelection]*/ };
                    options.Add(option);
                }
                break;
            case 4:
                foreach (var item in SoundManager.Instance.SoundsBar5)
                {
                    TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData() { text = item.Name/*, image = Backgrounds[backgroundSelection]*/ };
                    options.Add(option);
                }
                break;
        }

        soundOptionsDropDown.AddOptions(options);
        optionsDropDownSetupFinished = true;
    }


    public void Toggle_HideQuickPitch(bool On)
    {
        App.Instance.hideQuickPitch.isOn = On;

        int hideQuickPitchIs = On ? 1 : 0;
        PlayerPrefs.SetInt("HideQuickPitch", hideQuickPitchIs);

        if (On)
        {
            quickPitch.SetActive(false);
            quickVolume.SetActive(false);

            quickVolume_SOLO.SetActive(true);

            VolumeMeter_Slider_SOLO.value = Volume;
        }
        else
        {
            quickPitch.SetActive(true);
            quickVolume.SetActive(true);

            quickVolume_SOLO.SetActive(false);

            VolumeMeter_Slider.value = Volume;
        }
    }

    public void ToggleMuteButton(bool muted)
    {
        if (muted)
        {
          MuteButton_SoundControl.transform.GetChild(0).gameObject.SetActive(true);
          MuteButton_SoundControl.transform.GetChild(1).gameObject.SetActive(false);

          VolumeMeter_Slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;
            VolumeMeter_Slider_SOLO.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;
        }
        else
        {
            MuteButton_SoundControl.transform.GetChild(0).gameObject.SetActive(false);
            MuteButton_SoundControl.transform.GetChild(1).gameObject.SetActive(true);

            VolumeMeter_Slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.green;
            VolumeMeter_Slider_SOLO.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.green;
        }
    }

    public void Pass_SoundBarValue()
    {
        App.Instance.CurrentSoundBar_Tweaking_ID = ID;
    }
    public void Change_Volume(float vol)
    {
        VolumePrevious = Volume;
        Volume = vol;
        VolumeMeter_Slider.value = vol;
    
        

        SoundManager.Instance.ChangeSoundDynamics_Int(vol, Pitch, true, Muted, SoundChosen, ID);


        if (soundDetailsControlPanel.InitialUIValueBypassFinished) //Because vol actuall
        {
            if (vol <= 0)
            {
                Muted = true;
                ToggleMuteButton(true);
            }
            else if (Muted)
            {
                Muted = false;
                ToggleMuteButton(false);
            }
        }
    }
    public void Change_Pitch(float pitch)
    {
        App.Instance.CurrentSoundBar_Tweaking_ID = ID;

        PitchPrevious = Pitch;
        PitchMeter_Slider.value = pitch;
        Pitch = pitch;

        SoundManager.Instance.ChangeSoundDynamics_Int(Volume, pitch, true, Muted, SoundChosen, ID);
    }
    public void Change_Mute()
    {
        Muted = !Muted;

        SoundManager.Instance.ChangeSoundDynamics_Int(Volume, Pitch, true, Muted, SoundChosen, ID);
    }

    public void Change_Sound_String(int SoundIndex)
    {
        SoundManager.Instance.StopSound_By_String(Find_Sound_Name(LastSoundPlayed), ID);

        SoundChosen = SoundIndex;
        SoundChosen_Name =  Find_Sound_Name(SoundIndex);
        LastSoundPlayed = SoundIndex;

        if (App.Instance.play) SoundManager.Instance.PlaySound_By_String(SoundChosen_Name, ID);
        SoundManager.Instance.ChangeSoundDynamics_Int(Volume, Pitch, true, Muted, SoundChosen, ID);


        //  soundImage.sprite = soundImages[SoundIndex];
    }
    public void Change_Sound_Index(int SoundIndex)
    {
        SoundManager.Instance.StopSound_By_Index(LastSoundPlayed, ID);

        SoundChosen = SoundIndex;
        SoundChosen_Name = Find_Sound_Name(SoundIndex);
        LastSoundPlayed = SoundIndex;


        if (App.Instance.play) SoundManager.Instance.PlaySound_By_Index(SoundIndex, ID);
        SoundManager.Instance.ChangeSoundDynamics_Int(Volume, Pitch, true, Muted, SoundChosen, ID);

        // soundImage.sprite = soundImages[SoundIndex];
    }


    public bool SoundStatus_IsPlaying() 
    {
       return SoundManager.Instance.SoundStatus_IsPlaying(SoundChosen, ID);
    }
    public void Toggle_PlayPauseStop(int choice)
    {
        if(choice == 0) SoundManager.Instance.PlaySound_By_Index(LastSoundPlayed, ID);
        else if(choice == 1) SoundManager.Instance.PauseSound_By_Index(LastSoundPlayed, ID);
        else if(choice == 2) SoundManager.Instance.StopSound_By_Index(LastSoundPlayed, ID);
    }

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(1f);

        SoundChosen_Name = Find_Sound_Name(SoundChosen);

        SoundManager.Instance.ChangeSoundDynamics_Int(Volume, Pitch, true, Muted, SoundChosen, ID);
     
        // SoundManager.Instance.PlaySound_By_Index(SoundChosen, ID);


        VolumeMeter_Slider.value = Volume;
        VolumeMeter_Slider_SOLO.value = Volume;
        PitchMeter_Slider.value = Pitch;

    }

  



    #endregion
}
