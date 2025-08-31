using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayMusic("bgm");
        AudioManager.Instance.SetMusicVolume(0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
