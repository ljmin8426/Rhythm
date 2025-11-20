using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BackGroundVideoLoader : Singleton<BackGroundVideoLoader>
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Image previewVideo;
    [SerializeField] private Image backBlind;

    private void Start()
    {
        if (videoPlayer == null)
            videoPlayer = FindObjectOfType<VideoPlayer>();

        if (previewVideo == null)
            previewVideo = GameObject.Find("PreviewVideo")?.GetComponent<Image>();

        if (backBlind == null)
            backBlind = GameObject.Find("BackBlind")?.GetComponent<Image>();
    }

    public void SetBackground(Music music)
    {
        if (music == null)
        {
            Debug.Log("선택된 음악이 없습니다.");
            ClearBackground();
            return;
        }

        // 예외 방지: previewVideo가 파괴되었거나 null이면 동작하지 않게
        if (previewVideo == null)
        {
            Debug.LogWarning("previewVideo가 null이거나 이미 파괴됨. SetBackground 중단.");
            return;
        }

        var sprite = ImageCache.Get(music.BGPath);
        if (sprite != null)
        {
            previewVideo.sprite = sprite;
            previewVideo.gameObject.SetActive(true);
        }
        else
        {
            ClearBackground();
            Debug.Log("이미지 파일이 없습니다.");
        }
    }

    private void PlayVideo(string videoPath)
    {
        if (videoPlayer == null || previewVideo == null || backBlind == null)
        {
            Debug.LogWarning("비디오 관련 UI 컴포넌트가 null입니다.");
            return;
        }

        videoPlayer.gameObject.SetActive(true);
        previewVideo.gameObject.SetActive(false);
        backBlind.gameObject.SetActive(true);

        videoPlayer.url = videoPath;
        videoPlayer.Play();
    }

    private void ClearBackground()
    {
        if (videoPlayer != null)
            videoPlayer.gameObject.SetActive(false);

        if (previewVideo != null)
            previewVideo.gameObject.SetActive(false);

        if (backBlind != null)
            backBlind.gameObject.SetActive(true);
    }
}
