using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// Handles the technical process of mixing audio clips and exporting them to a .wav file.
/// </summary>
public class AudioExporterMono : MonoBehaviour
{
    [Header("Export Settings")]
    [Tooltip("The sample rate for the output WAV file (e.g., 44100, 48000).")]
    public int targetSampleRate = 44100;

    [Tooltip("The number of channels for the output WAV file (1 for mono, 2 for stereo).")]
    public int targetChannels = 2;

    [Tooltip("The bit depth for the output WAV file (e.g., 16).")]
    public int bitsPerSample = 16;

    /// <summary>
    /// Starts the audio export process.
    /// </summary>
    /// <param name="audioExportData">The data container with all the audio clips to mix.</param>
    /// <param name="outputFileName">The name of the file to be created (e.g., "MySong.wav").</param>
    /// <param name="onCompleted">Callback invoked when the export is finished. Parameters are (bool success, string pathOrError).</param>
    public void ExportAudioToFile(AudioExport audioExportData, string outputFileName, System.Action<bool, string> onCompleted)
    {
        if (audioExportData == null)
        {
            Debug.LogError("AudioFileExporter: AudioExport data is null. Cannot export.");
            onCompleted?.Invoke(false, "AudioExport data was null.");
            return;
        }

        StartCoroutine(ExportCoroutine(audioExportData, outputFileName, onCompleted));
    }

    private IEnumerator ExportCoroutine(AudioExport audioExportData, string outputFileName, System.Action<bool, string> onCompleted)
    {
        Debug.Log("AudioFileExporter: Starting audio export...");

        // 1) Mix down all recorded clips into a float[] buffer
        var masterBuffer = audioExportData.GetMixedAudioData(targetSampleRate, targetChannels);
        Debug.Log($"AudioFileExporter: Mixed buffer length = {masterBuffer?.Length}");

        if (masterBuffer == null || masterBuffer.Length == 0)
        {
            Debug.LogError("AudioFileExporter: Master buffer is empty. Nothing to export.");
            onCompleted?.Invoke(false, "The mixed audio buffer was empty.");
            yield break;
        }

        // 2) Build the file path
        var filePath = Path.Combine(Application.persistentDataPath, outputFileName);
        Debug.Log($"AudioFileExporter: Preparing to save WAV to: {filePath}");

        // 3) Write header + samples in a coroutine
        bool exportSuccess = false;
        string resultMessage = "";

        yield return StartCoroutine(WriteWavFile(filePath, masterBuffer, (success, error) =>
        {
            exportSuccess = success;
            resultMessage = success ? filePath : error;
        }));

        // 4) Invoke the final completion callback
        onCompleted?.Invoke(exportSuccess, resultMessage);
    }

    private IEnumerator WriteWavFile(string filePath, float[] masterBuffer, System.Action<bool, string> callback)
    {
        // === 1) HEADER WRITING (no yields) ===
        FileStream fileStream = null;
        BinaryWriter writer = null;
        string headerError = null;

        try
        {
            fileStream = new FileStream(filePath, FileMode.Create);
            writer = new BinaryWriter(fileStream);

            int byteRate = targetSampleRate * targetChannels * (bitsPerSample / 8);
            short blockAlign = (short)(targetChannels * (bitsPerSample / 8));
            int dataSize = masterBuffer.Length * (bitsPerSample / 8);
            int riffHeaderSize = 36 + dataSize;

            // RIFF chunk
            writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
            writer.Write(riffHeaderSize);
            writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));

            // fmt subchunk
            writer.Write(System.Text.Encoding.ASCII.GetBytes("fmt "));
            writer.Write(16); // Subchunk1Size = 16 for PCM
            writer.Write((short)1); // AudioFormat = PCM (1)
            writer.Write((short)targetChannels);
            writer.Write(targetSampleRate);
            writer.Write(byteRate);
            writer.Write(blockAlign);
            writer.Write((short)bitsPerSample);

            // data subchunk
            writer.Write(System.Text.Encoding.ASCII.GetBytes("data"));
            writer.Write(dataSize);
        }
        catch (System.Exception ex)
        {
            headerError = ex.Message;
        }

        if (headerError != null)
        {
            writer?.Dispose();
            fileStream?.Dispose();
            callback?.Invoke(false, $"Failed to write WAV header: {headerError}");
            yield break;
        }

        // === 2) SAMPLE WRITING (with yields) ===
        for (int i = 0; i < masterBuffer.Length; i++)
        {
            try
            {
                short sample16 = (short)(masterBuffer[i] * 32767f);
                writer.Write(sample16);
            }
            catch (System.Exception ex)
            {
                writer?.Dispose();
                fileStream?.Dispose();
                callback?.Invoke(false, $"Error writing sample #{i}: {ex.Message}");
                yield break;
            }

            // Yield periodically to prevent the application from freezing
            if (i > 0 && i % (targetSampleRate * targetChannels) == 0)
            {
                yield return null;
            }
        }

        // === 3) CLEAN UP ON SUCCESS ===
        writer.Dispose();
        fileStream.Dispose();
        Debug.Log("AudioFileExporter: WAV file written successfully.");
        callback?.Invoke(true, string.Empty);
    }
}