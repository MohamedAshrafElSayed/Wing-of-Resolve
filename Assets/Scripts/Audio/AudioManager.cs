using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    SFX,
    Music,
    Dialog
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<Clip> musicAudio;
    public List<Clip> sfxAudio;
    public List<Clip> dialog;

    [SerializeField] AudioSource Music;
    [SerializeField] AudioSource SFX;
    [SerializeField] AudioSource Dialog;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }  
    }

    public void SFXLoop(AudioType type, string name)
    {
       
                SFX.clip = GetClip(type, name);
                SFX.loop = true;
                SFX.Play();          
    }

    public void NoSFXLoop()
    {
        SFX.clip = null;
        SFX.loop = false;
        SFX.Stop();

    }


    public void LoopStop(AudioType type, string name)
    {
        switch (type)
        {
            case AudioType.SFX:
                SFX.Stop();
                //SFX.PlayOneShot(GetClip(type, name));
                //SFX.loop = false;
                break;

            case AudioType.Music:
                Music.Stop();
                // Music.PlayOneShot(GetClip(type, name));
                //Music.loop = false;
                break;
        }
    }
    public void Play(AudioType type, string name)
    {
        switch (type)
        {
            case AudioType.SFX:
                SFX.PlayOneShot(GetClip(type, name));              
                break;

            case AudioType.Music:
                Music.Stop();
                Music.clip = GetClip(type, name);
                Music.Play();
                break;

            case AudioType.Dialog:
                Dialog.PlayOneShot(GetClip(type, name));
                break;
        }
        
    }

    public void MuteType(AudioType type)
    {
        switch (type)
        {
            case AudioType.SFX:
                SFX.mute=!SFX.mute;
                break;

            case AudioType.Music:
                Music.mute = !Music.mute;
                break;

            case AudioType.Dialog:
                Dialog.mute = !Dialog.mute;
                break;
        }
    }
    public void MuteAll(AudioType type)
    {
        MuteType(AudioType.Music);
        MuteType(AudioType.SFX);
        MuteType(AudioType.Dialog);
    }

    public void PauseAll()
    {
        SFX.Pause();
        Music.Pause();
        Dialog.Pause();
    }
    public void ResumeAll()
    {
        SFX.Play();
        Music.Play();
        Dialog.Play();
    }

    public AudioClip GetClip(AudioType type, string name)
    {
        List<Clip> audioList = null;
        switch (type)
        {
            case AudioType.SFX:
                audioList = sfxAudio;
                break;

            case AudioType.Music:
                audioList = musicAudio;
                break;

            case AudioType.Dialog:
                audioList = dialog;
                break;
        }
        if (audioList != null)
        {
            return audioList.Find(clip => clip._clipName == name)._clip;
        }
        return null;
    }

    public void Play(AudioType type, AudioClip clip)
    {
        switch (type)
        {
            case AudioType.SFX:
                SFX.PlayOneShot(clip);
                break;

            case AudioType.Music:
                Music.PlayOneShot(clip);
                break;
        }

    }


}