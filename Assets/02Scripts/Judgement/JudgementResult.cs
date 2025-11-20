public struct JudgementResult
{
    public NoteData Note { get; }
    public Judge Type { get; }
    public float DeltaTime { get; }

    public JudgementResult(Judge type, NoteData note, float deltaTime)
    {
        Type = type;
        Note = note;
        DeltaTime = deltaTime;
    }
}