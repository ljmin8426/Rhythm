using System.IO;

public class ChartParser : IParser<Chart>
{
    public Chart Parse(string path)
    {
        var chart = new Chart { chartPath = path };
        int noteCount = 0;

        using var reader = new StreamReader(path);
        string line;
        ChartSection current = ChartSection.None;

        while ((line = reader.ReadLine()) != null)
        {
            line = line.Trim();

            if (line.StartsWith("["))
                current = ParseSection(line);

            if (current == ChartSection.Metadata)
            {
                if (line.StartsWith("Title:")) chart.title = GetValue(line);
                else if (line.StartsWith("Artist:")) chart.artist = GetValue(line);
                else if (line.StartsWith("Version:")) { chart.diff = GetValue(line); chart.ParseDifficulty(); }
            }

            if (current == ChartSection.Difficulty && line.StartsWith("CircleSize:"))
                chart.circleSize = int.Parse(GetValue(line));

            if (current == ChartSection.TimingPoints)
            {
                var parts = line.Split(',');
                if (parts.Length >= 3)
                {
                    chart.offSet = int.Parse(parts[0]);
                    chart.BPM = (int)float.Parse(parts[1]);
                    chart.meter = int.Parse(parts[2]);
                    current = ChartSection.None;
                }
            }

            if (current == ChartSection.HitObjects && !string.IsNullOrWhiteSpace(line))
                noteCount++;
        }

        chart.noteCount = noteCount - 1;
        return chart;
    }

    private ChartSection ParseSection(string line)
    {
        return line switch
        {
            "[Metadata]" => ChartSection.Metadata,
            "[Difficulty]" => ChartSection.Difficulty,
            "[TimingPoints]" => ChartSection.TimingPoints,
            "[HitObjects]" => ChartSection.HitObjects,
            _ => ChartSection.None,
        };
    }

    private string GetValue(string line) => line.Split(':')[1].Trim();

    private enum ChartSection
    {
        None,
        Metadata,
        Difficulty,
        TimingPoints,
        HitObjects 
    }
}
