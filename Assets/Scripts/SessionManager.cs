using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;  
    public string sessionCode; 
    public string gameMode = ""; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void SetSessionCode(string code)
    {
        sessionCode = code;
    }

    public string GetSessionCode()
    {
        return sessionCode;
    }

    public void ClearSessionCode()
    {
        sessionCode = null;
    }

    public void SetGameMode(string mode)
    {
        gameMode = mode;
        Debug.Log($"Game mode set to: {gameMode}");
    }

    public string GetGameMode()
    {
        return gameMode;
    }

    public void ClearGameMode()
    {
        gameMode = "";
        Debug.Log("Game mode cleared.");
    }
}