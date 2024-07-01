using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider DialogeSlider;


    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("SFXVolume") || PlayerPrefs.HasKey("VoiceVolume"))
        {
            LoadVolume();
        }
        else
        {
            setMusicVolume();
            setSFXVolume();
            setVoiceVolume();
        }
           
    }

    public void setMusicVolume()
    {
        float volume = MusicSlider.value;
        mixer.SetFloat("music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume",volume);
    }

    public void setSFXVolume()
    {
        float volume = SFXSlider.value;
        mixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void setVoiceVolume()
    {
        float volume = SFXSlider.value;
        mixer.SetFloat("dialoge", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("VoiceVolume", volume);
    }
    private void LoadVolume()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        DialogeSlider.value = PlayerPrefs.GetFloat("VoiceVolume");

        setSFXVolume();
        setMusicVolume();
        setVoiceVolume();
    }
}
