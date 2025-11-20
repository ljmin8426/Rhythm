using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroBGAManager : SingletonDestroy<IntroBGAManager>
{
    [SerializeField] private VideoPlayer vp;
    [SerializeField] private RawImage bga;

    protected override void DoAwake()
    {
        base.DoAwake();

        // RenderTexture 생성
        vp.targetTexture = new RenderTexture(1920, 1080, 0);
        bga.texture = vp.targetTexture;

        vp.prepareCompleted -= OnPrepareComplete;
        vp.prepareCompleted += OnPrepareComplete;
    }

    private void Start()
    {
        PreparePV("PV");
    }

    private void PreparePV(string songName)
    {
        AudioManager.Instance.PlayPV("PV", songName, loop: true);

        string videoPath = Path.Combine(Application.streamingAssetsPath, "PV", songName + ".mp4");
        if (File.Exists(videoPath))
        {
            vp.url = videoPath;
            vp.Prepare();
            vp.prepareCompleted += (source) =>
            {
                vp.Play();
            };
        }
    }

    private void OnPrepareComplete(VideoPlayer source)
    {
        PlayBGA();
    }

    private void PlayBGA()
    {
        if (vp.isPrepared)
        {
            // FMOD 오디오 시간과 동기화
            double audioTime = AudioManager.Instance.GetBGATimeMS() / 1000.0;
            vp.time = audioTime;
            vp.Play();
        }
    }

    public void StopBGA()
    {
        AudioManager.Instance.StopBGM();

        if (vp != null)
        {
            if (vp.isPrepared || vp.isPlaying)
            {
                vp.Stop();
            }
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) return;

        if (vp.isPrepared)
        {
            double audioTime = AudioManager.Instance.GetBGATimeMS() / 1000.0;
            vp.time = audioTime;
        }
    }

    private void OnDisable()
    {
        StopBGA();
    }

    private void OnDestroy()
    {
        if (vp != null)
        {
            if (vp.targetTexture != null)
            {
                vp.targetTexture.Release();
                vp.targetTexture = null;
            }

            vp.prepareCompleted -= OnPrepareComplete;
        }
    }
}
