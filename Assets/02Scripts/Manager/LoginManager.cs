using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [SerializeField] GameObject fade;

    private void Awake()
    {
        Cursor.visible = false;
        fade.SetActive(true);
    }

    private void Start()
    {
        DisplayManager.InitDisplayOptions();
        BundlePackage.Instance.MusicBundle.Init();
    }

    public void StartGame()
    {
        if (Fader.isFading)
            return;

        AuthManager.Instance.SaveUserData(null, true);

        Fader.Instance.FadeOut(() =>
        {
            IntroBGAManager.Instance.StopBGA();
            SceneManager.LoadScene(Scene_Name.Scene_MusicSelect.ToString());
        });
    }
}

