using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Diagnostics;
using TMPro;
using Overture.Achievements;

public class MusicMixer : MonoBehaviour
{
    public AudioSource bassSource, chordsSource, melodySource, drumsSource;
    public AudioClip[] bassVariationsInstruments, chordsVariationsInstruments, melodyVariationsInstruments; // Modo Instruments
    public AudioClip[] bassVariationsHarmony, chordsVariationsHarmony, melodyVariationsHarmony; // Modo Harmony
    public AudioClip defaultDrumsInstruments, defaultDrumsHarmony;

    public Button[] bassButtons, chordsButtons, melodyButtons;
    public Button playButton, doneButton;
    [SerializeField] private TMP_Text playButtonText;

    [SerializeField] private AudioExporterMono exporterMono;
    [SerializeField] private AudioPlatformHandler platformHandler;

    private Dictionary<string, AudioSource> audioSources;
    private Dictionary<string, AudioClip[]> audioClips;
    private Dictionary<string, Button[]> buttonGroups;
    private Dictionary<string, int> selectedIndices;

    private bool isPlaying = false;
    private string selectedCombination = "";

    private Dictionary<string, Color[]> _originalButtonColors = new Dictionary<string, Color[]>();

    void Start()
    {
        string gameMode = SessionManager.Instance ? SessionManager.Instance.GetGameMode() : "Instruments";

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

        StoreOriginalButtonColors();
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

    void StoreOriginalButtonColors()
    {
        _originalButtonColors = new Dictionary<string, Color[]>();
        foreach (var instrument in buttonGroups.Keys)
        {
            Color[] colors = new Color[buttonGroups[instrument].Length];
            for (int i = 0; i < buttonGroups[instrument].Length; i++)
            {
                colors[i] = buttonGroups[instrument][i].GetComponent<Image>().color;
            }
            _originalButtonColors[instrument] = colors;
        }
    }

    void HighlightSelectedButton(string instrument)
    {
        if (buttonGroups.ContainsKey(instrument))
        {
            for (int i = 0; i < buttonGroups[instrument].Length; i++)
            {
                var image = buttonGroups[instrument][i].GetComponent<Image>();
                image.color = (i == selectedIndices[instrument]) ? Color.white : _originalButtonColors[instrument][i];
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

        playButtonText.text = isPlaying ? "Pause" : "Play";
    }

    private void OnDoneClicked()
    {
        CheckForAchievements();

        var export = new AudioExport();
        foreach (var item in audioSources.Values)
        {
            if (item.clip)
                export.AddClip(item.clip, 0);
        }
        exporterMono.ExportAudioToFile(export, GenerateFileName(), OnComplete);

        void OnComplete(bool didSucceed, string filePath)
        {
            if (!didSucceed) return;
            platformHandler.HandleExportedFile(filePath, "harmonidome", "daw-composition", "user-created");
        }
    }

    private string GenerateFileName()
    {
        var timestamp = System.DateTime.Now.ToString("MMdd_HHmm");
        return $"Harmonidome_{selectedCombination}_{timestamp}.wav";
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

    private void CheckForAchievements()
    {
        const string gameId = "harmonidome";
        Achievement.Earn(gameId, gameId + "__export");

        if (selectedCombination == "AAA")
            Achievement.Earn(gameId, gameId + "__all-a");
        if (selectedCombination == "BBB")
            Achievement.Earn(gameId, gameId + "__all-b");
        if (selectedCombination == "CCC")
            Achievement.Earn(gameId, gameId + "__all-c");
        if (selectedCombination == "DDD")
            Achievement.Earn(gameId, gameId + "__all-d");
    }
}