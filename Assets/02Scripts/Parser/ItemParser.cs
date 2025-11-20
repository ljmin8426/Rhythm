#if UNITY_EDITOR
using System.IO;
using UnityEngine;

public static class ItemParser
{
    public static void ParseAll(MusicData_ musicData)
    {
        var musicParser = new MusicParser();
        string musicPath = Path.Combine(Application.streamingAssetsPath, "Music");

        musicData.musicList = musicParser.Parse(musicPath);
    }
}
#endif
