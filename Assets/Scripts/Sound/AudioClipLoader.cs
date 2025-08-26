using System.Collections.Generic;
using UnityEngine;
/// <summary>
///     Loads all AudioClips from Resources/Audio folder and caches them.
/// </summary>
public class AudioClipLoader : MonoBehaviour
{
    [Header("Audio Folder Path")]
    public string audioFolder = "Sound";

    private Dictionary<string, AudioClip> clips;

    private void Awake()
    {
        clips = new Dictionary<string, AudioClip>();
        AudioClip[] loaded = Resources.LoadAll<AudioClip>(audioFolder);
        foreach (AudioClip clip in loaded)
        {
            if (!clips.ContainsKey(clip.name))
                clips.Add(clip.name, clip);
        }
    }

    public AudioClip GetClip(string name)
    {
        if (clips.TryGetValue(name, out AudioClip clip)) return clip;
        Debug.LogWarning($"AudioClip '{name}' not found in Resources/{audioFolder}");
        return null;
    }
}