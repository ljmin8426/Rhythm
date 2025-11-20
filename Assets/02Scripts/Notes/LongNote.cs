using Unity.VisualScripting;
using UnityEngine;

public class LongNote : NoteBase
{
    private float endTime;

    public override void InitNote(NoteData data, float speed, float startY, float hitY)
    {
        startTime = data.StartTime;
        endTime = data.EndTime;
        scrollSpeed = speed;
        this.hitY = hitY;

        rect.anchoredPosition = new Vector2(0, startY);

        float height = Mathf.Abs((endTime - startTime) / 1000f * scrollSpeed);
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
    }

    protected override void ReturnNote()
    {
        NotePool.Instance.ReturnNote(gameObject);
    }
}
