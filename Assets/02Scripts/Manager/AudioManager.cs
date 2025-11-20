using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    public FMOD.System system;

    public FMOD.ChannelGroup musicChannelGroup;
    public FMOD.ChannelGroup bgmChannelGroup;
    public FMOD.ChannelGroup sfxChannelGroup;
    public FMOD.ChannelGroup keyChannelGroup;

    private FMOD.Sound musicSound;
    private FMOD.Channel musicChannel;

    private FMOD.Sound bgmSound;
    private FMOD.Channel bgmChannel;

    private Dictionary<string, float> lastPlayTime = new Dictionary<string, float>();
    private float minInterval = 0.03f;

    protected override void DoAwake()
    {
        base.DoAwake();
        system = FMODUnity.RuntimeManager.CoreSystem;

        system.createChannelGroup("Music", out musicChannelGroup);
        system.createChannelGroup("SFX", out sfxChannelGroup);
        system.createChannelGroup("BGM", out bgmChannelGroup);
        system.createChannelGroup("Key", out keyChannelGroup);
    }

    private void Update()
    {
        system.update();
    }

    public void PlayMusic(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"[AudioManager] 음악 파일 없음: {filePath}");
            return;
        }

        StopMusic();

        FMOD.RESULT res = system.createSound(filePath, FMOD.MODE.DEFAULT, out musicSound);
        if (res != FMOD.RESULT.OK || !musicSound.hasHandle())
        {
            Debug.LogError($"[AudioManager] 음악 로드 실패: {filePath}");
            return;
        }

        system.playSound(musicSound, musicChannelGroup, false, out musicChannel);
    }

    public void PlayBGM(string name)
    {
        StopBGM();

        bgmSound = BundlePackage.Instance.SoundBundle.GetSound(SoundBundle.Audio_Type.BGM, name);
        bgmSound.setMode(FMOD.MODE.LOOP_NORMAL);

        system.playSound(bgmSound, bgmChannelGroup, true, out bgmChannel);
        bgmChannel.setPaused(false);
    }

    public void PlaySFX(string name)
    {
        FMOD.Sound sound = BundlePackage.Instance.SoundBundle.GetSound(SoundBundle.Audio_Type.SFX, name);

        if (!sound.hasHandle())
        {
            Debug.LogWarning($"[AudioManager] SFX handle invalid: {name}");
            return;
        }

        system.playSound(sound, sfxChannelGroup, false, out FMOD.Channel ch);
    }

    public void PlayKeySound(string keySound)
    {
        string key = keySound.ToLower();

        if (!lastPlayTime.TryGetValue(key, out float last))
            last = -minInterval;

        if (Time.time - last < minInterval)
            return;

        FMOD.Sound sound = BundlePackage.Instance.SoundBundle.GetSound(SoundBundle.Audio_Type.Key, key);
        if (!sound.hasHandle()) return;

        system.playSound(sound, keyChannelGroup, false, out FMOD.Channel ch);
        lastPlayTime[key] = Time.time;
    }

    public void PlayPV(string folderName, string songName, bool loop = false)
    {
        string path = Path.Combine(Application.streamingAssetsPath, folderName, songName + ".ogg");
        if (!File.Exists(path))
        {
            Debug.LogWarning($"[AudioManager] PV 오디오 파일 없음: {path}");
            return;
        }

        StopBGM();

        FMOD.RESULT res = system.createSound(path, FMOD.MODE.DEFAULT, out FMOD.Sound sound);
        if (res != FMOD.RESULT.OK || !sound.hasHandle())
        {
            Debug.LogError($"[AudioManager] PV 로드 실패: {path}");
            return;
        }

        if (loop) sound.setMode(FMOD.MODE.LOOP_NORMAL);
        else sound.setMode(FMOD.MODE.LOOP_OFF);

        system.playSound(sound, bgmChannelGroup, true, out FMOD.Channel channel);
        channel.setPaused(false);
        bgmChannel = channel;
    }

    public void StopMusic()
    {
        if (musicChannel.hasHandle())
        {
            musicChannel.stop();
            musicChannel.clearHandle();
        }

        if (musicSound.hasHandle())
        {
            musicSound.release();
            musicSound.clearHandle();
        }
    }

    public void StopBGM()
    {
        if (bgmChannel.hasHandle())
        {
            bgmChannel.stop();
            bgmChannel.clearHandle();
        }
        if (bgmSound.hasHandle())
        {
            bgmSound.release();
            bgmSound.clearHandle();
        }
    }

    public float GetMusicTimeMS()
    {
        if (musicChannel.hasHandle())
        {
            musicChannel.getPosition(out uint pos, FMOD.TIMEUNIT.MS);
            return pos;
        }
        return -1f;
    }

    public float GetBGATimeMS()
    {
        if (bgmChannel.hasHandle())
        {
            bgmChannel.getPosition(out uint pos, FMOD.TIMEUNIT.MS);
            return pos;
        }
        return -1f;
    }

    private void OnDestroy()
    {
        StopBGM();
        StopMusic();

        sfxChannelGroup.release();
        musicChannelGroup.release();
        bgmChannelGroup.release();
        keyChannelGroup.release();
    }
}
