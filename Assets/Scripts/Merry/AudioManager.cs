﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager am;
    public Sound[] sounds;
    // Start is called before the first frame update
    private void Awake()
    {
        am = this;
        foreach(Sound s in sounds)
        {
            s.source =gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
       Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
        Debug.Log("played");
    }
}
