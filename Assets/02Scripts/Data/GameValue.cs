using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameValue
{
    public class LastPlayed
    {
        public string id;

        public Difficulty diff;

    }

    public static GameValue.LastPlayed lastPlayed;

    public static bool isShowInfoDetail;

    public static float connectSecond;

    public static BaseMusicBundle currentBundle = null;

    public static bool isPlayRandomMusic;

    public static bool isPlayHiddenMusic;

    public static bool isPlayHiddenBlind;

    public static void Save()
    {
        PlayerPrefs.SetString("LASTPLAYBUNDLE", JsonUtility.ToJson(GameValue.lastPlayed));
        PlayerPrefs.SetInt("INFO_DETAIL", GameValue.isShowInfoDetail ? 1 : 0);
    }

    static GameValue()
    {

        GameValue.isShowInfoDetail = (PlayerPrefs.GetInt("INFO_DETAIL", 0) == 1);
        try
        {
            if (!PlayerPrefs.HasKey("LASTPLAYBUNDLE"))
            {
                GameValue.lastPlayed = new GameValue.LastPlayed
                {
                    id = "RANDOM",
                    diff = Difficulty.Easy
                };
            }
            else
            {
                GameValue.lastPlayed = JsonUtility.FromJson<GameValue.LastPlayed>(PlayerPrefs.GetString("LASTPLAYBUNDLE", string.Empty));
            }
            if (GameValue.lastPlayed == null)
            {
                GameValue.lastPlayed = new GameValue.LastPlayed();
            }
        }
        catch (Exception message)
        {
            Debug.Log(message);
            GameValue.lastPlayed = new GameValue.LastPlayed();
        }
        GameValue.connectSecond = 0f;
    }
}
