using System;
using System.Collections.Generic;
using Plugins.SerializedFinder.RunTime.Finder;
using UnityEngine;

namespace Plugins.SerializedFinder.RunTime.Manager
{
    [DefaultExecutionOrder(-10)]
    public class FinderManager : MonoBehaviour
    {
        [SerializeField] private ScriptFinderSO[] finders;

        private void Awake()
        {
            foreach (ScriptFinderSO finder in finders)
            {
                Type key = finder.KeyType.Type;
                MonoBehaviour value = (MonoBehaviour)FindFirstObjectByType(key);
                if (value == null)
                {
                    Debug.LogWarning($"[FinderManager] Could not find an object in the scene corresponding to the type {finder.KeyType.Type.Name}.");
                    continue;
                }
                finder.SetTarget(value);
            }
        }
        private IEnumerable<MonoBehaviour> GetMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        }
    }
}