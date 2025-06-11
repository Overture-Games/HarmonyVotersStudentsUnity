using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct SceneTransitionOptions
{
    public float? FadeInDuration;
    public float? FadeOutDuration;
}

public class SceneTransition : Singleton<SceneTransition>
{
    private static bool _didInitialTransition;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void SceneTransitionStaticInit() => _didInitialTransition = false;

    [SerializeField] private RawImage image;
    [SerializeField] private float defaultDuration = 1f;
    [SerializeField] private bool shouldDoInitialTransition = true;
    // [SerializeField] private SoundSO whoosh;

    public bool CanBeInterrupted { get; private set; } = true;
    private float _progress;
    public float Progress
    {
        get => _progress;
        private set
        {
            _progress = value;
            image.material.SetFloat("_Progress", _progress);
        }
    }

    private List<string> _transitionHistory = new();
    public IReadOnlyList<string> TransitionHistory => _transitionHistory;

    private void Start()
    {
        if (SceneManager.sceneCount > 0)
            _transitionHistory.Add(SceneManager.GetSceneAt(0).name);
    }

    public static void LoadSceneAsync(int buildIndex, SceneTransitionOptions? options = null) => Instance.TransitionToScene(buildIndex, options);
    public static void LoadSceneAsync(string sceneName, SceneTransitionOptions? options = null) => Instance.TransitionToScene(sceneName, options);
    public void TransitionToScene(int buildIndex, SceneTransitionOptions? options = null)
    {
        if (!CanBeInterrupted) return;
        StopAllCoroutines();
        StartCoroutine(TransitionToSceneCoroutine(buildIndex, options));
    }

    public void TransitionToScene(string sceneName, SceneTransitionOptions? options = null)
    {
        if (!CanBeInterrupted) return;
        StopAllCoroutines();
        StartCoroutine(TransitionToSceneCoroutine(sceneName, options));
    }

    public IEnumerator TransitionToSceneCoroutine(int buildIndex, SceneTransitionOptions? options = null)
        => TransitionToSceneCoroutine(SceneManager.GetSceneByBuildIndex(buildIndex).name, options);

    public IEnumerator TransitionToSceneCoroutine(string sceneName, SceneTransitionOptions? options = null)
    {
        _transitionHistory.Add(sceneName);
        CanBeInterrupted = false;
        yield return Fade(0f, options?.FadeInDuration ?? defaultDuration);
        yield return SceneManager.LoadSceneAsync(sceneName);
        CanBeInterrupted = true;
        yield return Fade(1f, options?.FadeOutDuration ?? defaultDuration);
    }

    private IEnumerator Fade(float targetProgress, float duration)
    {
        // if (SoundPlayer.IsLoaded) whoosh.PlaySelf();
        var startProgress = 1 - targetProgress;
        Progress = startProgress;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            Progress = Mathf.Lerp(startProgress, targetProgress, timer / duration);
            yield return null;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        if (!shouldDoInitialTransition)
        {
            _didInitialTransition = true;
            Progress = 1;
            CanBeInterrupted = true;
            return;
        }

        if (!_didInitialTransition)
        {
            _didInitialTransition = true;
            Progress = 0f;
            StartCoroutine(Fade(1f, defaultDuration));
        }
    }
}
