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
        [SerializeField] private Slider masterSlider;

        private void Start()
        {
            LoadVolume();
            gameObject.SetActive(false);
        }
        
        public void SetMasterVolume()
        {
            float volume = masterSlider.value;
            myMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("Master", volume);
        }
        public void SetMusicVolume()
        {
            float volume = musicSlider.value;
            myMixer.SetFloat("BGM", Mathf.Log10(volume)*20);
            PlayerPrefs.SetFloat("BGM",volume);
        }
        public void SetSfxVolume()
        {
            float volume = sfxSlider.value;
            myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("SFX", volume);
        }
        
        private void LoadVolume()
        {
            musicSlider.value = PlayerPrefs.GetFloat("BGM" , 0f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFX" , 0f);
            masterSlider.value = PlayerPrefs.GetFloat("Master" , 0f);

            SetMusicVolume();
            SetSfxVolume();
            SetMasterVolume();
        }
    }
}
