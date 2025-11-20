using System.Collections.Generic;
using UnityEngine;
using System;

public class NoteGenerator : MonoBehaviour
{
    [SerializeField] private RectTransform[] lanes;         // 노트 스폰 X값

    [SerializeField] private RectTransform spawnline;       // 생성 Y 값
    [SerializeField] private RectTransform judgementLine;   // 판정성 Y 값

    [SerializeField] private float spawnOffset = 0f;      // 노트 스폰 시간을 조절하는 함수

    public event Action OnAllNotesSpawned;
    private bool allNotesSpawned = false;

    private NoteManager noteManager;
    private List<NoteData> noteList;

    private int currentIndex = 0; // 현재 스폰할 노트의 인덱스

    void Awake()
    {
        noteManager = FindObjectOfType<NoteManager>();
    }

    private void Start()
    {
        INoteTypeResolver resolver = new DefaultNoteTypeResolver();
        INoteDataFactory factory = new NoteDataFactory(resolver);
        var parser = new NoteParser(factory);

        noteList = parser.ParseHitObjects(GameData.selectedChartPath);
    }

    public void SpawnNotes(float currentTime)
    {
        while (currentIndex < noteList.Count)
        {
            NoteData note = noteList[currentIndex];
            float spawnTime = note.StartTime - GetTravelTime(noteManager.ScrollSpeed) - spawnOffset;

            if (currentTime >= spawnTime)
            {
                SpawnNote(note, note.LineIndex, noteManager.ScrollSpeed);
                currentIndex++;
            }
            else
            {
                break;
            }
        }

        if (!allNotesSpawned && currentIndex >= noteList.Count)
        {
            allNotesSpawned = true;
            OnAllNotesSpawned?.Invoke();
        }
    }

    private void SpawnNote(NoteData data, int laneIndex, float scrollSpeed)
    {
        GameObject noteObj = null;

        if(data.Type == Note.Long)
        {
            noteObj = NotePool.Instance.GetLongNote();
            noteObj.GetComponent<LongNote>().InitNote(data, scrollSpeed, spawnline.anchoredPosition.y, judgementLine.anchoredPosition.y);
        }
        else
        {
            noteObj = NotePool.Instance.GetShortNote();
            noteObj.GetComponent<ShortNote>().InitNote(data, scrollSpeed, spawnline.anchoredPosition.y, judgementLine.anchoredPosition.y);
        }

        noteObj.transform.SetParent(lanes[laneIndex], false);
    }

    public float GetTravelTime(float speed)
    {
        return (spawnline.anchoredPosition.y - judgementLine.anchoredPosition.y) / speed * 1000f;
    }
}
