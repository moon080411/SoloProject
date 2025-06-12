using System;
using _01.Script.Players;
using Plugins.ScriptFinder.RunTime.Finder;
using UnityEngine;

namespace _01.Script.Manager
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private ScriptFinderSO playerFinder;
        [SerializeField] private float clampRange = 100f;
        [SerializeField] private float fogMentalDown;

        private Player _player;

        private void Awake()
        {
            _player = playerFinder.GetTarget<Player>();
        }

        private void Update()
        {
            if(playerFinder.GetTargetTransform().position.magnitude > clampRange)
            {
                //안개 설정
                //_player.ScMental;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, clampRange);
        }
    }
}
