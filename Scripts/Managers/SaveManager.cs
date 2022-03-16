using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour, IDatable
{
    static public SaveManager Instance;
    DataStorage dataStorage;
   // public NotesMainScreen notesMainScreen;


    IEnumerable<IDatable> ObjsWithIDatable;

    public bool Save;
    public bool Load;


    //Both these bools serve two diff use cases.
    [Tooltip("For this, make sure you either have objects in question add themselves to DataStorage (best? EcoSystem Manager like), or do it from a controlling upper class")]
    public bool SaveDataFromStorageWithOwnIDatable;      //Practical for objects that will be regenerated, spawned during scene
    public bool SaveDataFromAllObjectsWithIDatable;      //Practical for objects that are premade for the scene


    public bool OverwriteLastSaveFile;
    public bool LoadMostRecentVersion;
    [Header("Only for use if load most recent version is not enabled, exact spelling matters, add array later")]
    public string SpecificFileToLoad;




    //if.....SaveDataFromStorageWithOwnIDatable
    public void PopulateData(TrackPresetData trackData, SettingsData settingsData, string FileToWriteTo)
    {
        if (SaveDataFromStorageWithOwnIDatable)
        {
            if (FileToWriteTo == "TrackPresets")
            {
                foreach (var item in dataStorage.Presets)
                {
                    if (item != null)
                    {
                        TrackPresetData.TrackStruct data = new TrackPresetData.TrackStruct();

                        data.Sounds = item.Sounds;

                        data.Loop = item.Loop;
                        data.PresetName = item.PresetName;
                        data.DateTimeCreated = item.DateTimeCreated;
        
                        data.LengthOfTrack = item.LengthOfTrack;

                        data.Background = item.Background;
                      

                        trackData.Tracks.Add(data);
                    }
                }
            }

       
            if (FileToWriteTo == "Settings")
            {
                 SettingsData.Settings settings = new SettingsData.Settings();

/*
                settings.AutoCorrect = dataStorage.Settings.AutoCorrect;
                settings.ChecklistMode = dataStorage.Settings.ChecklistMode;

                settings.DateOnlyOnSegmentLabels = dataStorage.Settings.DateOnlyOnSegmentLabels;
                settings.UpdateDateTimeOnNoteSegmentEdits = dataStorage.Settings.UpdateDateTimeOnNoteSegmentEdits;
                settings.DontShowSecondsOnTimeLabels = dataStorage.Settings.DontShowSecondsOnTimeLabels;

                settings.EnableFontSizeVarietyOnSegments = dataStorage.Settings.EnableFontSizeVarietyOnSegments;

                settings.SegmentSpacingBasedOnFontSize = dataStorage.Settings.SegmentSpacingBasedOnFontSize;
                settings.OpenNoteAtFirstSegment = dataStorage.Settings.OpenNoteAtFirstSegment;



                settings.BodyTextBold = dataStorage.Settings.BodyTextBold;
                settings.DateTimeTextBold = dataStorage.Settings.DateTimeTextBold;

                settings.BodyFontSize = dataStorage.Settings.BodyFontSize;
                settings.DateTimeFontSize = dataStorage.Settings.DateTimeFontSize;
                settings.EnableNoteLines = dataStorage.Settings.EnableNoteLines;


                settings.FontStyleNumber = dataStorage.Settings.FontStyleNumber;
               
                settings.BackgroundNumber = dataStorage.Settings.BackgroundNumber;

                settingsData._Settings = settings;*/

            }
        }    
    }

    public void UnloadData(TrackPresetData trackData, SettingsData settingsData, string FileToReadFrom)
    {
        if(SaveDataFromStorageWithOwnIDatable)
        {
            if (FileToReadFrom == "TrackPresets")
            {
                List<TrackPreset> trackPresets = new List<TrackPreset>();

                foreach (var item in trackData.Tracks)
                {
                    trackPresets.Add(new TrackPreset() { Loop = item.Loop, Sounds = item.Sounds, Background = item.Background, PresetName = item.PresetName, DateTimeCreated = item.DateTimeCreated, LengthOfTrack = item.LengthOfTrack });    
                }

                dataStorage.Presets = trackPresets;
            }

            if (FileToReadFrom == "Settings")
            {
                dataStorage.Settings = new Settings() {

                 /*   AutoCorrect = settingsData._Settings.AutoCorrect,
                    ChecklistMode = settingsData._Settings.ChecklistMode,
                    DateOnlyOnSegmentLabels = settingsData._Settings.DateOnlyOnSegmentLabels,
                    UpdateDateTimeOnNoteSegmentEdits = settingsData._Settings.UpdateDateTimeOnNoteSegmentEdits,
                    DontShowSecondsOnTimeLabels = settingsData._Settings.DontShowSecondsOnTimeLabels,

                    EnableFontSizeVarietyOnSegments = settingsData._Settings.EnableFontSizeVarietyOnSegments,

                    SegmentSpacingBasedOnFontSize = settingsData._Settings.SegmentSpacingBasedOnFontSize,
                    OpenNoteAtFirstSegment = settingsData._Settings.OpenNoteAtFirstSegment,



                    BodyTextBold = settingsData._Settings.BodyTextBold,
                    DateTimeTextBold = settingsData._Settings.DateTimeTextBold,
                    

                    BodyFontSize = settingsData._Settings.BodyFontSize,
                    DateTimeFontSize = settingsData._Settings.DateTimeFontSize,

                    //Aesthetics

                    EnableNoteLines = settingsData._Settings.EnableNoteLines,



                    FontStyleNumber = settingsData._Settings.FontStyleNumber,
                    BackgroundNumber = settingsData._Settings.BackgroundNumber
*/
            };

             /*   SettingsScreen.Instance.AutoCorrect = settingsData._Settings.AutoCorrect;
                SettingsScreen.Instance.ChecklistMode = settingsData._Settings.ChecklistMode;

                SettingsScreen.Instance.DateOnlyOnSegmentLabels = settingsData._Settings.DateOnlyOnSegmentLabels;
                SettingsScreen.Instance.UpdateDateTimeOnNoteSegmentEdits = settingsData._Settings.UpdateDateTimeOnNoteSegmentEdits;
                SettingsScreen.Instance.DontShowSecondsOnTimeLabels = settingsData._Settings.DontShowSecondsOnTimeLabels;

                SettingsScreen.Instance.EnableFontSizeVarietyOnSegments = settingsData._Settings.EnableFontSizeVarietyOnSegments;

                SettingsScreen.Instance.SegmentSpacingBasedOnFontSize = settingsData._Settings.SegmentSpacingBasedOnFontSize;
                SettingsScreen.Instance.OpenNoteAtFirstSegment = settingsData._Settings.OpenNoteAtFirstSegment;

                SettingsScreen.Instance.BodyTextBold = settingsData._Settings.BodyTextBold;
                SettingsScreen.Instance.DateTimeTextBold = settingsData._Settings.DateTimeTextBold;

                
                SettingsScreen.Instance.BodyFontSize = settingsData._Settings.BodyFontSize;
                SettingsScreen.Instance.DateTimeFontSize = settingsData._Settings.DateTimeFontSize;

                SettingsScreen.Instance.EnableNoteLines = settingsData._Settings.EnableNoteLines;

                SettingsScreen.Instance.FontStyleNumber = settingsData._Settings.FontStyleNumber;
                SettingsScreen.Instance.FontStyleNumberPrevious = settingsData._Settings.FontStyleNumber;
                SettingsScreen.Instance.BackgroundNumber = settingsData._Settings.BackgroundNumber;

                SettingsScreen.Instance.IntializeCurrentSavedSettingsValues();*/
            }
        
        }

    }

    public void SaveCall(string SaveFile)
    {
            if (SaveDataFromAllObjectsWithIDatable)
            {
                ObjsWithIDatable = FindObjectsOfType<MonoBehaviour>().OfType<IDatable>();

                /*              DataManager.SaveJsonData(ObjsWithIDatable, "Notes");
                              DataManager.SaveJsonData(ObjsWithIDatable, "Settings");*/

                DataManager.SaveJsonData(ObjsWithIDatable, SaveFile);
            }

            else
            {
            /* DataManager.SaveJsonData(new[] { this }, "Notes");
             DataManager.SaveJsonData(new[] { this }, "Settings");*/

              DataManager.SaveJsonData(new[] { this }, SaveFile);
            }

           Debug.Log("Save Completed");
    }

    public DataStorage AccessDataStorage()
    {
         return dataStorage;
    }
 
    private void Awake()
    {


        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        dataStorage = new DataStorage();


        Load = true;
    }

    private void Update()
    {
        if (SaveDataFromStorageWithOwnIDatable)
        {
            if (SaveDataFromAllObjectsWithIDatable) SaveDataFromAllObjectsWithIDatable = false;
        }
        else if(SaveDataFromAllObjectsWithIDatable)
        {
            if (SaveDataFromStorageWithOwnIDatable) SaveDataFromStorageWithOwnIDatable = false;
        }



        if (Save)
        {
            Save = false;

            if (SaveDataFromAllObjectsWithIDatable)
            {
                ObjsWithIDatable = FindObjectsOfType<MonoBehaviour>().OfType<IDatable>();

                DataManager.SaveJsonData(ObjsWithIDatable, "TrackPresets");
                DataManager.SaveJsonData(ObjsWithIDatable, "Settings");
            }

            else
            {
                DataManager.SaveJsonData(new[] { this }, "TrackPresets");
                DataManager.SaveJsonData(new[] { this }, "Settings");
            }

            Debug.Log("Save Completed");

        }
        if (Load)
        {
            if (SaveDataFromAllObjectsWithIDatable)
            {
                ObjsWithIDatable = FindObjectsOfType<MonoBehaviour>().OfType<IDatable>();

                DataManager.LoadJsonData(ObjsWithIDatable, "TrackPresets");
                DataManager.LoadJsonData(ObjsWithIDatable, "Settings");
            }

            else
            {
                DataManager.LoadJsonData(new[] { this }, "TrackPresets");
                DataManager.LoadJsonData(new[] { this }, "Settings");
            }

            Load = false;
            //Debug.Log("Load Completed");
        }

        if (dataStorage == null)
        {
            //Debug.Log("data storage is null");
            dataStorage = new DataStorage();
        }

    }



}


//DELETABLE:   Sample classes that can be used with DataManager.Serialize and DataManager.Deserialize if wanted 
public class Player
{
    public enum PlayerTraits
    {
        Strong = 20,
        Weak = 10
    }

    public int Health;
    public PlayerTraits Attributes;
    public Dictionary<string, int> FoodPrefs;
}
public class Enemy
{
    public enum EnemyTraits
    {
        Dark = 20,
        Light = 10

    }
    public int Health;
    public EnemyTraits Attributes;
    public Dictionary<string, int> FoodPrefs;
}


