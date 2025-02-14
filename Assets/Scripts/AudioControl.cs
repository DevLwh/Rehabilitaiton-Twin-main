using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    GameObject BackgroundMusic;
    AudioSource backgroundMusic;

    void Awake()
    {
        // 오디오 시작 스크립트
        BackgroundMusic = GameObject.Find("AudioManager");
        backgroundMusic = BackgroundMusic.GetComponent<AudioSource>();
        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.volume = 1f;
            return;
        }
        else 
        {
            backgroundMusic.Play();
            DontDestroyOnLoad(BackgroundMusic);
        }
    }

}
