using UnityEngine;
using TMPro;

public class JudgementUIManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI judgementText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        ScoreManager.OnChangeScore += UpdateScore;
        ScoreManager.OnChangeCombo += UpdateCombo;
        InputJudgeManager.OnChangeJudgemenUI += ShowJudgement;
    }

    private void OnDisable()
    {
        ScoreManager.OnChangeScore -= UpdateScore;
        ScoreManager.OnChangeCombo -= UpdateCombo;
        InputJudgeManager.OnChangeJudgemenUI -= ShowJudgement;
    }

    private void Start()
    {
        judgementText.gameObject.SetActive(false);
        comboText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }


    private void UpdateScore(int score)
    {
        scoreText.gameObject.SetActive(true);
        scoreText.text = score.ToString();
    }
    private void UpdateCombo(int score)
    {
        if (score > 0)
        {
            comboText.gameObject.SetActive(true);
            comboText.text = score.ToString();
        }
        else
        {
            comboText.gameObject.SetActive(false);
        }
    }
    private void ShowJudgement(Judge type)
    {
        judgementText.gameObject.SetActive (true);
        judgementText.text = type.ToString();
    }
}
