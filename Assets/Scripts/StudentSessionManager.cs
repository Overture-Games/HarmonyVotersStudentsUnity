using Proyecto26;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StudentSessionManager : MonoBehaviour
{
    public TMP_InputField inputSessionCode; 
    public TextMeshProUGUI statusText;
    public Button joinBtn; 

    private string firebaseUrl = "https://harmonyvoters-default-rtdb.firebaseio.com/sessions/";
    private string sessionCode; 

    void Start()
    {
        joinBtn.onClick.AddListener(JoinSession);
    }

    void JoinSession()
    {
        sessionCode = inputSessionCode.text.Trim(); 

        if (string.IsNullOrEmpty(sessionCode))
        {
            Debug.LogError("Session code cannot be empty!");
            return;
        }

        string url = firebaseUrl + sessionCode + ".json";
        RestClient.Get<Session>(url).Then(response =>
        {
            if (response != null)
            {
                // Incrementar el contador de estudiantes conectados
                response.students_connected++;

                // Guardar la sesión actualizada en Firebase
                RestClient.Put(url, response).Then(updateResponse =>
                {
                    Debug.Log("Student connected successfully! Updated students_connected.");

                    if (statusText != null)
                    {
                        statusText.text = "Connected";
                    }

                    if (joinBtn != null)
                    {
                        joinBtn.interactable = false; 
                    }

                    // Almacenar el código de sesión y el game mode en el SessionManager
                    SessionManager.Instance.SetSessionCode(sessionCode);
                    SessionManager.Instance.SetGameMode(response.gameMode);

                    Debug.Log($"Game mode retrieved: {response.gameMode}");

                    // Iniciar la verificación del estado del juego
                    StartCoroutine(CheckGameStarted());
                }).Catch(error =>
                {
                    Debug.LogError("Error updating students_connected: " + error.Message);
                });
            }
            else
            {
                Debug.LogError("Invalid session code!");
                if (statusText != null)
                {
                    statusText.text = "Invalid Code";
                }
            }
        }).Catch(error =>
        {
            Debug.LogError("Error fetching session data: " + error.Message);
        });
    }

    IEnumerator CheckGameStarted()
    {
        while (true)
        {
            string url = firebaseUrl + sessionCode + ".json";
            yield return RestClient.Get<Session>(url).Then(response =>
            {
                if (response != null)
                {
                    if (response.gameStarted)
                    {
                        Debug.Log("Game has started, switching to GameScene.");
                        SceneManager.LoadScene("GameScene");
                        StopCoroutine(CheckGameStarted()); 
                    }
                }
                else
                {
                    Debug.LogError("Session not found in Firebase.");
                }
            }).Catch(error =>
            {
                Debug.LogError("Error checking gameStarted: " + error.Message);
            });

            yield return new WaitForSeconds(1f); 
        }
    }
}