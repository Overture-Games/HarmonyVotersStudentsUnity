using Proyecto26;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StudentGameController : MonoBehaviour
{
    public TextMeshProUGUI instrumentText;  
    public TextMeshProUGUI roundNumberText;  
    public Button buttonA;
    public Button buttonB;
    public Button buttonC;
    public Button buttonD;
    public GameObject currentInstrumentDisplay;

    public Color highlightedColor = Color.green;  
    public Color defaultColor = Color.white; 

    public string sessionCode;  
    public string gameMode;
    private string firebaseUrl = "https://harmonyvoters-default-rtdb.firebaseio.com/sessions/";

    private int currentRound = 1;  
    private float checkInterval = 2f; 

    public RectTransform socketA;
    public RectTransform socketB;

    private Vector2 socket1;
    private Vector2 socket2;

    public GameObject FinalPanel;
    private bool hasHandledRound = false;

    public GameObject blockPanel;

    void Start()
    {
        sessionCode = SessionManager.Instance.GetSessionCode();

        if (string.IsNullOrEmpty(sessionCode))
        {
            Debug.LogError("No session code found!");
        }

        Debug.Log("Session Code retrieved: " + sessionCode);

        buttonA.onClick.AddListener(() => VoteOption("A"));
        buttonB.onClick.AddListener(() => VoteOption("B"));
        buttonC.onClick.AddListener(() => VoteOption("C"));
        buttonD.onClick.AddListener(() => VoteOption("D"));

        StartCoroutine(CheckRoundStatus());  
        socket1 = socketA.anchoredPosition;
        socket2 = socketB.anchoredPosition;
        buttonA.interactable = false;
        buttonB.interactable = false;
        instrumentText.text = "Bass";
        
        StartCoroutine(GetGameMode());
    }
    IEnumerator GetGameMode(){
        string url = firebaseUrl + sessionCode + ".json";

        yield return RestClient.Get<Session>(url).Then(response =>
        {
            gameMode = response.gameMode;
            SessionManager.Instance.SetGameMode(gameMode);

        }).Catch(error =>
        {
            Debug.LogError("Error fetching currentRound data: " + error.Message);
        });
    }
    IEnumerator CheckRoundStatus()
    {
        while (true)  
        {
            yield return StartCoroutine(GetCurrentRound());  
            yield return new WaitForSeconds(checkInterval);  
        }
    }

    IEnumerator GetCurrentRound()
    {
        string url = firebaseUrl + sessionCode + "/currentRound.json";

        yield return RestClient.Get<RoundData>(url).Then(response =>
        {
            if (response != null)
            {
                if (response.numberRound != currentRound)
                {
                    currentRound = response.numberRound;
                    hasHandledRound = false; 

                    if (currentRound <= 3)
                    {
                        instrumentText.text = "Bass";
                    }
                    else if (currentRound <= 6)
                    {
                        instrumentText.text = "Chords";
                    }
                    else if (currentRound <= 9)
                    {
                        instrumentText.text = "Melody";
                    }
                }
          
                if (response.isRoundActive)
                {
                    blockPanel.SetActive(false);
                }
                else
                {
                    blockPanel.SetActive(true);
                }

                if (response.isFinishGame)
                {
                    FinalPanel.gameObject.SetActive(true);  
                }
                if (!hasHandledRound)
                {
                    switch (((currentRound - 1) % 3 + 1))
                    {
                        case 1: 
                            HandleRoundAB();
                            break;
                        case 2: 
                            HandleRoundWinnerVsC(response);
                            break;
                        case 3: 
                            HandleRoundWinnerVsD(response);
                            break;
                        default:
                            Debug.LogError("Invalid round!");
                            break;
                    }
                    hasHandledRound = true;
                }
            }
            else
            {
                Debug.LogError("Error: currentRound data not found in Firebase.");
            }
        }).Catch(error =>
        {
            Debug.LogError("Error fetching currentRound data: " + error.Message);
        });
    }

    private void HandleRoundAB()
    {
        roundNumberText.text = "Round 1";
        buttonA.GetComponent<Image>().color = defaultColor;
        buttonB.GetComponent<Image>().color = defaultColor;

        buttonA.gameObject.SetActive(true);
        buttonB.gameObject.SetActive(true);
        buttonA.interactable = true;
        buttonB.interactable = true;
        buttonC.gameObject.SetActive(false);
        buttonD.gameObject.SetActive(false);
        buttonA.GetComponent<RectTransform>().anchoredPosition = socket1;
        buttonB.GetComponent<RectTransform>().anchoredPosition = socket2;
    }

    // Maneja la Ronda 2 (Ganador vs. C)
    private void HandleRoundWinnerVsC(RoundData response)
    {

        roundNumberText.text = "Round 2";
        buttonA.GetComponent<Image>().color = defaultColor;
        buttonB.GetComponent<Image>().color = defaultColor;
        buttonC.GetComponent<Image>().color = defaultColor;

        if (response.finalWinner == "A")
        {
            buttonA.gameObject.SetActive(true);
            buttonC.gameObject.SetActive(true);
            buttonA.interactable = true;
            buttonC.interactable = true;
            buttonB.gameObject.SetActive(false);
            buttonA.GetComponent<RectTransform>().anchoredPosition = socket1;
            buttonC.GetComponent<RectTransform>().anchoredPosition = socket2;
        }
        else if (response.finalWinner == "B")
        {
            buttonB.gameObject.SetActive(true);
            buttonC.gameObject.SetActive(true);
            buttonB.interactable = true;
            buttonC.interactable = true;
            buttonA.gameObject.SetActive(false);
            buttonB.GetComponent<RectTransform>().anchoredPosition = socket1;
            buttonC.GetComponent<RectTransform>().anchoredPosition = socket2;
        }
    }

    // Maneja la Ronda 3 (Ganador vs. D)
    private void HandleRoundWinnerVsD(RoundData response)
    {

        roundNumberText.text = "Final Round"; 
        buttonA.GetComponent<Image>().color = defaultColor;
        buttonB.GetComponent<Image>().color = defaultColor;
        buttonC.GetComponent<Image>().color = defaultColor;
        buttonD.GetComponent<Image>().color = defaultColor;
        if (response.finalWinner == "A")
        {
            buttonA.gameObject.SetActive(true);
            buttonD.gameObject.SetActive(true);
            buttonA.interactable = true;
            buttonD.interactable = true;
            buttonB.gameObject.SetActive(false);
            buttonC.gameObject.SetActive(false);
            buttonA.GetComponent<RectTransform>().anchoredPosition = socket1;
            buttonD.GetComponent<RectTransform>().anchoredPosition = socket2;

        }
        else if (response.finalWinner == "B")
        {

            buttonB.gameObject.SetActive(true);
            buttonD.gameObject.SetActive(true);
            buttonB.interactable = true;
            buttonD.interactable = true;
            buttonA.gameObject.SetActive(false);
            buttonC.gameObject.SetActive(false);
            buttonB.GetComponent<RectTransform>().anchoredPosition = socket1;
            buttonD.GetComponent<RectTransform>().anchoredPosition = socket2;
        }
        else if (response.finalWinner == "C")
        {
            buttonC.gameObject.SetActive(true);
            buttonD.gameObject.SetActive(true);
            buttonC.interactable = true;
            buttonD.interactable = true;
            buttonA.gameObject.SetActive(false);
            buttonB.gameObject.SetActive(false);
            buttonC.GetComponent<RectTransform>().anchoredPosition = socket1;
            buttonD.GetComponent<RectTransform>().anchoredPosition = socket2;
        }
    }

    void VoteOption(string option)
    {
        string url = firebaseUrl + sessionCode + "/currentRound.json";

        RestClient.Get<RoundData>(url).Then(response =>
        {
            if (response != null)
            {
                buttonA.interactable = false;
                buttonB.interactable = false;
                buttonC.interactable = false;
                buttonD.interactable = false;

                if (option == "A")
                {
                    buttonA.GetComponent<Image>().color = highlightedColor;
                    buttonB.GetComponent<Image>().color = defaultColor;
                }
                else if (option == "B")
                {
                    buttonB.GetComponent<Image>().color = highlightedColor;
                    buttonA.GetComponent<Image>().color = defaultColor;
                }
                else if (option == "C")
                {
                    buttonC.GetComponent<Image>().color = highlightedColor;
                    buttonD.GetComponent<Image>().color = defaultColor;
                }
                else if (option == "D")
                {
                    buttonD.GetComponent<Image>().color = highlightedColor;
                    buttonC.GetComponent<Image>().color = defaultColor;
                }

                if (option == "A")
                {
                    response.optionA++;  
                }
                else if (option == "B")
                {
                    response.optionB++; 
                }
                else if (option == "C")
                {
                    response.optionC++;  
                }
                else if (option == "D")
                {
                    response.optionD++;  
                }

                RestClient.Put(url, response).Then(updateResponse =>
                {
                    Debug.Log("Vote registered! Updated " + option);
                }).Catch(error =>
                {
                    Debug.LogError("Error updating vote: " + error.Message);
                });

            }
            else
            {
                Debug.LogError("Error: currentRound data not found in Firebase.");
            }
        }).Catch(error =>
        {
            Debug.LogError("Error fetching currentRound data: " + error.Message);
        });
    }
}

[System.Serializable]
public class RoundData
{
    public int optionA;
    public int optionB;
    public int optionC;
    public int optionD;
    public int numberRound;
    public bool isEndRound;
    public string finalWinner;
    public bool isRoundActive;
    public bool isFinishGame;
}
