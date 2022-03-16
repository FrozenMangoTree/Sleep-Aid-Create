using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public List<Sound> SoundsBar1 = new List<Sound>();
    public List<Sound> SoundsBar2 = new List<Sound>();
    public List<Sound> SoundsBar3 = new List<Sound>();
    public List<Sound> SoundsBar4 = new List<Sound>();
    public List<Sound> SoundsBar5 = new List<Sound>();

    public string SoundLastPlayed;

    public bool SetAudioSourceListenerPause;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        SetupSounds();


        // PlaySound("Theme1");
    }

    void Start()
    {
        App.Instance.OnAppStart += Instance_OnAppStart;
    }

    private void Instance_OnAppStart(object sender, EventArgs e)
    {
        /*  PlaySound("Underwater");
          PlaySound("Warning");*/
    }

   
    void SetupSounds()
    {
        //IF this becomes to taxing can maybe only intialize Sounds that are actually played
        foreach (Sound s in SoundsBar1)
        {
            //s.Name = s.AudioClip.name;

            s.SourceOfSound = gameObject.AddComponent<AudioSource>();

            s.SourceOfSound.Stop();
            s.SourceOfSound.playOnAwake = false;

            s.SourceOfSound.clip = s.AudioClip;
            s.SourceOfSound.volume = s.Volume;
            s.SourceOfSound.pitch = s.Pitch;


            s.SourceOfSound.loop = s.Loop;
            s.SourceOfSound.playOnAwake = false;

            if (SetAudioSourceListenerPause) s.SourceOfSound.ignoreListenerPause = true;
        }
        foreach (Sound s in SoundsBar2)
        {
           // s.Name = s.AudioClip.name;

            s.SourceOfSound = gameObject.AddComponent<AudioSource>();

            s.SourceOfSound.Stop();
            s.SourceOfSound.playOnAwake = false;

            s.SourceOfSound.clip = s.AudioClip;
            s.SourceOfSound.volume = s.Volume;
            s.SourceOfSound.pitch = s.Pitch;


            s.SourceOfSound.loop = s.Loop;
        }
        foreach (Sound s in SoundsBar3)
        {
           // s.Name = s.AudioClip.name;

            s.SourceOfSound = gameObject.AddComponent<AudioSource>();
            s.SourceOfSound.Stop();
            s.SourceOfSound.playOnAwake = false;

            s.SourceOfSound.clip = s.AudioClip;
            s.SourceOfSound.volume = s.Volume;
            s.SourceOfSound.pitch = s.Pitch;


            s.SourceOfSound.loop = s.Loop;

            if (SetAudioSourceListenerPause) s.SourceOfSound.ignoreListenerPause = true;

        }
        foreach (Sound s in SoundsBar4)
        {
         //   s.Name = s.AudioClip.name;

            s.SourceOfSound = gameObject.AddComponent<AudioSource>();

            s.SourceOfSound.Stop();
            s.SourceOfSound.playOnAwake = false;

            s.SourceOfSound.clip = s.AudioClip;
            s.SourceOfSound.volume = s.Volume;
            s.SourceOfSound.pitch = s.Pitch;


            s.SourceOfSound.loop = s.Loop;


            if (SetAudioSourceListenerPause) s.SourceOfSound.ignoreListenerPause = true;
        }


        foreach (Sound s in SoundsBar5)
        {
            //   s.Name = s.AudioClip.name;

            s.SourceOfSound = gameObject.AddComponent<AudioSource>();

            s.SourceOfSound.Stop();
            s.SourceOfSound.playOnAwake = false;

            s.SourceOfSound.clip = s.AudioClip;
            s.SourceOfSound.volume = s.Volume;
            s.SourceOfSound.pitch = s.Pitch;


            s.SourceOfSound.loop = s.Loop;


            if (SetAudioSourceListenerPause) s.SourceOfSound.ignoreListenerPause = true;
        }
    }


    [Obsolete]
    public float SoundRemap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void ChangeSoundDynamics_String(float volume, float pitch, bool loop, bool mute, string SoundToChangeName, int soundBarID)
    {
        Sound s = null;

        
        switch (soundBarID)
        {
            case 0:
                s = Array.Find(SoundsBar1.ToArray(), sound => sound.Name == SoundToChangeName && sound.ID == soundBarID);
                break;
            case 1:
                s = Array.Find(SoundsBar2.ToArray(), sound => sound.Name == SoundToChangeName && sound.ID == soundBarID);
                break;
            case 2:
                s = Array.Find(SoundsBar3.ToArray(), sound => sound.Name == SoundToChangeName && sound.ID == soundBarID);
                break;
            case 3:
                s = Array.Find(SoundsBar4.ToArray(), sound => sound.Name == SoundToChangeName && sound.ID == soundBarID);
                break;
            case 4:
                s = Array.Find(SoundsBar5.ToArray(), sound => sound.Name == SoundToChangeName && sound.ID == soundBarID);
                break;
        }
           

        if (s == null || soundBarID != s.ID)
        {
            Debug.LogWarning("Sound to change: " + name + " not found!" + " or IDs werent matching!");
            return;
        }

        s.SourceOfSound.volume = volume;
        s.SourceOfSound.pitch = pitch;
        s.SourceOfSound.loop = loop;


        s.SourceOfSound.mute = mute;
    }
    public void ChangeSoundDynamics_Int(float volume, float pitch, bool loop, bool mute, int SoundToChange, int soundBarID)
    {
        Sound s = null;


        switch (soundBarID)
        {
            case 0:
                s = SoundsBar1[SoundToChange];
                break;
            case 1:
                s = SoundsBar2[SoundToChange];
                break;
            case 2:
                s = SoundsBar3[SoundToChange];
                break;
            case 3:
                s =  SoundsBar4[SoundToChange];
                break;
            case 4:
                s = SoundsBar5[SoundToChange];
                break;
        }

        if(s.ID != soundBarID)
        {
            Debug.LogError("ID mismatch between bar ID and sound ID");
            s = null;
        }


        if (s == null || soundBarID != s.ID)
        {
            Debug.LogWarning("Sound to change: " + name + " not found!" + " or IDs werent matching!");
            return;
        }

        s.SourceOfSound.volume = volume;
        s.SourceOfSound.pitch = pitch;
        s.SourceOfSound.loop = loop;


        s.SourceOfSound.mute = mute;
    }

    public void PlaySound_By_String(string SoundToPlay, int soundBarID)
    {
        Sound s = null;

        switch (soundBarID)
        {
            case 0:
                s = Array.Find(SoundsBar1.ToArray(), sound => sound.Name == SoundToPlay && sound.ID == soundBarID);
                break;
            case 1:
                s = Array.Find(SoundsBar2.ToArray(), sound => sound.Name == SoundToPlay && sound.ID == soundBarID);
                break;
            case 2:
                s = Array.Find(SoundsBar3.ToArray(), sound => sound.Name == SoundToPlay && sound.ID == soundBarID);
                break;
            case 3:
                s = Array.Find(SoundsBar4.ToArray(), sound => sound.Name == SoundToPlay && sound.ID == soundBarID);
                break;
            case 4:
                s = Array.Find(SoundsBar5.ToArray(), sound => sound.Name == SoundToPlay && sound.ID == soundBarID);
                break;
        }

        if (s == null || soundBarID != s.ID)
        {
            Debug.LogWarning("Sound: " + name + " not found!" + " or IDs werent matching!");
            return;
        }


        s.SourceOfSound.Play();
        SoundLastPlayed = s.Name;
    }
    public void StopSound_By_String(string SoundToStop, int soundBarID)
    {
        Sound s = null;

        switch (soundBarID)
        {
            case 0:
                s = Array.Find(SoundsBar1.ToArray(), sound => sound.Name == SoundToStop && sound.ID == soundBarID);
                break;
            case 1:
                s = Array.Find(SoundsBar2.ToArray(), sound => sound.Name == SoundToStop && sound.ID == soundBarID);
                break;
            case 2:
                s = Array.Find(SoundsBar3.ToArray(), sound => sound.Name == SoundToStop && sound.ID == soundBarID);
                break;
            case 3:
                s = Array.Find(SoundsBar4.ToArray(), sound => sound.Name == SoundToStop && sound.ID == soundBarID);
                break;
            case 4:
                s = Array.Find(SoundsBar5.ToArray(), sound => sound.Name == SoundToStop && sound.ID == soundBarID);
                break;
        }



        if (s == null || soundBarID != s.ID) {
            Debug.LogWarning("Sound: " + name + " not found!" +" or IDs werent matching!"); 
            return; 
        }

        s.SourceOfSound.Stop();
       
        SoundLastPlayed = s.Name;
    }
    public void PlaySound_By_Index(int SoundToPlay, int soundBarID)
    {
        Sound s = null;

        switch (soundBarID)
        {
            case 0:
                s = SoundsBar1[SoundToPlay];
                break;
            case 1:
                s = SoundsBar2[SoundToPlay];
                break;
            case 2:
                s = SoundsBar3[SoundToPlay];
                break;
            case 3:
                s = SoundsBar4[SoundToPlay];
                break;
            case 4:
                s = SoundsBar5[SoundToPlay];
                break;

        }

        if (s == null || soundBarID != s.ID)
        {
            Debug.LogWarning("Sound: " + name + " not found!" + " or IDs werent matching!");
            return;
        }


        s.SourceOfSound.Play();
        SoundLastPlayed = s.Name;
    }

    public void PauseSound_By_Index(int SoundToPlay, int soundBarID)
    {
        Sound s = null;

        switch (soundBarID)
        {
            case 0:
                s = SoundsBar1[SoundToPlay];
                break;
            case 1:
                s = SoundsBar2[SoundToPlay];
                break;
            case 2:
                s = SoundsBar3[SoundToPlay];
                break;
            case 3:
                s = SoundsBar4[SoundToPlay];
                break;
            case 4:
                s = SoundsBar5[SoundToPlay];
                break;
        }

        if (s == null || soundBarID != s.ID)
        {
            Debug.LogWarning("Sound: " + name + " not found!" + " or IDs werent matching!");
            return;
        }


        s.SourceOfSound.Pause();

        SoundLastPlayed = s.Name;
    }
    public void StopSound_By_Index(int SoundToStop, int soundBarID)
    {
        Sound s = null;

        switch (soundBarID)
        {
            case 0:
                s = SoundsBar1[SoundToStop];
                break;
            case 1:
                s = SoundsBar2[SoundToStop];
                break;
            case 2:
                s = SoundsBar3[SoundToStop];
                break;
            case 3:
                s = SoundsBar4[SoundToStop];
                break;
            case 4:
                s = SoundsBar5[SoundToStop];
                break;
        }



        if (s == null || soundBarID != s.ID)
        {
            Debug.LogWarning("Sound: " + name + " not found!" + " or IDs werent matching!");
            return;
        }

        s.SourceOfSound.Stop();

        SoundLastPlayed = s.Name;
    }


    public bool SoundStatus_IsPlaying(int SoundToCheck, int soundBarID)
    {

        bool s = false;
        switch (soundBarID)
        {
            case 0:
                s = SoundsBar1[SoundToCheck].SourceOfSound.isPlaying;
                break;
            case 1:
                s = SoundsBar1[SoundToCheck].SourceOfSound.isPlaying;
                break;
            case 2:
                s = SoundsBar1[SoundToCheck].SourceOfSound.isPlaying;
                break;
            case 3:
                s = SoundsBar1[SoundToCheck].SourceOfSound.isPlaying;
                break;
            case 4:
                s = SoundsBar1[SoundToCheck].SourceOfSound.isPlaying;
                break;
        }

        return s;
    }


}


[System.Serializable]
public class Sound
{
    public string Name;
    public int ID;

    public AudioSource SourceOfSound;
    public AudioClip AudioClip;

    [Range(0,1f)]
    public float Volume;
    [Range(0.5f, 2f)]
    public float Pitch;
    public bool Loop;

}
