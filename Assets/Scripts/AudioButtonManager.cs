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
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/AAA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/AAB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/AAC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/AAD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/ABA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/ABB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/ABC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/ABD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/ACA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/ACB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/ACC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/ACD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/ADA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/ADB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/ADC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/ADD.wav",

    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BAA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BAB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BAC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BAD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BBA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BBB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BBC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BBD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BCA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BCB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BCC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BCD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BDA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BDB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BDC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/BDD.wav",

    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CAA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CAB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CAC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CAD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CBA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CBB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CBC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CBD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CCA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CCB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CCC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CCD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CDA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CDB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CDC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/CDD.wav",

    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DAA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DAB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DAC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DAD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DBA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DBB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DBC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DBD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DCA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DCB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarFmonyVotersStudents/Assets/MixInstruments/DCC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DCD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DDA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DDB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DDC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixInstruments/DDD.wav"
    };

    private string[] audioUrlsHarmony = new string[]
    {
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/AAA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/AAB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/AAC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/AAD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/ABA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/ABB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/ABC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/ABD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/ACA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/ACB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/ACC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/ACD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/ADA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/ADB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/ADC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/ADD.wav",

    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BAA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BAB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BAC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BAD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BBA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BBB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BBC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BBD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BCA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BCB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BCC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BCD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BDA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BDB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BDC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/BDD.wav",

    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CAA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CAB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CAC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CAD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CBA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CBB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CBC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CBD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CCA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CCB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CCC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CCD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CDA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CDB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CDC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/CDD.wav",

    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DAA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DAB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DAC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DAD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DBA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DBB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DBC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DBD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DCA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DCB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DCC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DCD.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DDA.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DDB.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DDC.wav",
    "https://github.com/Overture-Games/curriculum/raw/refs/heads/main/public/WebGLBuilds/HarmonyVotersStudents/Assets/MixHarmony/DDD.wav"
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