using System.Collections;
using UnityEngine;
/// <summary>
///     Handles background music playback with optional fade in/out.
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeDuration = 1f;
    private AudioSource bgmSource;

    private void Awake()
    {
        GameObject go = new GameObject("BGM_Source");
        go.transform.parent = transform;
        bgmSource = go.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
    }

    public void Play(AudioClip clip)
    {
        if (clip == null) return;
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void Stop()
    {
        bgmSource.Stop();
    }

    public void SetVolume(float vol)
    {
        bgmSource.volume = Mathf.Clamp01(vol);
    }

    public void SetMute(bool mute)
    {
        bgmSource.mute = mute;
    }

    public void FadeIn(AudioClip clip)
    {
        if (clip == null) return;
        StartCoroutine(FadeRoutine(0f, 1f, clip));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeRoutine(bgmSource.volume, 0f, null));
    }

    private IEnumerator FadeRoutine(float from, float to, AudioClip clip)
    {
        if (clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.volume = 0f;
            bgmSource.Play();
        }
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }
        bgmSource.volume = to;
        if (to == 0f)
            bgmSource.Stop();
    }
}