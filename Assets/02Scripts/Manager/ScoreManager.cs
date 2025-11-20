using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public delegate void ScoreChange(int score);

    public static event ScoreChange OnChangeScore;
    public static event ScoreChange OnChangeCombo;
    public static event ScoreChange OnChangeHP;
    public static event ScoreChange OnChangeJudgementCount;

    public static event Action OnDiedPlayer;

    private int score;

    private int curHP;
    private int maxHP;

    private int curCombo;
    private int maxCombo;

    public int Score => score;
    public int CurHP => curHP;
    public int MaxHP => maxHP;
    public int CurCombo => curCombo;
    public int MaxCombo => maxCombo;

    private int miss;
    private int good;
    private int great;
    private int perfect;

    public int Miss => miss;
    public int Good => good;
    public int Great => great;
    public int Perfect => perfect;

    public void HandleJudgementCount(Judge type)
    {
        switch (type)
        {
            case Judge.Perfect:
                perfect++;
                Debug.Log("Increased Perfect" + perfect);
                OnChangeJudgementCount?.Invoke(perfect);
                break;
            case Judge.Great:
                great++;
                Debug.Log("Increased Great" + great);
                OnChangeJudgementCount?.Invoke(great);
                break;
            case Judge.Good:
                good++;
                Debug.Log("Increased Good" + good);
                OnChangeJudgementCount?.Invoke(Good);
                break;
            case Judge.Miss:
                miss++;
                Debug.Log("Increased Miss" + miss);
                OnChangeJudgementCount?.Invoke(Miss);
                break;
        }
    }

    public void InitScore()
    {
        score = 0;
        OnChangeScore?.Invoke(Score);

        curHP = maxHP = 100;
        OnChangeHP?.Invoke(CurHP);

        curCombo = 0;
        OnChangeCombo?.Invoke(CurCombo);
    }

    public void HandleScore(int point)
    {
        score += point;
        OnChangeScore.Invoke(score);
    }

    public void HandleCombo(bool isIncreased)
    {
        if (isIncreased)
        {
            curCombo++;
        }
        else
        {
            curCombo = 0;
        }
        OnChangeCombo.Invoke(curCombo);
    }

    public void ChangeHP(bool isIncreased)
    {
        if(isIncreased)
        {
            IncreaseHP();
        }
        else
        {
            DecreaseHP();
        }

        OnChangeHP?.Invoke(curHP);
    }

    private void IncreaseHP()
    {
        curHP += 5;
        if(curHP > maxHP)
        {
            curHP = maxHP;
        }
    }

    private void DecreaseHP()
    {
        curHP -= 5;
        if (curHP < 0)
        {
            curHP = 0;
            OnDiedPlayer?.Invoke();
        }
    }
}
