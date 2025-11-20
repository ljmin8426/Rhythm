using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 현재 스크롤 상태는 아이템을 계속 재 배치해 주고 있음
// 처음에 배치를 하고 배치한 자리에 데이터만 바꾸는 형식으로 바꿔야함

public class MainMusicListScroller : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject musicListBarPrefab;
    [SerializeField] private MusicData_ musicData;

    [Header("UI Setting")]
    [SerializeField] private int itemHeight = 85;
    [SerializeField] private int visibleCount = 9;

    [Header("Input")]
    [SerializeField] private float keyRepeatDelay = 0.1f;

    private RectTransform content;
    private MusicSelecter musicSelecter;

    private float keyHoldTimer = 0f;
    private bool isScrolling = false;

    private int centerIndex = 0;
    private int totalItemCount;

    private bool isAble = true;

    private List<MainMusicBar> itemPool = new List<MainMusicBar>();


    private void Awake()
    {
        if (!TryGetComponent<MusicSelecter>(out musicSelecter))
            Debug.Log("MusicSelecter Null");

        content = GetComponent<ScrollRect>().content;

        totalItemCount = musicData.musicList.Count;
    }

    private void Start()
    {
        CreateItemPool();
        RebuildItems();
    }

    private void Update()
    {
        // 키 입력 감지
        if (isAble)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        bool pressDown = Input.GetKeyDown(KeyCode.DownArrow);
        bool pressUp = Input.GetKeyDown(KeyCode.UpArrow);
        bool holdDown = Input.GetKey(KeyCode.DownArrow);
        bool holdUp = Input.GetKey(KeyCode.UpArrow);

        if ((pressDown || (holdDown && keyHoldTimer <= 0f)) && !isScrolling)
        {
            Scroll(1);
            keyHoldTimer = pressDown ? 0.2f : keyRepeatDelay;
        }
        else if ((pressUp || (holdUp && keyHoldTimer <= 0f)) && !isScrolling)
        {
            Scroll(-1);
            keyHoldTimer = pressUp ? 0.2f : keyRepeatDelay;
        }

        if (holdDown || holdUp)
            keyHoldTimer -= Time.deltaTime;
    }

    private void CreateItemPool()
    {
        // 콘텐츠 안에 있는 자식들을 전부 삭제
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < visibleCount; i++)
        {
            GameObject obj = Instantiate(musicListBarPrefab, content);
            obj.name = $"MusicBar{i}";

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.pivot = new Vector2(0.5f, 1f);

            MainMusicBar bar = obj.GetComponent<MainMusicBar>();
            itemPool.Add(bar);
        }

        musicSelecter.ChangeMusic(centerIndex);
    }

    private void Scroll(int direction)
    {
        isScrolling = true;
        // 센터 인덱스 변경
        centerIndex = (centerIndex + direction + totalItemCount) % totalItemCount;
        StartCoroutine(DelayedRebuildItems());
    }

    private IEnumerator DelayedRebuildItems()
    {
        yield return new WaitForSeconds(0.05f);
        RebuildItems();
        isScrolling = false;
    }

    private void RebuildItems()
    {
        int halfCount = itemPool.Count / 2;
        float centerY = -340f;

        for (int i = 0; i < itemPool.Count; i++)
        {
            int offset = i - halfCount;
            int musicIndex = (centerIndex + offset + totalItemCount) % totalItemCount;

            MainMusicBar item = itemPool[i];

            RectTransform rt = item.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, -offset * itemHeight + centerY);

            item.SetMusicData(musicData.musicList[musicIndex]);
            item.SetSelected(offset == 0);
        }

        musicSelecter.ChangeMusic(centerIndex);
    }
}
