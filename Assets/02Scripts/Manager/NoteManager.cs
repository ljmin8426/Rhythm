using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
	[Header("General NoteSpeed")]
	[SerializeField] private float scrollSpeed; // 노트 이동속도를 조절하는 함수

	private NoteGenerator spawner;

    private int activeLongNoteCount = 0;   // 활성화된 롱노트 수 추적
    public float ScrollSpeed => scrollSpeed;

    public event Action OnAllNotesJudged;  // 모든 노트 판정 완료 이벤트
    private bool isAllJudged = false;      // 판정 완료 상태

    private Dictionary<int, Queue<NoteData>> noteQueues = new();

    // 롱노트 등록/해제 함수
    public void RegisterActiveLongNote() => activeLongNoteCount++;
    public void UnregisterActiveLongNote() => activeLongNoteCount--;

    private void Awake()
    {
        spawner = FindAnyObjectByType<NoteGenerator>();
    }

    private void Update()
	{
        if (InGameManager.Instance.isReady)
        {
            float currentTime = AudioManager.Instance.GetMusicTimeMS();
            spawner.SpawnNotes(currentTime);

            // 모든 큐 비었고 롱노트도 끝났을 때 이벤트
            if (!isAllJudged && AreAllQueuesEmpty() && activeLongNoteCount == 0)
            {
                isAllJudged = true;
                Debug.Log("모든 노트 판정 완료");
                OnAllNotesJudged?.Invoke();
            }
        }
    }
    // 모든 큐가 비었는지 확인하는 함수
    private bool AreAllQueuesEmpty()
    {
        foreach (var queue in noteQueues.Values)
        {
            if (queue.Count > 0)  // 큐가 하나라도 비어 있지 않으면 false 반환
                return false;
        }
        return true;  // 모든 큐가 비어 있으면 true 반환
    }
    public NoteData GetNextNoteFromQueue(int lane, float currentTime, float window)
	{
		if (!noteQueues.ContainsKey(lane) || noteQueues[lane].Count == 0)
		{
			Debug.Log($"{noteQueues.ContainsKey(lane)} 라인 큐가 존재하지 않음");
			//return null;
		}

		var nextNote = noteQueues[lane].Peek();  // 큐에서 첫 번째 노트 확인

		float timeDiff = Mathf.Abs(nextNote.StartTime - currentTime);
		if (timeDiff <= window)  // 주어진 윈도우 시간 내에 있으면 그 노트를 반환
		{
			return nextNote;
		}
        //return null;  // 윈도우 시간이 넘으면 null 반환
        return nextNote;
	}
	public Queue<NoteData> GetNoteQueue(int laneIndex)
	{
		// laneIndex에 해당하는 큐가 없으면 빈 큐를 반환
		return noteQueues.ContainsKey(laneIndex) ? noteQueues[laneIndex] : new Queue<NoteData>();
	}
    public void RemoveExpiredNotes(float currentTime, float window)
    {
        foreach (var kvp in noteQueues)
        {
            Queue<NoteData> queue = kvp.Value;

            while (queue.Count > 0)
            {
                var note = queue.Peek();
                if (note.StartTime + window < currentTime)
                {
                    queue.Dequeue();
                    Debug.Log($"[Miss] {note.LineIndex}번 라인에서 노트 자동 제거됨 (타임 아웃)");
                }
                else
                {
                    break; // 현재 노트는 아직 유효함
                }
            }
        }
    }
    // InGameManager에서 노트 데이터를 받아 해당 라인 큐에 추가하는 함수
    public void EnqueueNote(NoteData note)
    {
        // 인큐하기 전 라인 큐가 없으면 해당 Int 값의 라인 큐를 추가
        if (!noteQueues.ContainsKey(note.LineIndex))
        {
            noteQueues[note.LineIndex] = new Queue<NoteData>();
        }
        // 해당 라인에 인큐
        noteQueues[note.LineIndex].Enqueue(note);
    }
}
