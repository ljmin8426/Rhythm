using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MusicData", menuName = "ScriptableObjects/MusicData")]
public class MusicData_ : ScriptableObject
{
    public List<Music> musicList = new List<Music>();
}

[System.Serializable]
public class Music
{
    public string title;
    public string artist;

    public string audioPath;
    public string BGAPath;
    public string BGPath;

    public bool hasVideo;

    public List<Chart> charts = new();
}

[System.Serializable]
public class Chart
{
    public string diff;            // 난이도 텍스트
    public Difficulty enumDiff;    // enum으로 변환된 난이도
    public string chartPath;

    public string title;
    public string artist;

    public int offSet;
    public int BPM;
    public int meter;

    public int circleSize;

    public int noteCount;

    public void ParseDifficulty()
    {
        enumDiff = Parse(diff);
    }

    private Difficulty Parse(string difficulty)
    {
        if (string.IsNullOrWhiteSpace(difficulty))
        {
            Debug.LogWarning("[DifficultyParser] 빈 문자열 -> 기본값 Normal 반환");
            return Difficulty.Normal;
        }

        switch (difficulty.Trim().ToLower())
        {
            case "easy": 
                return Difficulty.Easy;
            case "normal":
                return Difficulty.Normal;
            case "hard":
                return Difficulty.Hard;
            case "insane":
                return Difficulty.Insane;
            default:
                Debug.LogWarning($"[DifficultyParser] 알 수 없는 난이도 '{difficulty}' -> 기본값 Normal 반환");
                return Difficulty.Normal;
        }
    }
}