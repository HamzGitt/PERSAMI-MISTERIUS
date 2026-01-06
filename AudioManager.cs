using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource backgroundMusicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip Background;

    private void Start()
    {
        backgroundMusicSource.clip = Background;
        backgroundMusicSource.Play();
    }
}
