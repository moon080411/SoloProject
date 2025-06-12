using System;
using Plugins.ScriptFinder.RunTime.Finder;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Script.Generator
{
    public class ResourceGenerator : MonoBehaviour
    {
        [SerializeField] private ScriptFinderSO resourceManagerFinder;
        [SerializeField] private Transform woodSpawnPoints;
        [SerializeField] private int firstWoodSpawn = 10;
        [SerializeField] private float spawnTime = 30f;

        private void Awake()
        {
            
        }
    }
}