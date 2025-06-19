using _01.Script.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _01.Script
{
    public class Setting : MonoBehaviour
    {
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private string PlayerBGMName;
        
        private void Start()
        {
            SoundManager.Instance.PlayBGM(PlayerBGMName);
        }
        public void SettingSet(bool isActive)
        {
            settingPanel.SetActive(isActive);
        }
        public void TutorialSet(bool isActive)
        {
            tutorialPanel.SetActive(isActive);
        }
        public void GoTitle()
        {
            SceneManager.LoadScene("Title");
        }
        public void GoGame()
        {
            SceneManager.LoadScene("Game");
        }
        public void GetOut()
        {
            Application.Quit();
        }

        public void ReStart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
