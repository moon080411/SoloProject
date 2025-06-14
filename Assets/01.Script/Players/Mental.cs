using System;
using System.Collections.Generic;
using _01.Script.Entities;
using _01.Script.Fires;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _01.Script.Players
{
    public class Mental : MonoBehaviour , IEntityComponent
    {
        [SerializeField]private float maxMental = 100f;
        [SerializeField]private float _mentalRegen = 0.1f;
        [SerializeField]private float _mentalDown = 0.3f;
        [SerializeField] private float _fogDownPer = 0.75f;
        [SerializeField] private float _bornFireDownPer = 1.5f;
        [SerializeField] private float timeToRegen = 0.1f;
        [SerializeField] private float timeToDown = 0.1f;
        [SerializeField] private float timeToFog = 0.1f;
        [SerializeField] private float timeToBornFire = 0.1f;
        [SerializeField] private Slider mentalSlider;
        
        public HashSet<Fire> lights = new HashSet<Fire>();
        
        private float _currentMental = 100f;
        
        private bool _isSafe = true;
        
        private bool _isInFog = false;
        
        private bool _bornFireIsSafe = true;

        private float _timer = 0f;
        
        private float _timerFog = 0f;
        
        private float _timerBornFire = 0f;
        
        public UnityEvent GameOver;

        private void Awake()
        {
            CheckLight();
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            _timerFog += Time.deltaTime;
            _timerBornFire += Time.deltaTime;
            if (_isInFog)
            {
                if (_timerFog >= timeToFog)
                {
                    DownMental(_fogDownPer);
                    _timerFog = 0f;
                }
            }

            if (_bornFireIsSafe == false)
            {
                if (_timerBornFire >= timeToBornFire)
                {
                    DownMental(_bornFireDownPer);
                    _timerBornFire = 0f;
                }
            }
            if (_isSafe)
            {
                if (_timer >= timeToRegen)
                {
                    RegenMental();
                    _timer = 0f;
                }
            }
            else
            {
                if (_timer >= timeToDown)
                {
                    DownMental();
                    _timer = 0f;
                }
            }
        }

        #region Regen,Down

        public void SetTimeToRegen(float value)
        {
            timeToRegen = value;
        }
        public void SetTimeToDown(float value)
        {
            timeToDown = value;
        }
        public void SetMentalRegen(float value)
        {
            _mentalRegen = value;
        }
        public void SetMentalDown(float value)
        {
            _mentalDown = value;
        }

        #endregion
        
        public void SetSafe(bool isSafe)
        {
            if (isSafe != _isSafe)
            {
                _isSafe = isSafe;
                _timer = 0;
            }
        }
        
        public void SetBornFireSafe(bool isSafe)
        {
            if (isSafe != _bornFireIsSafe)
            {
                _bornFireIsSafe = isSafe;
                _timer = 0;
            }
        }

        public void CheckMental()
        {
            if (_currentMental <= 0)
            {
                GameOver?.Invoke();
            }
        }

        #region Mental Control
        public void SetMental(float value)
        {
            _currentMental = Mathf.Clamp(value, 0, maxMental);
            mentalSlider.value = _currentMental / maxMental;
            CheckMental();
        }
        public void AddMental(float value)
        {
            SetMental(_currentMental + value);
        }
        private void RegenMental(float value = 1f)
        {
            SetMental(_currentMental + _mentalRegen * value);
        }
        private void DownMental(float value = 1f)
        {
            SetMental(_currentMental - _mentalDown * value);
        }
        #endregion

        public void Initialize(Entity entity)
        {
            
        }

        public void TryAddLight(Fire light)
        {
            if (lights.Add(light))
            {
                CheckLight();
            }
        }

        public void TryRemoveLight(Fire light)
        {
            if (lights.Remove(light))
            {
                CheckLight();
            }
        }

        private void CheckLight()
        {
            SetSafe(lights.Count > 0);
        }

        public void IsInFogChange(bool isFog)
        {
            if (isFog != _isInFog)
            {
                _isInFog = isFog;
                _timerFog = 0f;
            }
        }
        public void BornFireChange(bool isSafe)
        {
            if (isSafe != _bornFireIsSafe)
            {
                _bornFireIsSafe = isSafe;
                _timerBornFire = 0f;
            }
        }
    }
}
