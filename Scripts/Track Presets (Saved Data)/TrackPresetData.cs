using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

[System.Serializable]
public class TrackPresetData
{

    [System.Serializable]
    public struct TrackStruct
    {
        public string PresetName;

        public string DateTimeCreated;
        public float LengthOfTrack;

        public int Background;

        public bool Loop;

        public List<Sound> Sounds;
    }

    [System.Serializable]
    public struct Sound
    {
        public bool Muted;

        public float Volume, Pitch;
        public int SoundChosen;
    }


    //This/Tracks is what gets stored...not the structs themselves
    public List<TrackStruct> Tracks = new List<TrackStruct>(); 

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public void FromJson(string a_Json)
    {
        JsonConvert.PopulateObject(a_Json, this);
    }

}



public interface IDatable
{
    void PopulateData(TrackPresetData trackPresetData, SettingsData settingsData, string FileToWriteTo);
    void UnloadData(TrackPresetData trackPresetData, SettingsData settingsData, string FileToReadFrom);
}