using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MusicParser : IParser<List<Music>>
{
    private static readonly string defaultBGPath = Path.Combine(Application.streamingAssetsPath, "Default", "background.jpg");

    public List<Music> Parse(string musicFolderPath)
    {
        List<Music> musicList = new();

        if (!Directory.Exists(musicFolderPath))
        {
            Debug.LogError("Music 폴더를 찾을 수 없습니다.");
            return musicList;
        }

        foreach (string folderPath in Directory.GetDirectories(musicFolderPath))
        {
            Music music = new();

            LoadAudio(folderPath, music);
            LoadImageAndVideo(folderPath, music);
            LoadCharts(folderPath, music);

            musicList.Add(music);
        }

        return musicList;
    }

    private void LoadAudio(string path, Music music)
    {
        string[] exts = { "*.ogg", "*.mp3" };

        foreach (var ext in exts)
        {
            var files = new DirectoryInfo(path).GetFiles(ext);

            if (files.Length > 0)
            {
                music.audioPath = files[0].FullName;
                return;
            }
        }
    }

    private void LoadImageAndVideo(string path, Music music)
    {
        var images = GetFiles(path, new[] { "*.jpg", "*.png" });

        music.BGPath = images.Length > 0 ? images[0].FullName : defaultBGPath;

        var videos = new DirectoryInfo(path).GetFiles("*.mp4");

        if (videos.Length > 0)
        {
            music.hasVideo = true;
            music.BGAPath = videos[0].FullName;
        }
    }

    private void LoadCharts(string path, Music music)
    {
        var charts = new DirectoryInfo(path).GetFiles("*.osu");
        var parser = new ChartParser();

        foreach (var chartFile in charts)
        {
            var chart = parser.Parse(chartFile.FullName);

            // Chart에서 메타데이터를 Music에 반영
            if (string.IsNullOrEmpty(music.title))
                music.title = chart.title;

            if (string.IsNullOrEmpty(music.artist))
                music.artist = chart.artist;

            music.charts.Add(chart);
        }
    }

    private FileInfo[] GetFiles(string path, string[] extensions)
    {
        List<FileInfo> result = new();

        var dir = new DirectoryInfo(path);

        foreach (var ext in extensions)
            result.AddRange(dir.GetFiles(ext));

        return result.ToArray();
    }
}
