using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "ResourceBundle", menuName = "ScriptableObjects/Bundle")]
public class SoundBundle : ScriptableObject
{
    public enum Audio_Type { BGM, SFX, Key }

    [Header("BGM")]
    public List<SoundData> bgmList = new List<SoundData>();
    [Header("SFX")]
    public List<SoundData> sfxList = new List<SoundData>();
    [Header("Key Sounds")]
    public List<SoundData> keyList = new List<SoundData>();

    private Dictionary<string, FMOD.Sound> bgmDict = new();
    private Dictionary<string, FMOD.Sound> sfxDict = new();
    private Dictionary<string, FMOD.Sound> keyDict = new();

    private FMOD.Sound defaultSound;

    [Header("Load")]
    [SerializeField] private bool isLoaded;

    private void OnEnable()
    {
        isLoaded = false;
    }

    public void LoadAllFromStreamingAssets()
    {
        if (isLoaded) return;

        LoadAudioType(Audio_Type.BGM, "BGM", bgmList, bgmDict);
        LoadAudioType(Audio_Type.SFX, "SFX", sfxList, sfxDict);
        LoadAudioType(Audio_Type.Key, "KeySound", keyList, keyDict);

        isLoaded = true;
    }

    private void LoadAudioType(Audio_Type type, string folderName, List<SoundData> list, Dictionary<string, FMOD.Sound> dict)
    {
        dict.Clear();

        string folderPath = Path.Combine(Application.streamingAssetsPath, folderName);
        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning($"[ResourceBundle] 폴더 없음: {folderPath}");
            return;
        }

        // .ogg와 .wav 파일 모두 가져오기
        string[] oggFiles = Directory.GetFiles(folderPath, "*.ogg", SearchOption.AllDirectories);
        string[] wavFiles = Directory.GetFiles(folderPath, "*.wav", SearchOption.AllDirectories);
        string[] files = oggFiles.Concat(wavFiles).ToArray();

        list.Clear();

        foreach (var fullPath in files)
        {
            string key = Path.GetFileNameWithoutExtension(fullPath).ToLower();

            // 상대경로로 변환 (StreamingAssets/ 이후 부분만 남김)
            string relativePath = fullPath.Replace(Application.streamingAssetsPath + Path.DirectorySeparatorChar, "")
                                          .Replace("\\", "/");

            // FMOD.Sound 생성
            FMOD.RESULT res = AudioManager.Instance.system.createSound(fullPath, FMOD.MODE.DEFAULT, out FMOD.Sound sound);
            if (res != FMOD.RESULT.OK || !sound.hasHandle())
            {
                Debug.LogWarning($"[ResourceBundle] 사운드 로드 실패: {relativePath}");
                sound = defaultSound;
            }
            else
            {
                Debug.Log($"[ResourceBundle] 사운드 로드 성공: {key} ({relativePath})");
            }

            // 리스트에 저장
            SoundData data = new SoundData
            {
                soundName = key,
                path = relativePath, // 절대경로 대신 상대경로 저장
                sound = sound
            };
            list.Add(data);

            // 딕셔너리에 저장
            dict[key] = sound;
        }

        // 항상 "default" 키 존재
        if (!dict.ContainsKey("default"))
        {
            dict["default"] = defaultSound;
            list.Add(new SoundData { soundName = "default", path = "", sound = defaultSound });
        }
    }

    public FMOD.Sound GetSound(Audio_Type type, string name)
    {
        string key = string.IsNullOrEmpty(name) ? "default" : name.ToLower();

        return type switch
        {
            Audio_Type.BGM => bgmDict.TryGetValue(key, out var bgm) && bgm.hasHandle() ? bgm : bgmDict["default"],
            Audio_Type.SFX => sfxDict.TryGetValue(key, out var sfx) && sfx.hasHandle() ? sfx : sfxDict["default"],
            Audio_Type.Key => keyDict.TryGetValue(key, out var keySound) && keySound.hasHandle() ? keySound : keyDict["default"],
            _ => bgmDict["default"]
        };
    }
}
