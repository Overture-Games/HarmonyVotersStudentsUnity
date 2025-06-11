using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void StaticInit()
    {
        Debug.Log("Resetting " + typeof(T).Name);
        _instance = null;
    }

    [SerializeField] private bool dontDestroyOnLoad = true;

    private static T _instance = null;

    public static T Instance => _instance ? _instance : _instance = FindFirstObjectByType<T>();

    public static bool IsLoaded => _instance;

    protected virtual void Awake()
    {
        if (_instance && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this as T;
        if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
    }
}