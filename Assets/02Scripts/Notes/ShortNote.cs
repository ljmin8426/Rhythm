using Unity.VisualScripting;
using UnityEngine;

public class ShortNote : NoteBase
{
    public override void InitNote(NoteData data, float speed, float startY, float hitY)
    {
        startTime = data.StartTime;
        scrollSpeed = speed;
        this.hitY = hitY;

        rect.anchoredPosition = new Vector2(0, startY);
    }

    protected override void ReturnNote()
    {
        NotePool.Instance.ReturnNote(gameObject);
    }
}
