using UnityEngine;

public class JudgementTiming
{
    // 판정 범위 (ms)
    private readonly float PerfectTime = 80f;
    private readonly float GreatTime = 120f;
    private readonly float GoodTime = 160f;

    public JudgementResult Judge(NoteData note, float inputTime)
    {
        // 누른시간 - 노트시간 을 절댓값으로 변환 후 판정
        float delta = inputTime - note.StartTime;
        float absDelta = Mathf.Abs(delta);

        if (absDelta <= PerfectTime)
            return new JudgementResult(global::Judge.Perfect, note, delta);

        if (absDelta <= GreatTime)
            return new JudgementResult(global::Judge.Great, note, delta);

        if (absDelta <= GoodTime)
            return new JudgementResult(global::Judge.Good, note, delta);
        
        return new JudgementResult(global::Judge.Miss, note, delta);
    }
}