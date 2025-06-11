using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void OnRestartButtonClicked()
    {
        SceneTransition.LoadSceneAsync(2);
    }
}