using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MusicMixer : MonoBehaviour
{
    public AudioSource bassSource, chordsSource, melodySource, drumsSource;
    public AudioClip[] bassVariationsInstruments, chordsVariationsInstruments, melodyVariationsInstruments; // Modo Instruments
    public AudioClip[] bassVariationsHarmony, chordsVariationsHarmony, melodyVariationsHarmony; // Modo Harmony
    public AudioClip defaultDrumsInstruments, defaultDrumsHarmony;

    public Button[] bassButtons, chordsButtons, melodyButtons;
    public Button playButton, doneButton;

    private Dictionary<string, AudioSource> audioSources;
    private Dictionary<string, AudioClip[]> audioClips;
    private Dictionary<string, Button[]> buttonGroups;
    private Dictionary<string, int> selectedIndices;

    private bool isPlaying = false;
    private string selectedCombination = "";

    private string[] audioUrlsInstruments = new string[]
 {
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/AAA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/AAB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/AAC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/AAD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/ABA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/ABB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/ABC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/ABD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/ACA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/ACB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/ACC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/ACD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/ADA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/ADB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/ADC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/ADD.wav",

    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BAA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BAB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BAC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BAD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BBA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BBB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BBC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BBD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BCA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BCB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BCC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BCD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BDA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BDB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BDC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/BDD.wav",

    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CAA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CAB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CAC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CAD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CBA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CBB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CBC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CBD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CCA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CCB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CCC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CCD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CDA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CDB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CDC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/CDD.wav",

    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DAA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DAB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DAC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DAD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DBA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DBB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DBC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DBD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DCA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DCB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DCC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DCD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DDA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DDB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DDC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixInstruments/DDD.wav"
 };


  private string[] audioUrlsHarmony = new string[]
{
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/AAA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/AAB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/AAC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/AAD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/ABA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/ABB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/ABC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/ABD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/ACA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/ACB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/ACC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/ACD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/ADA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/ADB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/ADC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/ADD.wav",

    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BAA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BAB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BAC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BAD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BBA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BBB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BBC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BBD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BCA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BCB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BCC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BCD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BDA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BDB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BDC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/BDD.wav",

    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CAA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CAB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CAC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CAD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CBA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CBB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CBC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CBD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CCA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CCB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CCC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CCD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CDA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CDB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CDC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/CDD.wav",

    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DAA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DAB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DAC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DAD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DBA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DBB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DBC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DBD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DCA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DCB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DCC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DCD.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DDA.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DDB.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DDC.wav",
    "https://raw.githubusercontent.com/Overture-Games/HarmonyVotersStudentsUnity/main/Assets/Music/MixHarmony/DDD.wav"
};

    void Start()
    {
        string gameMode = SessionManager.Instance.GetGameMode();

        audioSources = new Dictionary<string, AudioSource>
        {
            { "Bass", bassSource },
            { "Chords", chordsSource },
            { "Melody", melodySource },
            { "Drums", drumsSource }
        };

        audioClips = new Dictionary<string, AudioClip[]>
        {
            { "Bass", gameMode == "Harmony" ? bassVariationsHarmony : bassVariationsInstruments },
            { "Chords", gameMode == "Harmony" ? chordsVariationsHarmony : chordsVariationsInstruments },
            { "Melody", gameMode == "Harmony" ? melodyVariationsHarmony : melodyVariationsInstruments }
        };

        buttonGroups = new Dictionary<string, Button[]>
        {
            { "Bass", bassButtons },
            { "Chords", chordsButtons },
            { "Melody", melodyButtons }
        };

        selectedIndices = new Dictionary<string, int>
        {
            { "Bass", -1 },
            { "Chords", -1 },
            { "Melody", -1 }
        };

        AssignButtonListeners("Bass", bassButtons);
        AssignButtonListeners("Chords", chordsButtons);
        AssignButtonListeners("Melody", melodyButtons);

        playButton.onClick.AddListener(PlayPauseMusic);
        doneButton.onClick.AddListener(OnDoneClicked);

        drumsSource.clip = gameMode == "Harmony" ? defaultDrumsHarmony : defaultDrumsInstruments;
    }

    void AssignButtonListeners(string instrument, Button[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => { SelectInstrumentVariation(instrument, index); });
        }
    }

    void SelectInstrumentVariation(string instrument, int variationIndex)
    {
        if (audioSources.ContainsKey(instrument) && audioClips.ContainsKey(instrument))
        {
            audioSources[instrument].clip = audioClips[instrument][variationIndex];
            selectedIndices[instrument] = variationIndex;
            HighlightSelectedButton(instrument);
            UpdateSelectedCombination();
        }
    }

    void HighlightSelectedButton(string instrument)
    {
        if (buttonGroups.ContainsKey(instrument))
        {
            for (int i = 0; i < buttonGroups[instrument].Length; i++)
            {
                var colors = buttonGroups[instrument][i].colors;
                colors.normalColor = (i == selectedIndices[instrument]) ? Color.yellow : Color.white;
                buttonGroups[instrument][i].colors = colors;
                buttonGroups[instrument][i].GetComponent<Image>().color = colors.normalColor;
            }
        }
    }

    void UpdateSelectedCombination()
    {
        string combination = "";

        if (selectedIndices["Bass"] != -1)
            combination += (char)('A' + selectedIndices["Bass"]);
        if (selectedIndices["Chords"] != -1)
            combination += (char)('A' + selectedIndices["Chords"]);
        if (selectedIndices["Melody"] != -1)
            combination += (char)('A' + selectedIndices["Melody"]);

        selectedCombination = combination;
    }

    void PlayPauseMusic()
    {
        if (isPlaying)
        {
            foreach (var source in audioSources.Values)
            {
                if (source.isPlaying)
                {
                    source.Stop();
                }
            }

            if (drumsSource.isPlaying)
            {
                drumsSource.Stop();
            }

            isPlaying = false;
        }
        else
        {
            foreach (var source in audioSources.Values)
            {
                if (source.clip != null && !source.isPlaying && source != drumsSource)
                {
                    source.Play();
                }
            }

            if (drumsSource.clip != null && !drumsSource.isPlaying)
            {
                drumsSource.loop = true;
                drumsSource.Play();
            }

            isPlaying = true;
        }
    }

    void OnDoneClicked()
    {
        string gameMode = SessionManager.Instance.GetGameMode();
        string[] audioUrls = gameMode == "Harmony" ? audioUrlsHarmony : audioUrlsInstruments;

        int index = CombinationToIndex(selectedCombination);
        if (index >= 0 && index < audioUrls.Length)
        {
            string audioUrl = audioUrls[index];
            Application.OpenURL(audioUrl);
        }
        else
        {
            Debug.LogError("Combination not founded " + selectedCombination);
        }
    }

    int CombinationToIndex(string combination)
    {
        if (combination.Length == 3)
        {
            int bassIndex = combination[0] - 'A';
            int chordsIndex = combination[1] - 'A';
            int melodyIndex = combination[2] - 'A';

            return bassIndex * 9 + chordsIndex * 3 + melodyIndex;
        }

        return -1;
    }
}