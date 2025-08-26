using UnityEngine;
/// <summary>
///     Core interface for audio playback. Routes requests to MusicPlayer and SFXPoolPlayer.
/// </summary>
public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    private AudioClipLoader clipLoader;
    private MusicPlayer musicPlayer;
    private SFXPoolPlayer sfxPlayer;
    public static AudioManager Instance {
        get {
            if (instance == null)
            {
                GameObject go = new GameObject("AudioManager");
                instance = go.AddComponent<AudioManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        clipLoader = gameObject.AddComponent<AudioClipLoader>();
        musicPlayer = gameObject.AddComponent<MusicPlayer>();
        sfxPlayer = gameObject.AddComponent<SFXPoolPlayer>();
    }

    /// <summary>
    ///     Play background music by clip name.
    /// </summary>
    public void PlayMusic(string clipName)
    {
        AudioClip clip = clipLoader.GetClip(clipName);
        musicPlayer.Play(clip);
    }

    /// <summary>
    ///     Play one-shot sound effect.
    /// </summary>
    public void PlaySFX(string clipName, float volume = 1f)
    {
        AudioClip clip = clipLoader.GetClip(clipName);
        sfxPlayer.PlaySFX(clip, volume);
    }

    public void SetMusicVolume(float vol)
    {
        musicPlayer.SetVolume(vol);
    }
    public void SetSFXVolume(float vol)
    {
        sfxPlayer.SetVolume(vol);
    }
    public void StopMusic()
    {
        musicPlayer.Stop();
    }
    public void SetMute(bool mute)
    {
        musicPlayer.SetMute(mute);
        sfxPlayer.SetMute(mute);
    }
}