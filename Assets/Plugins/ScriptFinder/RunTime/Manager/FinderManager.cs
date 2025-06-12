using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.ScriptFinder.RunTime.Finder;
using UnityEngine;

namespace Plugins.ScriptFinder.RunTime.Manager
{
    [DefaultExecutionOrder(-10)]
    public class FinderManager : MonoBehaviour
    {
        [SerializeField] private ScriptFinderSO[] finders;
        private Dictionary<Type, MonoBehaviour> _findedComponents;

        private void Awake()
        {
            if (finders == null || finders.Length == 0) return;
            
            Initialize();
            ProcessFinders();
        }

        private void Initialize()
        {
            var components = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            _findedComponents = new Dictionary<Type, MonoBehaviour>();
            
            foreach (var finder in finders)
            {
                Type keyType = finder.KeyType?.Type;
                if (keyType == null) continue;

                var component = components.FirstOrDefault(c => c.GetType() == keyType);
                if (component != null)
                {
                    _findedComponents[keyType] = component;
                }
            }
        }

        private void ProcessFinders()
        {
            foreach (var finder in finders)
            {
                Type keyType = finder.KeyType?.Type;
                if (keyType == null) continue;

                if (_findedComponents.TryGetValue(keyType, out var component))
                {
                    finder.SetTarget(component);
                }
                else
                {
                    Debug.LogWarning($"[FinderManager] Could not find an object in the scene corresponding to the type {keyType.Name}.");
                }
            }
        }
    }
}