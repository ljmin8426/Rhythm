using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoteBase : MonoBehaviour
{
    protected RectTransform rect;

    protected float startTime;
    protected float scrollSpeed;
    protected float hitY;
    protected bool isMove;

    protected NoteData note;

    protected virtual void Awake() => rect = GetComponent<RectTransform>();

    protected virtual void Update()
    {
        float delta = GetDeltaTime();
        float y = hitY + delta * scrollSpeed;
        rect.anchoredPosition = new Vector2(0, y);
    }
    protected virtual float GetDeltaTime()
    {
        return (startTime - AudioManager.Instance.GetMusicTimeMS()) / 1000f;
    }


    public abstract void InitNote(NoteData data, float speed, float startY, float hitY);
    protected abstract void ReturnNote();

}
