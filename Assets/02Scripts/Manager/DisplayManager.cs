using System;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{   
    public static int fullScreenTextIndex;

    public static int resolutionTextIndex;

    public static int frameTextIndex;

    public static int vsyncTextIndex;
    public static void InitDisplayOptions()
    {
        DisplayManager.LoadSettings();
        DisplayManager.ApplyDisplayOptions();
    }

    public static void ApplyDisplayOptions()
    {
        bool fullscreen = DisplayManager.fullScreenTextIndex == 1;
        int width;
        int height;
        switch (DisplayManager.resolutionTextIndex)
        {
            case 0:
                width = 1280;
                height = 720;
                break;
            case 1:
                width = 1600;
                height = 900;
                break;
            case 2:
                width = 1920;
                height = 1080;
                break;
            case 3:
                width = 2560;
                height = 1440;
                break;
            case 4:
                width = 3840;
                height = 2160;
                break;
            default:
                width = 1920;
                height = 1080;
                break;
        }
        int num;
        switch (DisplayManager.frameTextIndex)
        {
            case 0:
                num = 60;
                break;
            case 1:
                num = 120;
                break;
            case 2:
                num = 144;
                break;
            case 3:
                num = 240;
                break;
            case 4:
                num = 360;
                break;
            case 5:
                num = -1;
                break;
            default:
                num = -1;
                break;
        }
        int targetFrameRate = num;
        Screen.SetResolution(width, height, fullscreen);
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = DisplayManager.vsyncTextIndex;
    }

    public static void SaveSettings()
    {
        PlayerPrefs.SetInt("fullScreenTextIndex", DisplayManager.fullScreenTextIndex);
        PlayerPrefs.SetInt("resolutionTextIndex", DisplayManager.resolutionTextIndex);
        PlayerPrefs.SetInt("frameTextIndex", DisplayManager.frameTextIndex);
        PlayerPrefs.SetInt("vsyncTextIndex", DisplayManager.vsyncTextIndex);
        PlayerPrefs.Save();
    }
    public static void LoadSettings()
    {
        DisplayManager.fullScreenTextIndex = PlayerPrefs.GetInt("fullScreenTextIndex", 1);
        DisplayManager.resolutionTextIndex = PlayerPrefs.GetInt("resolutionTextIndex", 2);
        DisplayManager.frameTextIndex = PlayerPrefs.GetInt("frameTextIndex", 5);
        DisplayManager.vsyncTextIndex = PlayerPrefs.GetInt("vsyncTextIndex", 0);
    }
}
