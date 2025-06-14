using System;
using System.Collections.Generic;
using _01.Script.Entities;
using UnityEngine;

namespace _01.Script.Players
{
    public class SnowEffectGenerate : MonoBehaviour , IEntityComponent
    {
        [SerializeField] private List<GameObject> _snowEffects = new List<GameObject>();
        
        private Quaternion initialRotation;
        
        private GameObject _currentSnowEffect;
        

        private void Awake()
        {
            initialRotation = transform.localRotation;
            foreach (var snow in _snowEffects)
            {
                snow.SetActive(false);
            }
            _snowEffects[0].SetActive(true);
            _currentSnowEffect = _snowEffects[0];
        }

        private void FixedUpdate()
        {
            transform.localRotation = initialRotation;
        }

        public void SetSnowEffect(int index)
        {
            if (index < 0 || index >= _snowEffects.Count || _snowEffects[index] == null || _snowEffects[index] == _currentSnowEffect)
                return;
            
            if (_currentSnowEffect == _snowEffects[1] && index == 0)
            {
                _currentSnowEffect.SetActive(false);
            }

            _currentSnowEffect = _snowEffects[index];
            _currentSnowEffect.SetActive(true);
        }

        public void Initialize(Entity entity)
        {
            
        }
    }
}