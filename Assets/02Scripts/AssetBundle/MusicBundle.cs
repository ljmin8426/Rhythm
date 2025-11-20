using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMusicBundle", menuName = "Music/MusicBundle")]
public class MusicBundle : ScriptableObject
{
    [SerializeField] private BaseMusicBundle[] bundles;

    // 초기화 (필요시 BaseMusicBundle 내부 세팅)
    public void Init()
    {
        if (bundles == null || bundles.Length == 0)
            return;

        foreach (var baseMusicBundle in bundles)
        {
            baseMusicBundle?.Initialized();
        }
    }

    // 모든 음악 반환
    public BaseMusicBundle[] GetAllMusicBundles()
    {
        return bundles;
    }

    // 이름으로 특정 음악 찾기
    public BaseMusicBundle GetMusicBundle(string musicName)
    {
        if (string.IsNullOrEmpty(musicName))
            return null;

        foreach (var bundle in bundles)
        {
            if (bundle != null && bundle.Info != null &&
                string.Equals(bundle.Info.title, musicName, StringComparison.OrdinalIgnoreCase))
            {
                return bundle;
            }
        }

        return null;
    }

    // JSON 기반 음악 정보 로드
    public static MusicInfo_ LoadMusicInfo(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning($"[MusicBundle] 파일을 찾을 수 없습니다: {path}");
            return null;
        }

        try
        {
            string json = File.ReadAllText(path).Trim();
            return JsonUtility.FromJson<MusicInfo_>(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"[MusicBundle] JSON 파싱 실패 ({path}): {e.Message}");
            return null;
        }
    }
}
