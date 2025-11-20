using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class MusicInfo_
{
    public string title;

    public string artist;

    public string version;

    public int time;

    public string composer;

    public int sheetOffset;

    public List<Timming> timmingList;

    public float BPM => this.timmingList[0].bpm;
    public int BeatMetor => this.timmingList[0].beatMetor;
    public int BeatUnit => this.timmingList[0].beatUnit;

    public MusicInfo_()
    {
        this.title = string.Empty;
        this.artist = string.Empty;
        this.version = string.Empty;
        this.time = 0;
        this.composer = string.Empty;
        this.sheetOffset = 0;
        this.timmingList = new List<Timming>();
        this.timmingList.Add(new Timming());
    }

    public void InitializeValidate()
    {
        this.title = this.title.Trim();
        this.artist = this.artist.Trim();
        this.version = this.version.Trim();
    }

    // 유효성 검사 
    public bool IsValidInfo()
    {
        return !string.IsNullOrEmpty(this.title) && !string.IsNullOrEmpty(this.artist) && this.timmingList != null;
    }

    public string GetBpmString()
    {
        IEnumerable<float> source = from t in this.timmingList
                                    select t.bpm;
        float num = source.Min();
        float num2 = source.Max();
        if (num == num2)
        {
            return num.ToString();
        }
        return string.Format("{0}~{1}", num, num2);
    }

    // 병합
    public void Merge(MusicInfo_ add)
    {
        this.title = (string.IsNullOrEmpty(this.title) ? add.title : this.title);
        this.artist = (string.IsNullOrEmpty(this.artist) ? add.artist : this.artist);
        this.version = (string.IsNullOrEmpty(this.version) ? add.version : this.version);
        this.composer = ((!string.IsNullOrEmpty(add.composer)) ? add.composer : this.composer);
        this.sheetOffset = add.sheetOffset;
    }
}
