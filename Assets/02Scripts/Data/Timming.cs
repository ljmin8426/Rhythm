using System;

[Serializable]
public class Timming
{    
    public int millisec;

    public float bpm;

    public float speed;

    public int beatMetor;

    public int beatUnit;

    public Timming()
    {
        this.millisec = 0;
        this.bpm = 60f;
        this.speed = 1f;
        this.beatMetor = 4;
        this.beatUnit = 4;
    }

    public Timming(int millisec, float bpm, float speed, int beatMetor, int beatUnit)
    {
        this.millisec = millisec;
        this.bpm = bpm;
        this.speed = speed;
        this.beatMetor = beatMetor;
        this.beatUnit = beatUnit;
    }

    public void Copy(Timming copy)
    {
        this.millisec = copy.millisec;
        this.bpm = copy.bpm;
        this.speed = copy.speed;
        this.beatMetor = copy.beatMetor;
        this.beatUnit = copy.beatUnit;
    }
}