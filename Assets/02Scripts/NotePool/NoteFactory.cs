using UnityEngine;

public class NoteFactory : INoteFactory
{
    private readonly GameObject prefab;

    public NoteFactory(GameObject prefab)
    {
        this.prefab = prefab;
    }

    public GameObject CreateNote(Transform parent)
    {
        var note = Object.Instantiate(prefab, parent);
        note.SetActive(false);
        return note;
    }
}
