using Plugins.ScriptFinder.RunTime.Serializable;
using UnityEngine;

namespace Plugins.ScriptFinder.RunTime.Finder
{
    [CreateAssetMenu(fileName = "ScriptFinder",menuName = "SO/ScriptFinder", order = 0)]
    public class ScriptFinderSO : ScriptableObject
    {
        [Header("MonoBehaviour type to find in the scene")]
        [SerializeField]
        private SerializableType _keyType;
        private Transform _targetTransform;

        private MonoBehaviour _target;
        public SerializableType KeyType => _keyType;
        public void SetTarget(MonoBehaviour target)
        {
            _target = target;
            _targetTransform = target?.transform;
        }
        public T GetTarget<T>() where T : MonoBehaviour
        {
            if (_target is T t) return t;
            Debug.LogError($"ScriptFinderSO: Target is not of type {typeof(T).Name}. Current type: {_target?.GetType().Name ?? "null"}");
            return null;
        }

        public Transform GetTargetTransform()
        {
            return _targetTransform;
        }
    }
}