using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header ("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip BackGround;
    public AudioClip Coin;
    public AudioClip Attack;
    public AudioClip Jump;
    public AudioClip Chose;
    public AudioClip Ruong;
    public AudioClip VicTory;
    public AudioClip Lost;
    public AudioClip Lazer;

    private void Start()
    {
        musicSource.clip = BackGround;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip Clip)
    {
        SFXSource.PlayOneShot(Clip);
    }
}
