using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Handles platform-specific actions for an exported audio file.
/// For WebGL, it uploads the file to a server.
/// For Editor/Standalone, it reveals the file in the system's file explorer.
/// </summary>
public class AudioPlatformHandler : MonoBehaviour
{
    [System.Serializable]
    public class PlatformUploadResult
    {
        public bool success;
        public string message;
        public string songId;
    }
    
    /// <summary>
    /// Processes a previously exported audio file based on the current platform.
    /// </summary>
    /// <param name="filePath">The full path to the locally saved audio file.</param>
    public void HandleExportedFile(string filePath, string gameId, params string[] tags)
    {
        StartCoroutine(HandleFileCoroutine(filePath, gameId, tags));
    }

    private IEnumerator HandleFileCoroutine(string filePath, string gameId, params string[] tags)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // --- WEBGL-SPECIFIC LOGIC ---
        Debug.Log($"🌐 Uploading DAW export to platform: {filePath}");
        
        if (!File.Exists(filePath))
        {
            Debug.LogError($"❌ File not found for platform upload: {filePath}");
            yield break;
        }
        
        byte[] fileData = File.ReadAllBytes(filePath);
        string base64Audio = System.Convert.ToBase64String(fileData);
        Debug.Log($"📏 File size: {fileData.Length} bytes, Base64 length: {base64Audio.Length}");
        
        var songData = new {
            title = GenerateSongTitle(),
            gameId = "beat-decoders",
            audioData = base64Audio,
            format = "wav",
            duration = CalculateWavDuration(fileData),
            // These would ideally come from the exporter's settings, but we can hardcode for this example
            sampleRate = 44100, 
            channels = 2,
            fileSize = fileData.Length,
            isPublic = false,
            tags = tags.Concat(new[] { gameId }),
            description = "Complete DAW composition exported from Beat Decoders",
            bpm = 100
        };
        
        string songDataJson = JsonUtility.ToJson(songData);
        
        string jsCode = $@"
            (async function() {{
                try {{
                    console.log('🎵 DAW Export: Unity calling Platform saveSong API...');
                    const songData = {songDataJson};
                    console.log('📊 DAW Export Song data:', songData);
                    
                    if (typeof window.OverturePlatform === 'undefined' || !window.OverturePlatform.saveSong) {{
                        throw new Error('Platform saveSong API not available');
                    }}
                    
                    const songId = await window.OverturePlatform.saveSong(songData);
                    console.log('✅ DAW Export: Song saved successfully with ID:', songId);
                    
                    if (window.unityInstance) {{
                        window.unityInstance.SendMessage('{gameObject.name}', 'OnPlatformUploadResult', JSON.stringify({{
                            success: true,
                            message: 'DAW composition saved successfully',
                            songId: songId
                        }}));
                    }}
                }} catch (error) {{
                    console.error('❌ DAW Export: Platform saveSong failed:', error);
                    
                    if (window.unityInstance) {{
                        window.unityInstance.SendMessage('{gameObject.name}', 'OnPlatformUploadResult', JSON.stringify({{
                            success: false,
                            message: error.message || 'Unknown error',
                            songId: null
                        }}));
                    }}
                }}
            }})();
        ";
        
        Application.ExternalEval(jsCode);
        
        yield return new WaitForSeconds(1.0f); // Small delay to let the JS call initiate
        
        try
        {
            File.Delete(filePath);
            Debug.Log($"🗑️ Cleaned up local file: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"⚠️ Could not delete local file: {e.Message}");
        }

#else
        // --- EDITOR & STANDALONE LOGIC ---
        Debug.Log($"✅ DAW EXPORT SAVED (Editor/Standalone): File is at: {filePath}");

        #if UNITY_EDITOR
        EditorUtility.RevealInFinder(filePath);
        #endif

        OnPlatformUploadResult(JsonUtility.ToJson(new PlatformUploadResult
        {
            success = true,
            message = $"File saved locally to {filePath}",
            songId = "local-save-editor-id"
        }));

        yield return null;
#endif
    }
    
    // This method is called from JavaScript in WebGL builds
    public void OnPlatformUploadResult(string resultJson)
    {
        try
        {
            var result = JsonUtility.FromJson<PlatformUploadResult>(resultJson);
            if (result.success)
            {
                Debug.Log($"✅ PLATFORM RESULT: {result.message} | Song ID: {result.songId}");
            }
            else
            {
                Debug.LogError($"❌ PLATFORM RESULT: {result.message}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Error parsing platform upload result: {e.Message}");
        }
    }

    private string GenerateSongTitle()
    {
        var timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        return $"Beat Decoders DAW - {timestamp}";
    }
    
    private float CalculateWavDuration(byte[] wavData)
    {
        if (wavData.Length < 44) return 0f;
        try
        {
            int byteRate = System.BitConverter.ToInt32(wavData, 28);
            if (byteRate == 0) return 0f;
            int dataSize = wavData.Length - 44;
            return (float)dataSize / byteRate;
        }
        catch { return 0f; }
    }
}