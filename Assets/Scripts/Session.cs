[System.Serializable]
public class Session
{
    public string code;
    public string host;
    public int students_connected = 0;
    public bool gameStarted = false;
    public string gameMode;

    public Session(string code, string host)
    {
        this.code = code;
        this.host = host;
        this.students_connected = 0;
        this.gameStarted = false;
        this.gameMode = "";
    }
}