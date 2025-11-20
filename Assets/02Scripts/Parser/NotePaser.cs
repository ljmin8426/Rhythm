using System.Collections.Generic;
using System.IO;

// 텍스트 파일을 읽고 NoteData 리스트를 반환하는 클래스
public class NoteParser
{
    private readonly INoteDataFactory noteFactory;

    public NoteParser(INoteDataFactory factory)
    {
        this.noteFactory = factory;
    }

    public List<NoteData> ParseHitObjects(string path)
    {
        var notes = new List<NoteData>();
        bool inHitObjectSection = false;

        foreach (string rawLine in File.ReadLines(path))
        {
            string line = rawLine.Trim();

            if (line == "[HitObjects]")
            {
                inHitObjectSection = true;
                continue;
            }

            if (!inHitObjectSection || string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split(',');

            if (parts.Length < 5)
                continue;

            if (!int.TryParse(parts[0], out int x)) continue;
            if (!float.TryParse(parts[2], out float startTime)) continue;
            if (!int.TryParse(parts[3], out int noteType)) continue;

            string endTime = parts.Length > 5 ? parts[5] : "";

            NoteData note = noteFactory.Create(x, startTime, noteType, endTime);
            notes.Add(note);
        }

        notes.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
        return notes;
    }
}
