using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace _01.Script.Manager
{
    [DefaultExecutionOrder(-1)]
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioMixerGroup bgmGroup;
        [SerializeField] private AudioMixerGroup sfxGroup;

        [SerializeField , SerializedDictionary("SoundName" , "AudioClip")]
        private SerializedDictionary<string, AudioClip> clipMap = new SerializedDictionary<string, AudioClip>();

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("[SoundManager] Another instance already exists. Destroying this one.");
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void PlayBGM(string sound)
        {
            if (string.IsNullOrEmpty(sound))
            {
                return;
            }
            if (!clipMap.TryGetValue(sound, out var clip))
            {
                Debug.LogWarning($"[SoundManager] No BGM clip mapped for {sound}");
                return;
            }

            Debug.Log($"[SoundManager] Playing BGM: {sound}");

            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.outputAudioMixerGroup = bgmGroup;
            bgmSource.Play();
        }

        public void Play(string sound)
        {
            if (!clipMap.TryGetValue(sound, out var clip))
            {
                Debug.LogWarning($"[SoundManager] No clip mapped for {sound}");
                return;
            }
            sfxSource.outputAudioMixerGroup = sfxGroup;
            sfxSource.PlayOneShot(clip);
        }

        public AudioSource PlayWithLoop(string sound , Transform target = null)
        {
            if (!clipMap.TryGetValue(sound, out var clip))
            {
                Debug.LogWarning($"[SoundManager] No clip mapped for {sound}");
                return null;
            }

            var newSfxSource = target != null ? target.AddComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();
            if(target != null)
                newSfxSource.spatialBlend = 1;

            newSfxSource.clip = clip;
            newSfxSource.loop = true;
            newSfxSource.outputAudioMixerGroup = sfxGroup;
            newSfxSource.Play();
            
            return newSfxSource;
        }
        
        public void StopPlay(AudioSource source)
        {
            if (source != null)
            {
                source.Stop();
                Destroy(source);
            }
            else
            {
                Debug.LogWarning("[SoundManager] Attempted to stop a null AudioSource.");
            }
        }

        public void SetBGMVolume(float value)
        {
            float dB = Mathf.Approximately(value, 0f) ? -80f : Mathf.Log10(value) * 20f;
            audioMixer.SetFloat("BGMVolume", dB);
        }

        public void SetSFXVolume(float value)
        {
            float dB = Mathf.Approximately(value, 0f) ? -80f : Mathf.Log10(value) * 20f;
            audioMixer.SetFloat("SFXVolume", dB);
        }
    }
}
