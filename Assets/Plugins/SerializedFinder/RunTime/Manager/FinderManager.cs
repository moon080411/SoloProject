using System;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Codice.CM.Common;
using Plugins.SerializedFinder.RunTime.Dependencies;
using Plugins.SerializedFinder.RunTime.Finder;
using Plugins.SerializedFinder.RunTime.Serializable;
using UnityEngine;

namespace Plugins.SerializedFinder.RunTime.Manager
{
    [DefaultExecutionOrder(-1)]
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
    }
}