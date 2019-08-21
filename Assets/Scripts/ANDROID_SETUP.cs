using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANDROID_SETUP : MonoBehaviour {

    int qualityLevel = 0;

    private void Awake()
    {
        #if UNITY_ANDROID

                Application.targetFrameRate = 30;

                QualitySettings.vSyncCount = 0;

                QualitySettings.antiAliasing = 0;

                if (qualityLevel == 0)
                {
                    QualitySettings.shadowCascades = 0;
                    QualitySettings.shadowDistance = 15;
                }

                else if (qualityLevel == 5)
                {
                    QualitySettings.shadowCascades = 2;
                    QualitySettings.shadowDistance = 70;
                }

                Screen.sleepTimeout = SleepTimeout.NeverSleep;

        #endif



        #if UNITY_STANDALONE_WIN
         
                 Application.targetFrameRate = 60;
                 QualitySettings.vSyncCount = 1; 
         
                 if (qualityLevel == 0)
                 {
                     QualitySettings.antiAliasing = 0;
                 }
         
                 if (qualityLevel == 5)
                 {
                     QualitySettings.antiAliasing = 8;
                 }
         
        #endif
    }

}
