using System;

[Serializable]
public class SheetPack
{
    public string rootFileName;
    public string sheetPath;
    public float level;
    public Difficulty diff;
    public int combo;

    public SheetPack(string rootFileName, float level, Difficulty diff, int combo, string sheetPath)
    {
        this.rootFileName = rootFileName;
        this.level = level;
        this.diff = diff;
        this.combo = combo;
        this.sheetPath = sheetPath;
    }
}
