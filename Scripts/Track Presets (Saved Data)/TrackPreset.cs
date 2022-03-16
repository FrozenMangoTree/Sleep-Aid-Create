using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Pretty sure we have a Track Preset Class and a TrackPresetData because it's easier to extract and use the data from a class than it is to do it from a struct
//and then the Data class is purely for storage purposes. I don't think we can store a class so we store a struct instead. So Track PReset class is for app
//use purposes, data is purely for storage so need the conversion


[System.Serializable]
public class TrackPreset
{
    public string PresetName;

    public string DateTimeCreated;
    public float LengthOfTrack;

    public int Background;

    public bool Loop;

    public List<TrackPresetData.Sound> Sounds;
}


