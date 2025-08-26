using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///     Manages a pool of AudioSources for one-shot SFX playback.
/// </summary>
public class SFXPoolPlayer : MonoBehaviour
{
    [Header("Pool Settings")]
    public int poolSize = 10;
    [SerializeField]
    private GameObject audioSourcePrefab;

    private Queue<AudioSource> pool;
    private Transform poolRoot;

    private void Awake()
    {
        if (audioSourcePrefab == null)
        {
            audioSourcePrefab = Resources.Load<GameObject>("Prefabs/Audio/AudioSourcePrefab");
            if (audioSourcePrefab == null)
            {
                Debug.LogError("AudioSourcePrefab δ�ҵ����뽫����� Resources/Prefabs/Audio �²�����Ϊ AudioSourcePrefab");
                return;
            }
        }
        poolRoot = new GameObject("SFXPool").transform;
        poolRoot.parent = transform;
        pool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
            CreateNewSource();
    }

    private AudioSource CreateNewSource()
    {
        GameObject go = Instantiate(audioSourcePrefab, poolRoot);
        AudioSource src = go.GetComponent<AudioSource>();
        src.playOnAwake = false;
        go.SetActive(false);
        pool.Enqueue(src);
        return src;
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        AudioSource src = GetSource();
        src.clip = clip;
        src.volume = volume;
        src.gameObject.SetActive(true);
        src.Play();
        StartCoroutine(ReleaseAfterPlay(src));
    }

    private AudioSource GetSource()
    {
        if (pool.Count > 0)
            return pool.Dequeue();
        return CreateNewSource();
    }

    private IEnumerator ReleaseAfterPlay(AudioSource src)
    {
        yield return new WaitForSeconds(src.clip.length);
        src.Stop();
        src.clip = null;
        src.gameObject.SetActive(false);
        pool.Enqueue(src);
    }

    public void SetVolume(float vol)
    {


        foreach (AudioSource src in pool)
        {
            src.volume = vol;
        }
    }

    public void SetMute(bool mute)
    {
        foreach (AudioSource src in pool)
        {
            src.mute = mute;
        }
    }
}