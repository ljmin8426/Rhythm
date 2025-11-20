using System.Collections.Generic;
using UnityEngine;

public class NotePool : SingletonDestroy<NotePool>
{
    [Header("Note Prefabs")]
    [SerializeField] private GameObject shortNotePrefab;
    [SerializeField] private GameObject longNotePrefab;

    [Header("Parents")]
    [SerializeField] private Transform shortNoteParent;
    [SerializeField] private Transform longNoteParent; 

    [Header("Pool Size")]
    [SerializeField] private int shortNotePoolSize = 16;
    [SerializeField] private int longNotePoolSize = 16;

    private Queue<GameObject> shortNotePool = new Queue<GameObject>();
    private Queue<GameObject> longNotePool = new Queue<GameObject>();


    public GameObject GetShortNote() => GetNote(shortNotePool, shortNotePrefab, shortNoteParent);
    public GameObject GetLongNote() => GetNote(longNotePool, longNotePrefab, longNoteParent);

    public void InitPools()
    {
        for (int i = 0; i < shortNotePoolSize; i++)
            shortNotePool.Enqueue(CreateNote(shortNotePrefab, shortNoteParent));

        for (int i = 0; i < longNotePoolSize; i++)
            longNotePool.Enqueue(CreateNote(longNotePrefab, longNoteParent));
    }

    private GameObject CreateNote(GameObject prefab, Transform parent)
    {
        var note = Instantiate(prefab, parent);
        note.SetActive(false);
        return note;
    }

    private GameObject GetNote(Queue<GameObject> pool, GameObject prefab, Transform parent)
    {
        GameObject note = pool.Count > 0 ? pool.Dequeue() : CreateNote(prefab, parent);
        note.SetActive(true);
        return note;
    }

    public void ReturnNote(GameObject note)
    {
        note.SetActive(false);

        if (note.TryGetComponent<ShortNote>(out _))
        {
            note.transform.SetParent(shortNoteParent, false);
            shortNotePool.Enqueue(note);
        }
        else if (note.TryGetComponent<LongNote>(out _))
        {
            note.transform.SetParent(longNoteParent, false);
            longNotePool.Enqueue(note);
        }
        else
        {
            Debug.LogWarning("Unknown note type returned to pool.");
        }
    }
}
