using UnityEngine;
using System.Collections;

public class VolumeManager : Singleton<VolumeManager>
{
    public enum VolumeType { Music, SFX, BGM }

    private const float DefaultVolume = 0.5f;

    private const string KEY_MUSIC = "MusicVolume";
    private const string KEY_SFX = "SFXVolume";
    private const string KEY_BGM = "BGMVolume";

    public float MusicVolume { get; private set; }
    public float SFXVolume { get; private set; }
    public float BGMVolume { get; private set; }

    private void Awake()
    {
        LoadVolumes();
    }

    private void Start()
    {
        StartCoroutine(WaitForAudioManagerAndApply());
    }

    private IEnumerator WaitForAudioManagerAndApply()
    {
        float timeout = 5f;
        float timer = 0f;

        while (AudioManager.Instance == null && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (AudioManager.Instance != null)
            ApplyVolumes();
        else
            Debug.LogWarning("[VolumeManager] AudioManager not found. Volumes not applied.");
    }

    private void LoadVolumes()
    {
        MusicVolume = PlayerPrefs.GetFloat(KEY_MUSIC, DefaultVolume);
        SFXVolume = PlayerPrefs.GetFloat(KEY_SFX, DefaultVolume);
        BGMVolume = PlayerPrefs.GetFloat(KEY_BGM, DefaultVolume);
    }

    private void SaveVolumes()
    {
        PlayerPrefs.SetFloat(KEY_MUSIC, MusicVolume);
        PlayerPrefs.SetFloat(KEY_SFX, SFXVolume);
        PlayerPrefs.SetFloat(KEY_BGM, BGMVolume);
        PlayerPrefs.Save();
    }

    public void SetVolume(VolumeType type, float value)
    {
        value = Mathf.Clamp01(value);

        switch (type)
        {
            case VolumeType.Music:
                MusicVolume = value;
                break;
            case VolumeType.SFX:
                SFXVolume = value;
                break;
            case VolumeType.BGM:
                BGMVolume = value;
                break;
        }

        SaveVolumes();
        ApplyVolumes();
    }

    public void ApplyVolumes()
    {
        var audio = AudioManager.Instance;
        if (audio == null) return;

        audio.musicChannelGroup.setVolume(MusicVolume);
        audio.sfxChannelGroup.setVolume(SFXVolume);
        audio.bgmChannelGroup.setVolume(BGMVolume);
    }
}
