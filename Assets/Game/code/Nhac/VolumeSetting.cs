using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        // Load volume đã lưu
        float musicVol = PlayerPrefs.GetFloat("musicvolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("sfxvolume", 1f);

        MusicSlider.value = musicVol;
        SFXSlider.value = sfxVol;

        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetMusicVolume()
    {
        float volume = Mathf.Clamp(MusicSlider.value, 0.0001f, 1f);
        Mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicvolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = Mathf.Clamp(SFXSlider.value, 0.0001f, 1f);
        Mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxvolume", volume);
    }
}

