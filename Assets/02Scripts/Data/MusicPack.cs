using System;
using System.IO;
using Unity.VisualScripting;

[Serializable]
public class MusicPack
{
    public MusicPack(string videoName, string audioName)
    {
        this.videoName = videoName;
        this.audioName = audioName;
    }

    public string videoName;
    public string audioName;

    public string videoUrl
    {
        get
        {
            if(!string.IsNullOrEmpty(this.videoName))
            {
                return Path.Combine(Paths.STELL_PATH, this.videoName);
            }
            return string.Empty;
        }
    }

    public string audioUrl
    {
        get
        { 
            return Path.Combine(Paths.STELL_PATH, this.audioName);
        }
    }
}
