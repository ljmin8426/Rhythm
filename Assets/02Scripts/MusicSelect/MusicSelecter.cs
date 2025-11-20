using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicSelecter : MonoBehaviour
{
    [SerializeField] private MusicData_ musicData;
    [SerializeField] private MusicInfo musicInfo;

    private BackGroundVideoLoader backgroundVideoLoader;
    private Music currentSelectedMusic;

    private bool isSelected = false;

    public delegate void SelectedMusic(Music music);
    public static event SelectedMusic OnSelectedMusic;

    private void Start()
    {
        backgroundVideoLoader = FindAnyObjectByType<BackGroundVideoLoader>();
    }

    void Update()
    {
        SelectInputHandle();
    }

    private void SelectInputHandle()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SelectMusic();
        }

        // 좌우 방향키로 난이도 변경
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            musicInfo.ChangeDifficulty(-1, currentSelectedMusic);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            musicInfo.ChangeDifficulty(1, currentSelectedMusic);
        }
    }

    private void SelectMusic()
    {
        if (!isSelected)
        {
            isSelected = true;

            Chart selectedChart = musicInfo.GetSelectedChart(currentSelectedMusic);

            if (selectedChart == null)
            {
                Debug.LogWarning("선택된 차트가 없습니다.");
                return;
            }
            
            GameData.selectedMusic = currentSelectedMusic;
            GameData.selectedChartPath = selectedChart.chartPath;

            Debug.Log($"SelectedMusic : {GameData.selectedMusic}, SelectedChartPath : {GameData.selectedChartPath}");

            SceneManager.LoadScene(Scene_Name.Scene_InGame.ToString());
        }
    }

    // MusicScroller에서 호출할 메서드
    public void ChangeMusic(int centerIndex)
    {
        int musicDataIndex = (centerIndex + musicData.musicList.Count) % musicData.musicList.Count;
        currentSelectedMusic = musicData.musicList[musicDataIndex];

        OnSelectedMusic?.Invoke(currentSelectedMusic);

        // 음악 및 영상 재생
        StopAllCoroutines();
        StartCoroutine(LoadMediaAsync(currentSelectedMusic));
    }

    // 비동기 로딩
    private IEnumerator LoadMediaAsync(Music music)
    {
        yield return new WaitForSeconds(1f); // 로딩 준비 시간
        backgroundVideoLoader.SetBackground(music);
        AudioManager.Instance.PlayMusic(music.audioPath);
    }
}
