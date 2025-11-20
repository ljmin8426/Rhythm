using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class MusicInfo : MonoBehaviour
{
    [Header("곡 정보")]
    [SerializeField] private Image musicImage;
    [SerializeField] private TextMeshProUGUI musicTitle;
    [SerializeField] private TextMeshProUGUI musicArtist;
    [SerializeField] private TextMeshProUGUI bpmText;
    [SerializeField] private TextMeshProUGUI comboText;

    [Header("난이도")]
    [SerializeField] private GameObject[] difficultyIcons; // 0: Easy, 1: Normal, 2: Hard, 3: Insane

    private int selectedDifficultyIndex = 0;

    private string selectedChartName = "";

    private void OnEnable()
    {
        MusicSelecter.OnSelectedMusic += SetMusicInfo;
    }

    private void OnDisable()
    {
        MusicSelecter.OnSelectedMusic -= SetMusicInfo;
    }

    public string GetSelectedChartName() => selectedChartName; // 이름 getter 함수
    public Chart GetSelectedChart(Music music)
    {
        foreach (var chart in music.charts)
        {
            if (chart.enumDiff.ToString() == selectedChartName)
            {
                return chart;
            }
        }

        Debug.LogWarning($"[MusicInfo] {music.title}에서 {selectedChartName} 난이도 차트를 찾지 못했습니다.");
        return null;
    }
    public void SetMusicInfo(Music music)
    {
        if (music == null) return;

        musicTitle.text = music.title;
        musicArtist.text = music.artist;
        bpmText.text = music.charts.Count > 0 ? music.charts[0].BPM.ToString() : "N/A";

        var sprite = ImageCache.Get(music.BGPath);
        if (sprite != null)
        {
            // 이미지 설정
            musicImage.sprite = sprite;
            SetImageSize(musicImage, 500, 280);
        }
        else
        {
            Debug.LogWarning("이미지 로드 실패 또는 없음: " + music.BGPath);
        }

        selectedDifficultyIndex = 0;
        SetDifficultyIcons(music);
        UpdateDifficultySelection(music);
    }
    private void SetImageSize(Image image, float width, float height)
    {
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(width, height);
        }
    }

    private Sprite LoadSpriteFromFile(string path)
    {
        byte[] imageBytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    private void SetDifficultyIcons(Music music)
    {
        // 먼저 모든 아이콘을 비활성화
        for (int i = 0; i < difficultyIcons.Length; i++)
        {
            if (difficultyIcons[i] != null)
            {
                // 아이콘 전부 비활성화
                difficultyIcons[i].SetActive(false); 
                Image iconImage = difficultyIcons[i].GetComponent<Image>();
                if (iconImage != null)
                {
                    // 이미지 아이콘 FILL 전부 비활성화
                    iconImage.fillCenter = false;
                }
            }
        }

        // 선택 Music에 있는 charts를 돌면서 난이도 채보가 있는 아이콘을 활성화 
        foreach (Chart chart in music.charts)
        {
            int index = (int)chart.enumDiff;
            if (index >= 0 && index < difficultyIcons.Length && difficultyIcons[index] != null)
            {
                difficultyIcons[index].SetActive(true);
            }
        }
    }


    private void UpdateDifficultySelection(Music music)
    {
        //music.charts에서 난이도(enumDiff)를 정수로 변환해서 중복 없이 리스트로 뽑아낸다
        var availableDifficulties = music.charts.Select(chart => (int)chart.enumDiff).Distinct().ToList();
        //오름차순 정렬함
        availableDifficulties.Sort();

        // 선택한 곡의 없는 난이도면 가장 낮은 난이도로 설정한다
        if (!availableDifficulties.Contains(selectedDifficultyIndex))
        {
            selectedDifficultyIndex = availableDifficulties.FirstOrDefault();
        }

        // 아이콘을 전부 돌면서 채보 파일이 있는 인덱스만 활성화 시킨다
        for (int i = 0; i < difficultyIcons.Length; i++)
        {
            if (difficultyIcons[i] != null)
            {
                Image iconImage = difficultyIcons[i].GetComponent<Image>();
                if (iconImage != null)
                {
                    iconImage.fillCenter = (i == selectedDifficultyIndex);
                }
            }
        }

        // selectedChartName을 변경
        Chart selectedChart = music.charts.FirstOrDefault(chart => (int)chart.enumDiff == selectedDifficultyIndex);
        selectedChartName = selectedChart != null ? selectedChart.diff : "";

        if (comboText != null && selectedChart != null)
        {
            comboText.text = $"{selectedChart.noteCount}";
        }
    }

    public void ChangeDifficulty(int direction, Music music)
    {
        if (music == null || music.charts.Count == 0)
            return;

        // 무한 루프 방지용 카운터.
        // 모든 난이도 아이콘을 한 번씩 돌아볼 수 있는 최대 횟수.
        int maxTries = difficultyIcons.Length;

        // do : while 문 전에 한번은 무조건 실행됨
        do
        {
            // 음수 인덱스 방지
            selectedDifficultyIndex = (selectedDifficultyIndex + direction + difficultyIcons.Length) % difficultyIcons.Length;
            maxTries--;
        }
        // 지금 인덱스에 해당하는 난이도가 실제로 존재하지 않으면 계속 도는 조건
        while (!music.charts.Any(chart => (int)chart.enumDiff == selectedDifficultyIndex) && maxTries > 0);

        UpdateDifficultySelection(music);
    }
}
