public struct NoteData
{
    public int LineIndex { get; }                       // 노트 라인
    public float StartTime { get; }                     // 노트 판정 시간
    public float EndTime { get; }                       // 노트 끝 시간
    public Note Type { get; }                       // 노트의 타입 (롱, 숏)

    public NoteData(int lineIndex, float startTime, Note type, float endTime)
    {
        LineIndex = lineIndex;
        StartTime = startTime;
        Type = type;
        EndTime = endTime;
    }
}