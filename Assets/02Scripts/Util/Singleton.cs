using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(Instance);

            DoAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected virtual void DoAwake() { }
}

public class SingletonDestroy<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DoAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected virtual void DoAwake() { }
}