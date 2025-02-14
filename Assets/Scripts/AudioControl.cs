using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    GameObject BackgroundMusic;
    AudioSource backgroundMusic;

    void Awake()
    {
        // ����� ���� ��ũ��Ʈ
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
