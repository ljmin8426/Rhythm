using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MainMusicBar : MonoBehaviour
{
    [SerializeField] private RectTransform panel;

    [Header("Image")]
    [SerializeField] private Image selectImage;
    [SerializeField] private Image musicImage;
    [SerializeField] private Image backImage;
    [SerializeField] private Image rankImage;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI artistText;

    public void SetMusicData(Music music)
    {
        titleText.text = music.title;
        artistText.text = music.artist;

        var sprite = ImageCache.Get(music.BGPath);
        if (sprite != null)
        {
            musicImage.sprite = sprite;
            SetImageSize(musicImage, 133, 75);
        }
        else
        {
            Debug.LogWarning("이미지 로드 실패 또는 없음: " + music.BGPath);
        }

        SetSelected(false);
    }

    private void SetImageSize(Image image, float width, float height)
    {
        if (image != null)
        {
            image.rectTransform.sizeDelta = new Vector2(width, height);
        }
    }

    public void SetSelected(bool isSelected)
    {
        selectImage.enabled = isSelected;
    }
}
