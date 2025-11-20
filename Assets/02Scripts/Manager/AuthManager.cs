using System;
using System.Collections.Generic;
using UnityEngine;

public class AuthManager : Singleton<AuthManager>
{
    public UserData userData;

    private void Start()
    {
        if (PlayerPrefs.HasKey("UserData01"))
        {
            LoadUserData(null);
            return;
        }
        CreateNewUser();
    }

    public void LoadUserData(Action OnSuccess = null)
    {
        string @string = PlayerPrefs.GetString("UserData01");
        userData = JsonUtility.FromJson<UserData>(@string);
        OnSuccess?.Invoke();
    }

    public void SaveUserData(Action OnSuccess = null, bool isLogin = false)
    {
        if(!isLogin)
        {
            //foreach (MusicData musicData in this.userData.musicList)
            //{
            //    this.userData.musicDictionary[musicData.musicName] = musicData;
            //}
        }
        //userData.musicDictionary_ = new SerializableDictionary();
        //foreach (KeyValuePair<string, MusicData> keyValuePair in userData.musicDictionary)
        //{
        //    userData.musicDictionary_.key.Add(keyValuePair.Key);
        //    userData.musicDictionary_.value.Add(keyValuePair.Value);
        //}
        string value = JsonUtility.ToJson(userData);
        PlayerPrefs.SetString("UserData01", value);
        PlayerPrefs.SetFloat(Data_Setting.MusicVolume.ToString(), userData.musicVolume);
        PlayerPrefs.Save();

        OnSuccess?.Invoke();
    }

    private void CreateNewUser()
    {
        userData = new UserData();
        SaveUserData(null, false);
    }
}
