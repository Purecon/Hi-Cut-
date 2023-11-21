using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Source")]
    public AudioSource audiosrc;
    [Header("Audio Clips")]
    public List<NamedClips> soundList = new List<NamedClips>();
    public Dictionary<string, AudioClip> soundDict = new Dictionary<string, AudioClip>();

    /*
    public delegate void SoundEvent(string name);
    public event SoundEvent PlaySoundEvent;
    */

    [Serializable]
    public struct NamedClips
    {
        public string name;
        public AudioClip sound;
    }

    private void Start()
    {
        soundDict = new Dictionary<string, AudioClip>();

        foreach (NamedClips clip in soundList)
        {
            soundDict.Add(clip.name, clip.sound);
        }
        DontDestroyOnLoad(gameObject);
        //PlaySound("Boom");
    }

    public void PlaySound(string name, float volume)
    {
        if (soundDict.ContainsKey(name))
        {
            Debug.Log("Playsound: " + name);
            audiosrc.PlayOneShot(soundDict[name], volume);
        }
        else
        {
            Debug.LogWarning("No audio clip with that name");
        }
    }
}
