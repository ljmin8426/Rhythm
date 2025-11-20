using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseMusicBundle", menuName = "Music/BaseMusicBundle")]
public class BaseMusicBundle : ScriptableObject
{
    [Header("Music")]
    [SerializeField] private string rootFileName;
    [SerializeField] private string folder;
    [SerializeField] private string preview;
    [SerializeField] private string spritePath;
    [SerializeField] private MusicInfo_ musicInfo;
    [SerializeField] private MusicPack originalPack;
    [SerializeField] private SheetPack[] sheetPack4Keys;

    [NonSerialized] private Sprite loadAlbum;
    [NonSerialized] private MusicSheet loadSheet;
    [NonSerialized] private SoundData loadSound;
    [NonSerialized] private string loadVideoUrl;
    [NonSerialized] private Difficulty applyDiffculty;

    public virtual MusicInfo_ Info => this.musicInfo;
    public virtual MusicSheet Sheet => this.loadSheet;
    public virtual SoundData SoundClip => this.loadSound;
    public string VideoUrl
    {
        get
        {
            if(!File.Exists(this.loadVideoUrl))
            {
                return string.Empty;
            }
            return this.loadVideoUrl;
        }
    }
    public Sprite Album => this.loadAlbum;
    public virtual Difficulty ApplyDiffculty => this.applyDiffculty;
    public string title { get; private set; }
    public string previewUrl { get; private set; }
    public string artist { get; private set; }
    public float bpm { get; private set; }
    public int time { get; private set; }
    public string sortArtist { get; private set; }

    public void Initialized()
    {
        // StreamingAssets 경로로부터 Sprite 로드
        string fullPath = Path.Combine(Application.streamingAssetsPath, this.spritePath);

        if (File.Exists(fullPath))
        {
            byte[] imageBytes = File.ReadAllBytes(fullPath);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(imageBytes))
            {
                this.loadAlbum = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
            }
        }
        if (this.loadAlbum == null)
        {
            Debug.Log("Album is null : " + this.rootFileName);
        }
        this.previewUrl = Path.Combine(Paths.MUSIC_PATH, this.preview);
        this.title = this.Info.title;
        this.artist = this.Info.artist;
        this.bpm = this.Info.BPM;
        this.time = this.Info.time;
        this.sortArtist = this.Info.artist;
    }

    public static int MedicateValue(Difficulty diff)
    {
        int result;
        switch (diff)
        {
            case Difficulty.Easy:
                result = 1;
                break;
            case Difficulty.Normal:
                result = 1;
                break;
            case Difficulty.Hard:
                result = -1;
                break;
            case Difficulty.Insane:
                result = -1;
                break;
            default:
                result = 1;
                break;
        }
        return result;
    }

    // LoadMusic 
    public void LoadMusic()
    {
        string musicPath = Path.Combine(Application.streamingAssetsPath, "Music", folder, $"{rootFileName}.ogg");
        if (!File.Exists(musicPath))
        {
            Debug.LogWarning($"[BaseMusicBundle] 음악 파일이 존재하지 않음: {musicPath}");
            return;
        }

        var result = AudioManager.Instance.system.createSound(musicPath, FMOD.MODE.LOOP_NORMAL, out FMOD.Sound sound);
        if (result == FMOD.RESULT.OK)
        {
            this.loadSound = new SoundData
            {
                sound = sound,
                path = musicPath
            };
        }
        else
        {
            Debug.LogError($"[BaseMusicBundle] FMOD 사운드 로드 실패: {result}");
        }

        // BGA 비디오 경로 세팅
        string videoPath = Path.Combine(Application.streamingAssetsPath, "Music", folder, $"{rootFileName}.mp4");
        if (File.Exists(videoPath))
        {
            this.loadVideoUrl = videoPath;
        }
    }

    // PlayMusic
    public void PlayMusic()
    {
        if (this.loadSound == null || !this.loadSound.sound.hasHandle())
        {
            LoadMusic(); // 로드
        }
        if (this.loadSound != null && this.loadSound.sound.hasHandle())
        {
            AudioManager.Instance.PlayMusic(this.loadSound.path);
        }
        else
        {
            Debug.LogWarning($"[BaseMusicBundle] 사운드가 로드되지 않았습니다: {this.rootFileName}");
        }
    }
}
