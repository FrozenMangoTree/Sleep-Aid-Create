using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

public class FramesManager : MonoBehaviour
{
    public static FramesManager Instance;



    public bool StartFrameToggle;
    float frameToggleTimer;
    public float FrameToggleTimePeriod;

    public void ResetFrameToggle()
    {
        frameToggleTimer = FrameToggleTimePeriod;
        ToggleFrameIntervalInstant(false);
    }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        //StartCoroutine(ToggleFrameInterval(false));
    }

    public int ReturnOnDemandRenderingFrameInterval() => OnDemandRendering.renderFrameInterval;

    public IEnumerator TimeToggleFrameSwitch(float timeAmount)
    {
       if(OnDemandRendering.renderFrameInterval != 1) OnDemandRendering.renderFrameInterval = 1; //Fast

        yield return new WaitForSecondsRealtime(timeAmount);

       if (OnDemandRendering.renderFrameInterval != 60) OnDemandRendering.renderFrameInterval = 60; //Minimal
    }
    public void ToggleFrameIntervalInstant(bool MinimalFrames, int RestoreOriginalFrameRateTo = 1)
    {
        if (MinimalFrames)
        {
            if (OnDemandRendering.renderFrameInterval != 60) OnDemandRendering.renderFrameInterval = 60;
        }
        else
        {
            if (OnDemandRendering.renderFrameInterval != 1) OnDemandRendering.renderFrameInterval = RestoreOriginalFrameRateTo;
        }
    }
    public IEnumerator ToggleFrameInterval(bool On, int RestoreOriginalFrameRateTo = 1, float toggleDelayAmount = 0, bool toggleDelayOneFrame = default)
    {
        if (toggleDelayOneFrame) yield return new WaitForEndOfFrame();
        else yield return new WaitForSecondsRealtime(toggleDelayAmount);

        if (On)
        {
         //   Debug.Log("toggled on coro");
            OnDemandRendering.renderFrameInterval = RestoreOriginalFrameRateTo;
        }
        else
        {
          //  Debug.Log("toggled off coro");
            OnDemandRendering.renderFrameInterval = 60;
        }
    }

    [Obsolete] //Wait Until doesn't seem to be working with parameters, only direct references hence we will use Screen Condition Frame Toggle
    public IEnumerator ConditionFrameToggle(GameObject TargetObjectToWaitActiveOn = default, Func<bool> methodToWaitReturnOn = default, bool boolToWaitOn = default)
    {
        /*
                //Wait Until doesn't seem to be working with parameters, only direct references hence we will use Screen Condition Frame Toggle
                bool waitingOn = boolToWaitOn;
                ToggleFrameInterval(true);
                Debug.Log("loading ");
                // yield return new WaitUntil(() => methodToWaitOn());
                yield return new WaitUntil(() => waitingOn*//*notesEditScreen.LoadedSegments*//*);

                yield return new WaitForSecondsRealtime(1f);
                Debug.Log("Finished loading");
                ToggleFrameInterval(false);*/

        yield return new WaitForSecondsRealtime(1f);
    }
    public IEnumerator ScreenSpecificConditionFrameToggle(string screenOf/*, Action<int> funkytown*/)
    {
        StartCoroutine(ToggleFrameInterval(true, 0));

       // yield return new WaitForEndOfFrame();

        switch(screenOf)
        {
            case "EditScreen":
            //yield return new WaitUntil(() => notesEditScreen.LoadedEditScreen);

            yield return new WaitForSecondsRealtime(1f);
                StartCoroutine(ToggleFrameInterval(false));
           // Debug.Log("frame back to default false frames manager");

            break;
        }
    }



    private void Update()
    {
        if (StartFrameToggle)
        {
            frameToggleTimer -= Time.deltaTime;

            if (frameToggleTimer <= 0)
            {
                ToggleFrameIntervalInstant(true);
            }
        }
    }
}
