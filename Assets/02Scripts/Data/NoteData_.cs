using System;
using UnityEngine;

[Serializable]
public struct NoteData_ 
{
    public Note type;
    public int index;
    public double headMilliSec;
    public double tailMilliSec;

    public int GetTotalCombo(float bpm, int beatUnit)
    {
        int num = Mathf.RoundToInt(60000f / bpm * (4f / (float)beatUnit) / 16f);
        Note note = this.type;
        if (note == Note.Short)
        {
            return 1;
        }
        if (note != Note.Long)
        {
            return 0;
        }
        return 1 + Mathf.CeilToInt((float)(this.tailMilliSec - this.headMilliSec) / ((float)num * 4f));
    }
}
