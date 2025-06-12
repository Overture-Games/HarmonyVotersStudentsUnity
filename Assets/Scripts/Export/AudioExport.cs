using UnityEngine;
using System.Collections.Generic;

public class AudioExport
{
    private struct AudioEvent
    {
        public AudioClip clip;
        public float startTime;
        public float volume;
        public float duration;
        public float release;
    }

    private List<AudioEvent> _audioEvents = new List<AudioEvent>();

    public void AddClip(AudioClip clip, float timeSeconds, float volume = 1f, float duration = -1f, float release = 0f)
    {
        if (clip == null)
        {
            return;
        }

        float effectiveDuration = (duration < 0) ? clip.length : duration;

        volume = Mathf.Clamp01(volume);
        effectiveDuration = Mathf.Max(0f, effectiveDuration);
        release = Mathf.Max(0f, release);

        _audioEvents.Add(new AudioEvent
        {
            clip = clip,
            startTime = timeSeconds,
            volume = volume,
            duration = effectiveDuration,
            release = release
        });
    }

    public float[] GetMixedAudioData(int targetSampleRate, int targetChannels)
    {
        if (_audioEvents.Count == 0)
        {
            return new float[0];
        }

        var maxEndTime = 0f;
        foreach (var audioEvent in _audioEvents)
        {
            if (audioEvent.clip.frequency != targetSampleRate || audioEvent.clip.channels != targetChannels)
            {
            }

            var naturalClipEndTime = audioEvent.startTime + audioEvent.clip.length;
            
            var cutOrFadeEndTime = audioEvent.startTime + audioEvent.duration + audioEvent.release;

            maxEndTime = Mathf.Max(maxEndTime, naturalClipEndTime, cutOrFadeEndTime);
        }

        if (maxEndTime <= 0f)
        {
            return new float[0];
        }

        var totalSamples = Mathf.CeilToInt(maxEndTime * targetSampleRate) * targetChannels;
        var masterBuffer = new float[totalSamples];

        foreach (var audioEvent in _audioEvents)
        {
            var clipData = new float[audioEvent.clip.samples * audioEvent.clip.channels];
            audioEvent.clip.GetData(clipData, 0);

            var startSampleIndex = (int)(audioEvent.startTime * targetSampleRate) * targetChannels;

            float fadeStartTimeAbsolute = audioEvent.startTime + audioEvent.duration;
            float fadeEndTimeAbsolute = audioEvent.startTime + audioEvent.duration + audioEvent.release;

            for (var i = 0; i < clipData.Length; i++)
            {
                var masterBufferIndex = startSampleIndex + i;

                if (masterBufferIndex >= masterBuffer.Length)
                {
                    break;
                }

                float currentTimeInMix = (masterBufferIndex / (float)targetChannels) / targetSampleRate;

                float currentSampleVolume = audioEvent.volume;

                if (currentTimeInMix >= fadeStartTimeAbsolute)
                {
                    if (audioEvent.release > 0)
                    {
                        float timeIntoRelease = currentTimeInMix - fadeStartTimeAbsolute;
                        float fadeFactor = 1f - (timeIntoRelease / audioEvent.release);
                        currentSampleVolume *= Mathf.Clamp01(fadeFactor);
                    }
                    else
                    {
                        currentSampleVolume = 0f;
                    }
                }

                if (currentSampleVolume <= 0f && currentTimeInMix >= fadeStartTimeAbsolute)
                {
                    break;
                }

                masterBuffer[masterBufferIndex] += clipData[i] * currentSampleVolume;
            }
        }

        for (var i = 0; i < masterBuffer.Length; i++)
        {
            masterBuffer[i] = Mathf.Clamp(masterBuffer[i], -1f, 1f);
        }

        return masterBuffer;
    }
}