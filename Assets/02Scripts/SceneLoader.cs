using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Tooltip("다음으로 로드할 씬의 빌드 인덱스 또는 이름")]
    public string nextSceneName = "YourNextSceneName"; // 유니티 인스펙터에서 씬 이름으로 변경하세요

    void Update()
    {
        // Enter 키(또는 Return 키)가 눌렸는지 확인합니다.
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadNextScene();
        }
    }

    /// <summary>
    /// 설정된 다음 씬을 로드하는 함수
    /// </summary>
    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            try
            {
                // 지정된 이름의 씬을 로드합니다.
                SceneManager.LoadScene(nextSceneName);
            }
            catch (System.Exception e)
            {
                Debug.LogError("다음 씬을 로드하는 데 실패했습니다. 씬 이름 또는 빌드 설정 확인: " + nextSceneName + "\n오류: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("로드할 다음 씬 이름이 설정되지 않았습니다. 인스펙터에서 'nextSceneName'을 설정해주세요.");
        }
    }
}