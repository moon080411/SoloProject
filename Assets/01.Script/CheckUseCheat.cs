using System;
using _01.Script.Players;
using Plugins.ScriptFinder.RunTime.Finder;
using TMPro;
using UnityEngine;

namespace _01.Script
{
    public class CheckUseCheat : MonoBehaviour
    {
        [SerializeField ]private ScriptFinderSO _playerFinder;
        [SerializeField ]private TextMeshProUGUI _cheatText;

        private void OnEnable()
        {
            if (_playerFinder.GetTarget<Player>().IsUseCheat)
            {
                _cheatText.gameObject.SetActive(true);
            }
            else
            {
                _cheatText.gameObject.SetActive(false);
            }
        }
    }
}
