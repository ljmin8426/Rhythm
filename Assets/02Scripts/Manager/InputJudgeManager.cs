using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputJudgeManager : MonoBehaviour
{
    [Header("Judgement Keys")]
    [SerializeField] private KeyCode[] laneKeys; // 노트 판정 입력 키

    [Header("Judgement Window")]
    [SerializeField] private float inputWindow;  // 노트가 일정 범위에 들어왔을때 판정되는 값

    private ScoreManager scoreManager;
    private NoteManager noteManager;
    private JudgementTiming judgementTiming;

    public delegate void ChangeJudgementUI(Judge type);
    public static event ChangeJudgementUI OnChangeJudgemenUI;

    private class HoldingNoteInfo
    {
        public NoteData Note;
        public int TargetTicks;
        public int CurrentTick;
    }

    private Dictionary<int, HoldingNoteInfo> holdingNotes = new();
    private readonly float TickIntervalMS = 200f; // 1틱당 시간 간격 (200ms마다 콤보 추가)

    private void Awake()
    {
        judgementTiming = new JudgementTiming();

        scoreManager = FindAnyObjectByType<ScoreManager>();
        if (scoreManager == null)
            Debug.Log("ScoreManager is Null");

        noteManager = FindAnyObjectByType<NoteManager>();
        if (noteManager == null)
            Debug.Log("NoteManger is Null");
    }

    void Update()
    {
        float currentTime = AudioManager.Instance.GetMusicTimeMS();
        noteManager.RemoveExpiredNotes(currentTime, inputWindow);

        for (int i = 0; i < laneKeys.Length; i++)
        {
            if (Input.GetKeyDown(laneKeys[i]))
            {
                NoteData noteData = noteManager.GetNextNoteFromQueue(i, currentTime, inputWindow);

                //if (noteData == null)
                //    continue;

                if (noteData.Type == Note.Long)
                {
                    int targetTicks = Mathf.Max(1, Mathf.FloorToInt((noteData.EndTime - noteData.StartTime) / TickIntervalMS));
                    holdingNotes[i] = new HoldingNoteInfo
                    {
                        Note = noteData,
                        TargetTicks = targetTicks,
                        CurrentTick = 0
                    };

                    // 롱노트 시작  > NoteManager에 알림
                    noteManager.RegisterActiveLongNote();
                }

                var result = judgementTiming.Judge(noteData, currentTime);
                HandleJudgement(result);

                noteManager.GetNoteQueue(i)?.Dequeue();
            }

            if (Input.GetKey(laneKeys[i]))
            {
                if (holdingNotes.ContainsKey(i))
                {
                    HoldingNoteInfo info = holdingNotes[i];
                    float elapsed = currentTime - info.Note.StartTime;
                    int expectedTick = Mathf.FloorToInt(elapsed / TickIntervalMS);

                    if (expectedTick > info.CurrentTick && expectedTick <= info.TargetTicks)
                    {
                        // 새 틱마다 콤보 추가
                        info.CurrentTick = expectedTick;

                        var tickResult = new JudgementResult(Judge.Perfect, info.Note, 0);
                        HandleJudgement(tickResult); // 틱마다 콤보 추가
                    }
                }
            }

            if (Input.GetKeyUp(laneKeys[i]))
            {
                if (holdingNotes.ContainsKey(i))
                {
                    // 롱노트 처리
                    HoldingNoteInfo info = holdingNotes[i];
                    float delta = currentTime - info.Note.EndTime;
                    float absDelta = Mathf.Abs(delta);

                    JudgementResult result;
                    if (absDelta <= inputWindow)
                    {
                        result = new JudgementResult(Judge.Perfect, info.Note, delta);
                    }
                    else
                    {
                        result = new JudgementResult(Judge.Miss, info.Note, delta);
                    }

                    HandleJudgement(result);
                    noteManager.UnregisterActiveLongNote();
                    holdingNotes.Remove(i);
                }
            }
        }
    }
    public void HandleJudgement(JudgementResult result)
    {
        //if (result.Note == null)
        //{
        //    Debug.LogError("result.Note is null");
        //    return;
        //}

        SetScore(result.Type);

        scoreManager.HandleJudgementCount(result.Type);

        string dir = result.DeltaTime > 0 ? "늦음" : "빠름";
        //Debug.Log($"판정 결과: {result.Type}, 시간차: {Mathf.Abs(result.DeltaTime):F1}ms ({dir})");

        OnChangeJudgemenUI?.Invoke(result.Type);
    }

    private void SetScore(Judge judgement)
    {
        switch (judgement)
        {
            case Judge.Perfect:
                scoreManager.HandleScore(300);
                scoreManager.HandleCombo(true);
                break;
            case Judge.Great:
                scoreManager.HandleScore(100);
                scoreManager.HandleCombo(true);
                break;
            case Judge.Good:
                scoreManager.HandleScore(50);
                scoreManager.HandleCombo(true);
                break;
            case Judge.Miss:
                scoreManager.HandleCombo(false);
                break;
        }
    }
}
