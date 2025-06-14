using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _01.Script
{
    public class VolumeSetting : MonoBehaviour
    {
        
        [SerializeField] private AudioMixer myMixer;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        private void Start()
        {
            if (PlayerPrefs.HasKey("BGMVolume"))
            {
                LoadVolume();
            }
            else
            {
                SetMusicVolume();
                SetSfxVolume();
            }
        
        }
        public void SetMusicVolume()
        {
            float volume = musicSlider.value;
            myMixer.SetFloat("BGMVolume", Mathf.Log10(volume)*20);
            PlayerPrefs.SetFloat("BGMVolume",volume);
        }
        public void SetSfxVolume()
        {
            float volume = sfxSlider.value;
            myMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }
        
        private void LoadVolume()
        {
            musicSlider.value = PlayerPrefs.GetFloat("BGMVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

            SetMusicVolume();
            SetSfxVolume();
        }
    }
}
