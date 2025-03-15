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
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/AAA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/AAB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/AAC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/AAD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/ABA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/ABB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/ABC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/ABD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/ACA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/ACB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/ACC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/ACD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/ADA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/ADB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/ADC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/ADD.wav",

    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BAA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BAB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BAC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BAD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BBA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BBB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BBC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BBD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BCA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BCB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BCC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BCD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BDA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BDB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BDC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/BDD.wav",

    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CAA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CAB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CAC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CAD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CBA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CBB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CBC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CBD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CCA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CCB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CCC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CCD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CDA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CDB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CDC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/CDD.wav",

    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DAA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DAB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DAC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DAD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DBA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DBB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DBC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DBD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DCA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DCB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DCC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DCD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DDA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DDB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DDC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Instruments/DDD.wav"
    };

    private string[] audioUrlsHarmony = new string[]
    {
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/AAA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/AAB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/AAC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/AAD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/ABA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/ABB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/ABC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/ABD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/ACA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/ACB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/ACC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/ACD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/ADA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/ADB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/ADC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/ADD.wav",

    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BAA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BAB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BAC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BAD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BBA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BBB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BBC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BBD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BCA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BCB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BCC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BCD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BDA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BDB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BDC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/BDD.wav",

    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CAA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CAB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CAC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CAD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CBA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CBB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CBC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CBD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CCA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CCB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CCC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CCD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CDA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CDB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CDC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/CDD.wav",

    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DAA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DAB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DAC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DAD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DBA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DBB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DBC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DBD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DCA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DCB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DCC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DCD.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DDA.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DDB.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DDC.wav",
    "https://eddieborbon.com/OvertureGames/HarmonyVoters/Harmony/DDD.wav"
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
            Debug.LogError("Combinación no válida o no encontrada: " + selectedCombination);
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