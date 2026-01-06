using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [Header("Audio Sources")]   
    [SerializeField] AudioSource backgroundMusicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip Background;
    public AudioClip JumpSFX;
    public AudioClip RunSFX;
    public AudioClip WalkSFX;
    public AudioClip CrouchSFX;
    public AudioClip Flashlight;
    public AudioClip CollectItemSFX;
    public AudioClip CampfireSFX;
    public AudioClip BirdSFX;

    private void Start()
    {
        backgroundMusicSource.clip = Background;
        backgroundMusicSource.Play();
    }
}
