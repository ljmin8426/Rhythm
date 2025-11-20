using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : SingletonDestroy<InGameManager>
{
    [SerializeField] private GameObject resultPanel;  // 결과 UI (Canvas에 붙여놔야 함)

    private NoteManager noteManager;
    private ScoreManager scoreManager;

    private bool canProceed = false;
    public bool isReady = false;

    public delegate void GameReadyAction();
    public static event GameReadyAction OnGameReady;

    protected override void DoAwake()
    {
        noteManager = FindAnyObjectByType<NoteManager>();
        if (noteManager == null)
            Debug.Log("NoteManager is null");

        scoreManager = FindAnyObjectByType<ScoreManager>();
        if (scoreManager == null)
            Debug.Log("ScoreManger is null");
    }

    void Start()
    {
        StartCoroutine(PrepareGame());

        resultPanel.SetActive(false); // 시작 시 비활성화

        // NoteManager의 OnAllNotesJudged 이벤트를 구독
        noteManager.OnAllNotesJudged += HandleAllNotesJudged;
    }
    void Update()
    {
        if (canProceed && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Scene_MusicSelect");  // 씬 이름은 프로젝트에 맞게 수정
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Scene_MusicSelect");  // 씬 이름은 프로젝트에 맞게 수정
        }

        if(Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene(Scene_Name.Scene_InGame.ToString());  // 씬 이름은 프로젝트에 맞게 수정
        }
    }

    private void HandleAllNotesJudged()
    {
        Debug.Log("모든 노트 판정 완료");
        StartCoroutine(ShowResult(1f));
    }
    // 게임준비 함수 
    private IEnumerator PrepareGame()
    {
        scoreManager?.InitScore();

        // 게임 시작 전 2초 대기
        yield return new WaitForSeconds(2f);

        // 노트 파싱
        INoteTypeResolver resolver = new DefaultNoteTypeResolver();
        INoteDataFactory factory = new NoteDataFactory(resolver);
        var parser = new NoteParser(factory);

        // NoteData 리스트를 생성
        var allNotes = parser.ParseHitObjects(GameData.selectedChartPath);

        // NoteManager의 큐에 노트 데이터 추가
        foreach (var note in allNotes)
        {
            noteManager.EnqueueNote(note);
        }

        isReady = true;
        OnGameReady?.Invoke(); // 게임 준비 완료 이벤트 발생

        // 노래 재생
        AudioManager.Instance.PlayMusic(GameData.selectedMusic.audioPath);
    }
    private IEnumerator ShowResult(float delay)
    {
        yield return new WaitForSeconds(delay);
        resultPanel.SetActive(true);
        canProceed = true;
    }
}
