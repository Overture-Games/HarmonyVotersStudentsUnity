using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(2);
    }
}