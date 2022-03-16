using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

[Obsolete]
[System.Serializable]
public class SettingsData
{
    [System.Serializable]
    public struct Settings
    {

     /*   public bool AutoCorrect;
        public bool ChecklistMode;

        public bool UpdateDateTimeOnNoteSegmentEdits;
        public bool DateOnlyOnSegmentLabels;
        public bool DontShowSecondsOnTimeLabels;

        public bool EnableFontSizeVarietyOnSegments;

        public bool SegmentSpacingBasedOnFontSize;
        public bool OpenNoteAtFirstSegment; 

        public bool BodyTextBold;
        public bool DateTimeTextBold;

        public int BodyFontSize;
        public int DateTimeFontSize;


        //Aesthetics    

        public bool EnableNoteLines;

        public int FontStyleNumber;
        public int BackgroundNumber;*/
    }

    public Settings _Settings = new Settings();

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public void FromJson(string a_Json)
    {
        JsonConvert.PopulateObject(a_Json, this);
    }
}
