using AdvancedInputFieldPlugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class App : MonoBehaviour, IPointerDownHandler //EVENT TRIGGER Addable COMPONENT ARE HUGEEEEE (MUST USE, in future!!!)
{

    public static App Instance;

    public event EventHandler OnAppStart;

    [Header("App Settings")]
    #region AppSettings
    public bool AppNeverSleep;
    #endregion
    #region Mixer Fields
    public bool Muted;


    public List<GameObject> SoundBar = new List<GameObject>();
    public Transform SoundMixerParent;
    public int CurrentSoundBar_Tweaking_ID;

    public SoundDetailsControlPanel soundDetailsControlPanel;
    
    public GameObject MuteButton_SoundControlPanel;




    //Preloaded Values
  
    public List<TrackPreset> DefaultTrackPresets = new List<TrackPreset>();

    /*public List<> SoundBar */



    //Background
    public Toggle Background1Toggle, Background2Toggle;
    public Image MainAppBackground, BackgroundToggled1, BackgroundToggled2;
    public Transform BackgroundToggled1Visible, BackgroundToggled1Hidden;
    public Transform BackgroundToggled2Visible, BackgroundToggled2Hidden;

    public TMP_Dropdown MainBackgroundDropDown;

    //Preset Selection
    public TMP_Dropdown presetSelection;
    public List<Sprite> Backgrounds = new List<Sprite>();
    public int indexOfLastChosenPreset_FromData;

    //Preset Saving
    public AdvancedInputField presetTitle;
    public TextMeshProUGUI presetDateTime;
    public GameObject presetSavingMainParent;
    public TMP_Dropdown presetBackgroundDropdown;
    public TMP_InputField presetInputField;

    //Load/Save wait
    bool AppLoaded = false;


    //Other
    public TextMeshProUGUI noticeText;

    // public int LastPresetBackgroundChosen;
    //public int LastPresetNameUsed;


    #endregion
    #region Timer Fields

    public Timer mainTimer;



    //Timer
    public AdvancedInputField timerField_Hour, timerField_Minute;
    public TextMeshProUGUI timerText_Seconds;
    public Image timerCountRing;
    public GameObject timerPlay, timerPause;

    public float timer_Seconds, timer_Minutes, timer_Hours;
    public float originalTimerAmount_Hours, originalTimerAmount_Minutes, originalTimerAmount_Seconds;
    public float timerNormalized;
    public bool timerValueWasChanged;


    float timeAtPlay;
    float differenceOfTimeAtPause;

    //If both are false, then we are stopped
    public bool play;
    public bool paused; 
    public bool loop;

    int timesVibrated;

    bool playTimer;

    public GameObject loopImage, noLoopImage;
    public GameObject Play_Button, Stop_Button;

    //Player prefs Setting
    bool settingsOpen;
    public GameObject settingsPanel;
    public Toggle saveTimerPerPreset;
    public Toggle defaultTimerValue;
    public Toggle resetSettings;
    public Toggle hideQuickPitch;

    public float DefaultTimerValue;

    

    #endregion
    #region PresetTools
    public GameObject DeletePresetPrompt;
    public Toggle deletePresetPromptToggle;
    #endregion

    #region Timer Methods
    //Lets use timer/count down as an alarm too, that vibrates and makes a noise at end!

    public void Toggle_HideQuickPitch(bool On)
    {
       foreach (var item in SoundBar) item.GetComponentInChildren<SoundBar>().Toggle_HideQuickPitch(On);
    }


    void ResetTimer_InvokeExternalDelegates()
    {
        mainTimer.End();


      

        mainTimer.SetDuration((int)originalTimerAmount_Seconds)
        .OnEnd(() => { OnTimerFinish(loop); }).OnChange((seconds) =>
        {
            timer_Seconds = seconds;
            Seconds_To_PreferedFormat();
            timerCountRing.fillAmount = timerNormalized;

        }).OnPause((_paused) =>
        {

        });

        mainTimer.SetPaused(true);
    }
    public string formatTime(float time)
    {
        return (time < 10 ? "0" + time.ToString() : time.ToString());
    }
    public void Seconds_To_PreferedFormat()
    {
        /*  timerField_Hour.Text = (timer_Seconds / 60 / 60).ToString("F2");
          timerField_Minute.Text = (timer_Seconds / 60 ).ToString("F0");*/



        var minute = Mathf.Floor(timer_Seconds / 60) % 60;
        var hour = Mathf.Floor(timer_Seconds / 60 / 60) % 60;
        var seconds = Mathf.Floor(timer_Seconds) % 60;

        timer_Minutes = minute;
        timer_Hours = hour;
      


        timerNormalized = timer_Seconds / originalTimerAmount_Seconds;


        timerField_Minute.Text = formatTime(minute);
        timerField_Hour.Text = formatTime(hour);
        timerText_Seconds.text = formatTime(seconds);
        
    }
    public void ConvertTimerValue_ToSeconds_Final()
    {
        float hourConverted = Int32.Parse(timerField_Hour.Text);
        float minuteConverted = Int32.Parse(timerField_Minute.Text);

        //Watch for this with care
        /*if(hourConverted > 0 || minuteConverted > 0)*/ originalTimerAmount_Seconds = (hourConverted * 60) * 60 + (minuteConverted * 60);
    }
    public void ChangeTimerValue_Hour(string timerValue, EndEditReason reason)
    {
        timerText_Seconds.text = "00";
        if(indexOfLastChosenPreset_FromData <= DefaultTrackPresets.Count)
        {/*
            timerField_Hour.Text = formatTime(timer_Hours);

            NoticeText_Toggle(2f, "Timer change won't be saved", true, Color.red);
            return;*/
        }
        originalTimerAmount_Hours = Int32.Parse(timerValue);

        float hourConverted = Int32.Parse(timerField_Hour.Text);
        float minuteConverted = Int32.Parse(timerField_Minute.Text);

        SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].LengthOfTrack = (hourConverted * 60) * 60 + (minuteConverted * 60);
        SaveManager.Instance.SaveCall("TrackPresets");

        PlayerPrefs.SetFloat("SecondsTimerSave", (hourConverted * 60) * 60 + (minuteConverted * 60));


        timerValueWasChanged = true;

        timerField_Hour.Text = formatTime(hourConverted);
        timerField_Minute.Text = formatTime(minuteConverted);

        ConvertTimerValue_ToSeconds_Final();
        ResetTimer_InvokeExternalDelegates();
    }
    public void ChangeTimerValue_Minute(string timerValue, EndEditReason reason)
    {
        timerText_Seconds.text = "00";

        if (indexOfLastChosenPreset_FromData <= DefaultTrackPresets.Count)
        {
           /* timerField_Minute.Text = formatTime(timer_Minutes);

            NoticeText_Toggle(2f, "Timer change won't be saved", true, Color.red);
            return;*/
        }
        originalTimerAmount_Minutes = Int32.Parse(timerValue);

        float hourConverted = Int32.Parse(timerField_Hour.Text);
        float minuteConverted = Int32.Parse(timerField_Minute.Text);

        SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].LengthOfTrack = (hourConverted * 60) * 60 + (minuteConverted * 60);
        SaveManager.Instance.SaveCall("TrackPresets");

        PlayerPrefs.SetFloat("SecondsTimerSave", (hourConverted * 60) * 60 + (minuteConverted * 60));


        timerValueWasChanged = true;

        timerField_Hour.Text = formatTime(hourConverted);
        timerField_Minute.Text = formatTime(minuteConverted);

        ConvertTimerValue_ToSeconds_Final();
        ResetTimer_InvokeExternalDelegates();
    }
    void ToggleLoop_Local_PlayerPrefs()
    {
        bool _loop = default;
        if (PlayerPrefs.HasKey("TrackLoop")) _loop = PlayerPrefs.GetInt("TrackLoop") == 0 ? false : true;

        
        if (_loop)
        {
            loopImage.SetActive(true);
            noLoopImage.SetActive(false);
        }
        else
        {
            loopImage.SetActive(false);
            noLoopImage.SetActive(true);
        }

        loop = _loop;
    }
    public void ToggleLoop()
    {
        loop = !loop;

        int intIs = loop ? 1 : 0;
        PlayerPrefs.SetInt("TrackLoop", intIs);

        if (loop)
        {
            loopImage.SetActive(true);
            noLoopImage.SetActive(false);
        }
        else
        {
            loopImage.SetActive(false);
            noLoopImage.SetActive(true);
        }
    }
    public void ResetTrack()
    {
        PresetReselect_CurrentIndex();
    }
    public void ToggleTimer(bool Play)
    {
        play = Play;
    

        if (play)
        {
            /* if (paused) timeAtPlay = Time.time - differenceOfTimeAtPause;
             else timeAtPlay = Time.time;*/


            timerPlay.SetActive(false);
            timerPause.SetActive(true);

            foreach (var item in SoundBar)
            {
              if(!item.GetComponentInChildren<SoundBar>().SoundStatus_IsPlaying()) item.GetComponentInChildren<SoundBar>().Toggle_PlayPauseStop(0);
            }

            playTimer = true;

            timerField_Hour.interactable = false;
            timerField_Minute.interactable = false;

            timerField_Hour.ReadOnly = true;
            timerField_Minute.ReadOnly = true;

            if (!paused /*|| timerValueWasChanged*/)
            {
                ConvertTimerValue_ToSeconds_Final();
               // timer_Seconds = originalTimerAmount_Seconds;
                ResetTimer_InvokeExternalDelegates();
            }

           // timerValueWasChanged = false;

            mainTimer.SetPaused(false);
   
            mainTimer.Begin();
            paused = false;
        }
        else
        {
            /* differenceOfTimeAtPause = Time.time - timeAtPlay;*/

            timerPlay.SetActive(true);
            timerPause.SetActive(false);

            paused = true;

            foreach (var item in SoundBar) item.GetComponentInChildren<SoundBar>().Toggle_PlayPauseStop(1);

            playTimer = false;

            timerField_Hour.interactable = true;
            timerField_Minute.interactable = true;

            timerField_Hour.ReadOnly = false;
            timerField_Minute.ReadOnly = false;

            mainTimer.SetPaused(true);
        }
   }

    IEnumerator Vibrate(float vibrateInterval, int timesToVibrate)
    {
        while (timesVibrated < timesToVibrate)
        {
            timesVibrated++;
            Handheld.Vibrate();
            yield return new WaitForSecondsRealtime(vibrateInterval);
        }

       if(timesVibrated >= timesToVibrate)  timesVibrated = 0;
    }
    public void OnTimerFinish(bool _loop)
    {
        timer_Seconds = originalTimerAmount_Seconds;

        foreach (var item in SoundBar)
        {
            if (item.GetComponentInChildren<SoundBar>().SoundStatus_IsPlaying())
            {
                //Debug.Log("is playing");
                item.GetComponentInChildren<SoundBar>().Toggle_PlayPauseStop(2);
            }
        }

        timerField_Hour.interactable = true;
        timerField_Minute.interactable = true;

        timerField_Hour.ReadOnly = false;
        timerField_Minute.ReadOnly = false;


        Seconds_To_PreferedFormat();
        timerCountRing.fillAmount = timerNormalized;

        timerPlay.SetActive(true);
        timerPause.SetActive(false);

        timerText_Seconds.text = "00";


        playTimer = false;



        if (_loop)
        {
             StartCoroutine(Vibrate(2f, 3));


            //Was causing stackoverflow with the pause for some reason? So we need IEnumerator
            IEnumerator pauseBeforeReplay()
            {
                yield return new WaitForSecondsRealtime(0.1f);
                ToggleTimer(true);
            }

            StartCoroutine(pauseBeforeReplay());
        }
        
    }

    public void ResetValues_Timer(float TimerHours, float TimerMinutes, float TimerSeconds)
    {

        play = false;
        paused = false;
        
        playTimer = false;

        timerValueWasChanged = false;
        timerNormalized = 1;

         timer_Seconds = TimerSeconds;
         timer_Minutes = TimerMinutes;
         timer_Hours = TimerHours;

        Play_Button.SetActive(true);
        Stop_Button.SetActive(false);


        timerText_Seconds.text = formatTime(TimerSeconds);
        timerField_Hour.Text = formatTime(TimerHours);
        timerField_Minute.Text = formatTime(TimerMinutes);


        originalTimerAmount_Hours = TimerHours;
        originalTimerAmount_Minutes = TimerMinutes;
        originalTimerAmount_Seconds = TimerSeconds;
    }

    #endregion
    #region Mixer Methods



    public void BackgroundUI_Setup()
    {

        List<TMP_Dropdown.OptionData> backgroundOptions = new List<TMP_Dropdown.OptionData>();
        foreach (var item in Backgrounds)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData { image = item, text = item.name };
            backgroundOptions.Add(optionData);
        }
        MainBackgroundDropDown.ClearOptions();
        MainBackgroundDropDown.AddOptions(backgroundOptions);


        presetSavingMainParent.SetActive(true);

        presetBackgroundDropdown.ClearOptions();
        presetBackgroundDropdown.AddOptions(backgroundOptions);



        presetSavingMainParent.SetActive(false);

        MainBackgroundDropDown.value = 0;
        MainAppBackground.sprite = Backgrounds[0];
        BackgroundToggled1.sprite = Backgrounds[0];
        BackgroundToggled2.sprite = Backgrounds[0];
    }
    public void EstablishPresetValues_AND_UIValues()
    {
        bool presetTimerSave = PlayerPrefs.GetInt("TimerPerPreset") == 0 ? false : true;
        bool _defaultTimerValue = PlayerPrefs.GetInt("TimerDefaultValue") == 0 ? false : true;
        bool _hideQuickPitch = PlayerPrefs.GetInt("HideQuickPitch") == 0 ? false : true;

        saveTimerPerPreset.isOn = presetTimerSave;
        defaultTimerValue.isOn = _defaultTimerValue;
        hideQuickPitch.isOn = _hideQuickPitch;

        ToggleLoop_Local_PlayerPrefs();

        
        if (SaveManager.Instance.AccessDataStorage().Presets.Count > 0)
        {
            if (PlayerPrefs.HasKey("LastChosenPreset"))
            {              
      
                string LastChosenPreset = PlayerPrefs.GetString("LastChosenPreset");
          
             //   Debug.Log("howww" + LastChosenPreset);
               TrackPreset lastChosenPreset = SaveManager.Instance.AccessDataStorage().Presets.Where(i => i.PresetName == LastChosenPreset).FirstOrDefault();
          //      Debug.Log("howww2 " + lastChosenPreset.PresetName);
                //Can always turn this back into a tmp int storage if, saving as field var doesnt work...


                indexOfLastChosenPreset_FromData = SaveManager.Instance.AccessDataStorage().Presets.IndexOf(lastChosenPreset);
                
                string Append = indexOfLastChosenPreset_FromData <= DefaultTrackPresets.Count - 1 ? " (Default)" : "";

                if (lastChosenPreset != null)
                {
                    presetDateTime.text = lastChosenPreset.DateTimeCreated;
                    presetTitle.Text = lastChosenPreset.PresetName + Append;
                }

                for (int i = 0; i < SoundBar.Count; i++)
                {
                    float _Volume = SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].Sounds[i].Volume;
                    bool _Muted = SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].Sounds[i].Muted;
                    float _Pitch = SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].Sounds[i].Pitch;

                    int _SoundChosen = SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].Sounds[i].SoundChosen;

                    SoundBar[i].GetComponentInChildren<SoundBar>().InitializeKeyFields(_Volume, _Pitch, _SoundChosen, _Muted); //Possibly this line
                    SoundBar[i].GetComponentInChildren<SoundBar>().Toggle_PlayPauseStop(2);
                }

               
            }
        }

    }
    public void Initialize_UIValues(int backgroundChangeForCurrentPreset = 999)
    {

        //Main App Background Override by Background Selection (Must come after Preset Loading)


        if (PlayerPrefs.HasKey("LastChosenBackground"))
        {
            if (indexOfLastChosenPreset_FromData > (DefaultTrackPresets.Count - 1))
            {
                MainAppBackground.sprite = Backgrounds[PlayerPrefs.GetInt("LastChosenBackground")];
                BackgroundToggled1.sprite = Backgrounds[PlayerPrefs.GetInt("LastChosenBackground")];
                BackgroundToggled2.sprite = Backgrounds[PlayerPrefs.GetInt("LastChosenBackground")];

                MainBackgroundDropDown.value = PlayerPrefs.GetInt("LastChosenBackground");
            }
            else
            {
                MainAppBackground.sprite = Backgrounds[SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].Background];
                BackgroundToggled1.sprite = Backgrounds[SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].Background];
                BackgroundToggled2.sprite = Backgrounds[SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].Background];

                MainBackgroundDropDown.value = SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].Background;
            }
        }




            //Preset Selection Dropdown Fields
            if (SaveManager.Instance.AccessDataStorage().Presets.Count > 0)
        {
            List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
            int backgroundSelection = default;
            for (int i = 0; i < SaveManager.Instance.AccessDataStorage().Presets.Count; i++)
            {

                if (i == indexOfLastChosenPreset_FromData && backgroundChangeForCurrentPreset != 999) backgroundSelection = backgroundChangeForCurrentPreset;
                else backgroundSelection = SaveManager.Instance.AccessDataStorage().Presets[i].Background;


                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData() { text = SaveManager.Instance.AccessDataStorage().Presets[i].PresetName, image = Backgrounds[backgroundSelection] };
                
                optionDatas.Add(optionData);
            }
            presetSelection.ClearOptions();
            presetSelection.AddOptions(optionDatas);

            presetSelection.value = indexOfLastChosenPreset_FromData;

        }
    }
    public void InitializeReferences()
    {
        foreach(Transform item in SoundMixerParent)
        {
            SoundBar.Add(item.gameObject);
        }
    }
    public void Preset_EndEdit()
    {
        if (presetInputField.text.Length > 0 && !string.IsNullOrWhiteSpace(presetInputField.text))
        {
            if (SaveNewPreset(presetInputField.text, presetBackgroundDropdown.value, 1800))
            {
                presetTitle.Text = presetInputField.text;
                string Date = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year + "   " + DateTime.Now.ToString("h:mm tt");
                presetDateTime.text = Date;
            }


            presetSavingMainParent.SetActive(false);
        }
    }

    public void HideMixer_Toggle(bool on)
    {
        if (on)
        {
            BackgroundToggled1.enabled = true;
           Background1Toggle.transform.position = BackgroundToggled1Hidden.position;
        }
        else
        {
            BackgroundToggled1.enabled = false;
            Background1Toggle.transform.position = BackgroundToggled1Visible.position;
        }
    }


    public void HideAllUI_Toggle(bool on)
    {
        if (on)
        {
            BackgroundToggled2.enabled = true;
            Background2Toggle.transform.position = BackgroundToggled2Hidden.position;
        }
        else
        {
            BackgroundToggled2.enabled = false;
            Background2Toggle.transform.position = BackgroundToggled2Visible.position;
        }
    }

    public void ChangeBackground(int backgroundChoice)
    {
        if (AppLoaded)
        {
            PlayerPrefs.SetInt("LastChosenBackground", backgroundChoice);

            MainAppBackground.sprite = Backgrounds[backgroundChoice];
            BackgroundToggled1.sprite = Backgrounds[backgroundChoice];
            BackgroundToggled2.sprite = Backgrounds[backgroundChoice];


            if (indexOfLastChosenPreset_FromData <= DefaultTrackPresets.Count - 1) Save_An_UpdatedPreset();
            else
            {
                //Just using this block of code to bypass a chunk of code inside of "Save_An_UpdatedPreset()", and avoid setting soundbars again...
                if (SaveManager.Instance.AccessDataStorage().Presets.Count >= indexOfLastChosenPreset_FromData + 1)
                {
                    SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].Background = backgroundChoice;

                    Initialize_UIValues(backgroundChoice);

                    SaveManager.Instance.SaveCall("TrackPresets");
                }
            }

        }
    }

    public void PresetSelection(int preset)
    {
        timerField_Hour.interactable = true;
        timerField_Minute.interactable = true;

        timerField_Hour.ReadOnly = false;
        timerField_Minute.ReadOnly = false;


        timerText_Seconds.text = "00";
        timerCountRing.fillAmount = 1;
        mainTimer.End();





        if (AppLoaded)
        { 
            if (SaveManager.Instance.AccessDataStorage().Presets.Count > 0) PlayerPrefs.SetString("LastChosenPreset", SaveManager.Instance.AccessDataStorage().Presets[preset].PresetName);
            EstablishPresetValues_AND_UIValues();

            MainAppBackground.sprite = Backgrounds[SaveManager.Instance.AccessDataStorage().Presets[preset].Background];
            BackgroundToggled1.sprite = Backgrounds[SaveManager.Instance.AccessDataStorage().Presets[preset].Background];
            BackgroundToggled2.sprite = Backgrounds[SaveManager.Instance.AccessDataStorage().Presets[preset].Background];



            MainBackgroundDropDown.value = SaveManager.Instance.AccessDataStorage().Presets[preset].Background;
        }


        if (PlayerPrefs.GetInt("TimerDefaultValue") == 1)
        {
            var minute = Mathf.Floor(DefaultTimerValue / 60) % 60;
            var hour = Mathf.Floor(DefaultTimerValue / 60 / 60) % 60;
            var second = Mathf.Floor(DefaultTimerValue) % 60;

            ResetValues_Timer(hour, minute, second);
        }
        else if (PlayerPrefs.GetInt("TimerPerPreset") == 1)
        {
            TrackPreset track = SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData];

            var minute = Mathf.Floor(track.LengthOfTrack / 60) % 60;
            var hour = Mathf.Floor(track.LengthOfTrack / 60 / 60) % 60;
            var second = Mathf.Floor(track.LengthOfTrack) % 60;


            ResetValues_Timer(hour, minute, second);
        }
        else
        {
            if (!PlayerPrefs.HasKey("SecondsTimerSave")) ResetValues_Timer(originalTimerAmount_Hours, originalTimerAmount_Minutes, originalTimerAmount_Seconds);
            else
            {
                float savedSeconds = PlayerPrefs.GetFloat("SecondsTimerSave");

                var minute = Mathf.Floor(savedSeconds / 60) % 60;
                var hour = Mathf.Floor(savedSeconds / 60 / 60) % 60;
                var second = Mathf.Floor(savedSeconds) % 60;

                ResetValues_Timer(hour, minute, second);

           
            }
        }
    }

    public void DeletePreset_Prompt_Toggle(bool boolis)
    {
        int val = boolis ? 0 : 1;
        PlayerPrefs.SetInt("DeletePresetPromptOn", val);
    }
    public void DeletePreset_Prompt()
    {
        if (PlayerPrefs.GetInt("DeletePresetPromptOn") == 1) // 0 is false, 1 is true
        {
            DeletePresetPrompt.SetActive(true); 
        }
        else DeletePreset();
    }
    public void DeletePreset()
    {
        if (indexOfLastChosenPreset_FromData <= DefaultTrackPresets.Count - 1)
        {
            StartCoroutine(NoticeText_Toggle(2f, "Can't delete Default Presets", true, Color.red));

            //Pop Up Notice
            GameObject popUp = Instantiate(Resources.Load<GameObject>("PopUpNotice"));
            popUp.GetComponent<PopUpMessage>().SetMessage("Can't delete Default Presets", 2f, "NOTICE");

            return;
        }

        SaveManager.Instance.AccessDataStorage().Presets.RemoveAt(indexOfLastChosenPreset_FromData);
        SaveManager.Instance.SaveCall("TrackPresets");

        PresetSelection(indexOfLastChosenPreset_FromData - 1);
        Initialize_UIValues();

        NoticeText_Toggle(2f, $"Sucesfully deleted {presetTitle.Text} preset", true, Color.green);

    }

    
    //USE EVENT TRIGGERS LIKE THIS, in the FUTURE!
     public void OnPointerClick_ScrollDropDown_ToSelection(string data)
     {
       // Debug.Log("USda");

         GameObject ddl = GameObject.Find("Dropdown List");
         GameObject vp = ddl.GetComponentsInChildren<RectTransform>()[1].gameObject;
         GameObject content = vp.GetComponentsInChildren<RectTransform>()[1].gameObject;

         RectTransform rt = content.GetComponent<RectTransform>();

        int index = presetSelection.value;
         // 75 is the height of an item in my dropdown
         rt.position = rt.position + Vector3.up * index * 75;
     }
    
    void PresetReselect_CurrentIndex()
    {
        timerCountRing.fillAmount = 1;
        int indexToPickDef = indexOfLastChosenPreset_FromData;
        int indexToPick = default;

        if (indexOfLastChosenPreset_FromData > 0) indexToPick = indexOfLastChosenPreset_FromData - 1;
        else indexToPick = indexOfLastChosenPreset_FromData + 1;

        PresetSelection(indexToPick);
        PresetSelection(indexToPickDef);
    }

    public IEnumerator NoticeText_Toggle(float time, string message, bool flashText, Color color)
    {
        noticeText.color = color;

        noticeText.text = message;

        if (flashText)
        {
            yield return new WaitForSecondsRealtime(0.25f);
            noticeText.text = "";
            yield return new WaitForSecondsRealtime(0.25f);
            noticeText.text = message;
           yield return new WaitForSecondsRealtime(0.25f);
            noticeText.text = "";
            yield return new WaitForSecondsRealtime(0.25f);
            noticeText.text = message;
        }
        yield return new WaitForSecondsRealtime(time);
        noticeText.text = "";
    }
    public void UpdatePresetName()
    {
        if (indexOfLastChosenPreset_FromData <= (DefaultTrackPresets.Count - 1))
        {
            presetTitle.Text = DefaultTrackPresets[indexOfLastChosenPreset_FromData].PresetName + " (Default)";


            StartCoroutine(NoticeText_Toggle(3f, "Can't rename default presets!", true, Color.red));

            return;
        }

        TrackPreset currentPreset = SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData];
        bool nameAlreadyExists = SaveManager.Instance.AccessDataStorage().Presets.Where(i => i.PresetName == presetTitle.Text).Where(j => j != currentPreset).Any();


        IEnumerator checkValidName()
        {
            if (string.IsNullOrWhiteSpace(presetTitle.Text))
            {
                //Pop Up Notice
                GameObject popUp = Instantiate(Resources.Load<GameObject>("PopUpNotice"));
                popUp.GetComponent<PopUpMessage>().SetMessage("Please provide a Proper Name!", 5f, "NOTICE");

                StartCoroutine(NoticeText_Toggle(2f, "Please provide a Proper Name!", true, Color.red));

                yield return new WaitForSecondsRealtime(1f);
                presetTitle.Select();
            }
            else if (nameAlreadyExists)
            {
                //Pop Up Notice
                GameObject popUp2 = Instantiate(Resources.Load<GameObject>("PopUpNotice"));
                popUp2.GetComponent<PopUpMessage>().SetMessage("This name already exits, try a different name!", 5f, "NOTICE");

                StartCoroutine(NoticeText_Toggle(2f, "This name already exits, try a different name!", true, Color.red));

                presetTitle.Text = "";
                yield return new WaitForSecondsRealtime(1f);
                presetTitle.Select();
            }
            else 
            {

                PlayerPrefs.SetString("LastChosenPreset", presetTitle.Text);

                SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].PresetName = presetTitle.Text;

                SaveManager.Instance.SaveCall("TrackPresets");

                presetSelection.options[indexOfLastChosenPreset_FromData].text = presetTitle.Text;

                presetSelection.value = indexOfLastChosenPreset_FromData;
                presetSelection.RefreshShownValue();
            }
        }
        StartCoroutine(checkValidName());
    }
    public void Save_An_UpdatedPreset()
    {

        Debug.Log("ehre");

        if (indexOfLastChosenPreset_FromData <= (DefaultTrackPresets.Count - 1))
        {
            StartCoroutine(NoticeText_Toggle(1f, "Changes to Default Presets won't be Saved!", false, Color.red));
      
            return;
        }

        if (SaveManager.Instance.AccessDataStorage().Presets.Count > 0) {

            List<TrackPresetData.Sound> sounds = new List<TrackPresetData.Sound>();

            foreach(var item in SoundBar)
            {
                SoundBar soundBar = item.GetComponentInChildren<SoundBar>();
                TrackPresetData.Sound sound = new TrackPresetData.Sound();

                sound.Muted = soundBar.Muted;
                sound.Pitch = soundBar.Pitch;
                sound.SoundChosen = soundBar.SoundChosen;
                sound.Volume = soundBar.Volume;

                sounds.Add(sound);
            }




            // float TimeAmount = originalTimerAmount_Seconds <= 0 ? 0 : originalTimerAmount_Seconds;
            float TimeAmount = default;

            if (PlayerPrefs.GetInt("TimerPerPreset") == 0) TimeAmount = originalTimerAmount_Seconds;
            else  TimeAmount = SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].LengthOfTrack;

            string name = indexOfLastChosenPreset_FromData <= (DefaultTrackPresets.Count - 1) ?
                SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].PresetName : presetTitle.Text;

            int background = indexOfLastChosenPreset_FromData <= (DefaultTrackPresets.Count - 1) ? SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData].Background : PlayerPrefs.GetInt("LastChosenBackground");

            TrackPreset trackPreset = new TrackPreset { Sounds = sounds, Background = background, DateTimeCreated = presetDateTime.text,
            LengthOfTrack = TimeAmount, PresetName = name };

            SaveManager.Instance.AccessDataStorage().Presets[indexOfLastChosenPreset_FromData] = trackPreset;
            SaveManager.Instance.SaveCall("TrackPresets");

           }
    }



    public void SaveDefaultPresets(List<TrackPreset> trackPresetsToSave)
    {
        List<TrackPreset> trackPresetsLocal = new List<TrackPreset>();

        foreach (var item in trackPresetsToSave)
        {
            string Date = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year + "   " + DateTime.Now.ToString("h:mm tt");
            item.DateTimeCreated = Date;

            trackPresetsLocal.Add(item);
        }


         SaveManager.Instance.AccessDataStorage().Presets = trackPresetsLocal;


         List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
         
         for (int i = 0; i < SaveManager.Instance.AccessDataStorage().Presets.Count; i++)
         {
             TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData() { text = SaveManager.Instance.AccessDataStorage().Presets[i].PresetName, image = Backgrounds[SaveManager.Instance.AccessDataStorage().Presets[i].Background] };
             optionDatas.Add(optionData);
         }
         
         presetSelection.AddOptions(optionDatas);

        
        presetSelection.value = 0;
        indexOfLastChosenPreset_FromData = 0;

        PlayerPrefs.SetString("LastChosenPreset", "Winter Warm");
        SaveManager.Instance.SaveCall("TrackPresets");
    }

    public bool SaveNewPreset(string Title, int Background, float LengthOfTrack)
    {
        bool sucessfullySaved = false;

        TrackPreset PresetMatch = SaveManager.Instance.AccessDataStorage().Presets.Where(i => i.PresetName == Title).FirstOrDefault();
        int indexOf = PresetMatch == null ? 999 : SaveManager.Instance.AccessDataStorage().Presets.IndexOf(PresetMatch);


        if (indexOf <= DefaultTrackPresets.Count - 1 && PresetMatch != null)
        {
            GameObject popUp0 = Instantiate(Resources.Load<GameObject>("PopUpNotice"));
            popUp0.GetComponent<PopUpMessage>().SetMessage("Can't Over-write Presets", 2f, "NOTICE");

            StartCoroutine(NoticeText_Toggle(3f, "Can't Over-write Presets", true, Color.red));

            sucessfullySaved = false;
            return sucessfullySaved;
        }

        TrackPreset trackPreset = new TrackPreset();
        List<TrackPresetData.Sound> sounds = new List<TrackPresetData.Sound>();

        foreach (GameObject item in SoundBar)
        {
            bool _muted = item.GetComponentInChildren<SoundBar>().Muted;
            float _volume = item.GetComponentInChildren<SoundBar>().Volume;
            float _pitch = item.GetComponentInChildren<SoundBar>().Pitch;
            int _soundChosen = item.GetComponentInChildren<SoundBar>().SoundChosen;

            sounds.Add(new TrackPresetData.Sound { Muted = _muted, Volume = _volume, Pitch = _pitch, SoundChosen = _soundChosen });
        }

        string Date = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year + "   " + DateTime.Now.ToString("h:mm tt");
        trackPreset.DateTimeCreated = Date;

        trackPreset.PresetName = Title;
        trackPreset.Sounds = sounds;
        trackPreset.LengthOfTrack = LengthOfTrack;
        trackPreset.Background = Background;


        //This block of code caused major bug because when we change the value, the callback function of Change Background gets called before
        //the Index Update occurs, causing many bugs, but keeping it around incase other unseen bugs pop up
/*
        MainAppBackground.sprite = Backgrounds[Background];
        MainBackgroundDropDown.value = Background;
*/
        PlayerPrefs.SetString("LastChosenPreset", Title);


        TrackPreset track = SaveManager.Instance.AccessDataStorage().Presets.Where(s => s.PresetName == trackPreset.PresetName).FirstOrDefault();

        bool presetOverWritten = false;
        if (track == null) SaveManager.Instance.AccessDataStorage().Presets.Add(trackPreset);
        else
        {

            track.DateTimeCreated = Date;

            //track.PresetName = Title;
            track.Sounds = sounds;
            track.LengthOfTrack = LengthOfTrack;
            track.Background = Background;

            SaveManager.Instance.AccessDataStorage().Presets[SaveManager.Instance.AccessDataStorage().Presets.IndexOf(track)] = track;


            presetOverWritten = true;
        }


        List<string> presetNames = new List<string>();

        foreach (var item in presetSelection.options) presetNames.Add(item.text);

        if (!presetNames.Contains(Title))
        {
            presetSelection.ClearOptions();

            List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();

            for (int i = 0; i < SaveManager.Instance.AccessDataStorage().Presets.Count; i++)
            {
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData() { text = SaveManager.Instance.AccessDataStorage().Presets[i].PresetName, image = Backgrounds[SaveManager.Instance.AccessDataStorage().Presets[i].Background] };
                optionDatas.Add(optionData);
            }

            presetSelection.AddOptions(optionDatas);
            presetNames.Add(Title);
        }
        else
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData() { text = Title, image = Backgrounds[Background] };
            presetSelection.options[presetNames.IndexOf(Title)] = optionData;
        }

        presetSelection.value = presetNames.IndexOf(Title);
        indexOfLastChosenPreset_FromData = presetSelection.value;

        SaveManager.Instance.SaveCall("TrackPresets");

        //Pop Up Notice

      
        GameObject popUp = Instantiate(Resources.Load<GameObject>("PopUpNotice"));


        string messageIs = presetOverWritten == false ? "Saved New Preset!" : "Preset Overwritten!";
        popUp.GetComponent<PopUpMessage>().SetMessage(messageIs.ToString(), 2f, "NOTICE");

        Color colorIs = presetOverWritten == false ? Color.green : Color.green;
      
        StartCoroutine(NoticeText_Toggle(2f, messageIs, true, colorIs));

        sucessfullySaved = true;
        return sucessfullySaved;
    }
    public void SetSoundBar_Tweaking(int barId)
    {
        CurrentSoundBar_Tweaking_ID = barId;

        float vol = SoundBar[CurrentSoundBar_Tweaking_ID].GetComponentInChildren<SoundBar>().Volume;
        float pitch = SoundBar[CurrentSoundBar_Tweaking_ID].GetComponentInChildren<SoundBar>().Pitch;
        bool muted = SoundBar[CurrentSoundBar_Tweaking_ID].GetComponentInChildren<SoundBar>().Muted;

       // Debug.Log(muted);
        soundDetailsControlPanel.SetValues_ToUseWithUI(vol, pitch, muted);
        soundDetailsControlPanel.transform.parent.gameObject.SetActive(true);
    }
    public void ToggleMuteButton(bool muted)
    {
        if(muted)
        {
            MuteButton_SoundControlPanel.transform.GetChild(0).gameObject.SetActive(true);
            MuteButton_SoundControlPanel.transform.GetChild(1).gameObject.SetActive(false);

        }
        else
        {
            MuteButton_SoundControlPanel.transform.GetChild(0).gameObject.SetActive(false);
            MuteButton_SoundControlPanel.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void Change_Volume(float vol)
    {
        
        SoundBar[CurrentSoundBar_Tweaking_ID].GetComponentInChildren<SoundBar>().Change_Volume(vol);

        if (vol <= 0) ToggleMuteButton(true);
        else ToggleMuteButton(false);
    }
    public void Change_Pitch(float pitch)
    {
       SoundBar[CurrentSoundBar_Tweaking_ID].GetComponentInChildren<SoundBar>().Change_Pitch(pitch);
    }
    public void Change_Mute()
    {
        SoundBar[CurrentSoundBar_Tweaking_ID].GetComponentInChildren<SoundBar>().Change_Mute();
   
        //For when the sound control panel is already open
        if (SoundBar[CurrentSoundBar_Tweaking_ID].GetComponentInChildren<SoundBar>().Muted)
        {
            ToggleMuteButton(true);
            SoundBar[CurrentSoundBar_Tweaking_ID].GetComponentInChildren<SoundBar>().ToggleMuteButton(true);
        }
        else
        {
            ToggleMuteButton(false);
            SoundBar[CurrentSoundBar_Tweaking_ID].GetComponentInChildren<SoundBar>().ToggleMuteButton(false);
        }
    }


    #endregion
    #region  Settings
    public void toggleSettingsMenu()
    {
        settingsOpen = !settingsOpen;

        settingsPanel.SetActive(settingsOpen);  
    }

    public void SaveTimerPerPreset(bool boolis)
    {
        int presetTimerSave = boolis ? 1 : 0;
        PlayerPrefs.SetInt("TimerPerPreset", presetTimerSave);

       
        saveTimerPerPreset.isOn = boolis;

        if(PlayerPrefs.GetInt("TimerPerPreset") == 1 && PlayerPrefs.GetInt("TimerDefaultValue") == 1)
        {
            PlayerPrefs.SetInt("TimerPerPreset", 0);
            PlayerPrefs.SetInt("TimerDefaultValue", 0);

            saveTimerPerPreset.isOn = false;
            defaultTimerValue.isOn = false;
        }
    }

    public void TimerDefaultValue_Toggle(bool boolis)
    {
        int timerDef = boolis ? 1 : 0;
        PlayerPrefs.SetInt("TimerDefaultValue", timerDef);


        defaultTimerValue.isOn = boolis;


        if (PlayerPrefs.GetInt("TimerPerPreset") == 1 && PlayerPrefs.GetInt("TimerDefaultValue") == 1)
        {
            PlayerPrefs.SetInt("TimerPerPreset", 0);
            PlayerPrefs.SetInt("TimerDefaultValue", 0);

            saveTimerPerPreset.isOn = false;
            defaultTimerValue.isOn = false;

            return;
        }

        if(boolis) PresetReselect_CurrentIndex();

    }

    public void ResetSettings()
    {
        PlayerPrefs.SetInt("DeletePresetPromptOn", 1);

        // Causes a bug when i use this line...
        //PlayerPrefs.SetString("LastChosenPreset", "Winter Solace"); 

        PlayerPrefs.SetInt("TimerPerPreset", 0);
        PlayerPrefs.SetInt("TimerDefaultValue", 0);
        PlayerPrefs.SetInt("HideQuickPitch", 0);


        resetSettings.isOn = false;
        deletePresetPromptToggle.isOn = false;
        hideQuickPitch.isOn = false;


        StartCoroutine(NoticeText_Toggle(3f, "Settings Reset!", true, Color.green));

        //Pop Up Notice
        GameObject popUp = Instantiate(Resources.Load<GameObject>("PopUpNotice"));
        popUp.GetComponent<PopUpMessage>().SetMessage("Settings Reset!", 3f, "NOTICE");
  
    }

    public void ExitApp() => Application.Quit();


    #endregion



    void Awake()
    {

      // PlayerPrefs.DeleteAll();

        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);


        InitializeReferences();    
    }
    IEnumerator Start()
    {
        if(AppNeverSleep) Screen.sleepTimeout = SleepTimeout.NeverSleep;


        yield return new WaitForSecondsRealtime(0.2f);

        BackgroundUI_Setup();
        
        if (!PlayerPrefs.HasKey("FirstLoad")) //0 is true. 1 is false
        {
            int timerAmount = timerField_Hour.Text.Length <= 0 ? 25 : Int32.Parse(timerField_Hour.Text);
            // SaveNewPreset("Winter Solace", 0, timerAmount);
            SaveDefaultPresets(DefaultTrackPresets);

            PlayerPrefs.SetInt("DeletePresetPromptOn", 1);
            deletePresetPromptToggle.isOn = false;
        }

        EstablishPresetValues_AND_UIValues();
        

        if (PlayerPrefs.HasKey("FirstLoad")) Initialize_UIValues();


        if (!PlayerPrefs.HasKey("FirstLoad")) PlayerPrefs.SetInt("FirstLoad", 1);

        if (indexOfLastChosenPreset_FromData <= 0)
        {
            presetSelection.value = DefaultTrackPresets.Count - 1;
            presetSelection.value = indexOfLastChosenPreset_FromData;
        }

        AppLoaded = true;
        OnAppStart?.Invoke(this, EventArgs.Empty);


        //Impliments sound changes on Audio Sources of all bars "LAZY CODING" (Must HAVE)
        yield return new WaitForSecondsRealtime(0.1f);
        PresetSelection(indexOfLastChosenPreset_FromData);



        yield return new WaitForSecondsRealtime(9f);
        FramesManager.Instance.ToggleFrameIntervalInstant(true);
    }



    void Update()
    {

        /*
         //Timing is still exactly same using frames minimilzing
         if(playTimer)
         {
             timer_Seconds = (originalTimerAmount_Seconds - (Time.time - timeAtPlay));

             if (timer_Seconds <= 0) OnTimerFinish(loop);

             //timer_Seconds -= Time.time;
             timerCountRing.fillAmount = timerNormalized;

             Seconds_To_PreferedFormat();

           //  if (timer_Seconds <= 0) OnTimerFinish(loop);
         }
        */


         if(Input.GetMouseButtonDown(0)) FramesManager.Instance.ResetFrameToggle();

 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        settingsPanel.SetActive(false);
    }
}
