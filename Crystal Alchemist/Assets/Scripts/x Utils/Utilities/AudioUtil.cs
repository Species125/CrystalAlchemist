﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUtil : MonoBehaviour
{
    private static Dictionary<AudioClip, float> soundsAlreadyPlayed = new Dictionary<AudioClip, float>();
    private static AudioSource blub;

    public static void playSoundEffect(AudioClip soundeffect)
    {
        playSoundEffect(null, soundeffect);
    }

    public static void playSoundEffect(AudioClip soundeffect, float volume)
    {
        playSoundEffect(null, soundeffect, volume);
    }

    public static void playSoundEffect(AudioClip music, AudioSource source, float volume)
    {
        source.volume = volume;
        source.clip = music;
        source.playOnAwake = false;
        source.loop = false;
        source.Play();
    }

    public static void playSoundEffect(GameObject gameObject, AudioClip soundeffect)
    {
        playSoundEffect(gameObject, soundeffect, MasterManager.settings.soundEffectVolume);
    }

    public static void playSoundEffect(GameObject gameObject, AudioClip soundeffect, float volume)
    {
        if (soundeffect != null)
        {
            GameObject parent = GameObject.FindWithTag("Audio");

            if (canPlaySound(soundeffect))
            {
                GameObject temp = new GameObject(soundeffect.name);
                if (parent != null) temp.transform.SetParent(parent.transform);

                AudioSource source = temp.AddComponent<AudioSource>();
                source.pitch = MasterManager.settings.soundEffectPitch;
                source.volume = volume;
                source.clip = soundeffect;

                if (gameObject != null)
                {
                    temp.transform.position = gameObject.transform.position;
                    source.maxDistance = 100f;
                    source.spatialBlend = 1f;
                    source.rolloffMode = AudioRolloffMode.Linear;
                    source.dopplerLevel = 0f;
                }
                source.Play();
                Destroy(temp, soundeffect.length);
            }
        }
    }

    private static bool canPlaySound(AudioClip clip)
    {
        if (soundsAlreadyPlayed.ContainsKey(clip))
        {
            float lastTimePlayed = soundsAlreadyPlayed[clip];
            float maximum = .05f;
            if (lastTimePlayed + maximum < Time.time)
            {
                soundsAlreadyPlayed[clip] = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            soundsAlreadyPlayed.Add(clip, Time.time);
            return true;

        }
    }
}
