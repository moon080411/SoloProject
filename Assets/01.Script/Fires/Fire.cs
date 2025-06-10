using System;
using System.Collections;
using System.Collections.Generic;
using _01.Script.Manager;
using _01.Script.Players;
using Plugins.SerializedFinder.RunTime.Finder;
using UnityEngine;

namespace _01.Script.Fires
{
    public abstract class Fire : MonoBehaviour
    {
        [SerializeField] protected ScriptFinderSO playerFinder;
        [SerializeField] protected ScriptFinderSO fireLightManagerFinder;
        [SerializeField] protected ScriptFinderSO fireManagerFinder;
        [SerializeField] protected float timer = 150f;
        [SerializeField] protected float maxRangeTime = 450f;
        [SerializeField] protected float maxRange = 15f;
        [SerializeField] protected float nowRangeSmallPercent = 0.35f;
        [SerializeField] protected Light lightSource;
        protected float _nowRange = 0f;
        protected float _lightRange = 0f;
        protected float _lightTime = 0f;
        protected bool _isLightCange = false;
        protected Coroutine _coroutine;
        protected HashSet<Transform> nowFireCheckables = new HashSet<Transform>();

        
        protected Player _player;
        
        protected float NowRangeSmallPercent => _lightRange * nowRangeSmallPercent;

        protected virtual void Awake()
        {
            _player = playerFinder.GetTarget<Player>();
            LightOn();
        }

        protected virtual void OnEnable()
        {
            fireManagerFinder.GetTarget<FireManager>().AddFire(this);
            LightOn();
        }
        
        protected virtual void OnDestroy()
        {
            fireManagerFinder.GetTarget<FireManager>().AddFire(this);
            LightRemove();
        }

        protected virtual void OnDisable()
        {
            fireManagerFinder.GetTarget<FireManager>().RemoveFire(this);
        }

        protected virtual void Update()
        {
            TimeCheck();
            RangeSet();
            PlayerInCheck();
            if (!_isLightCange)
            {
                LightRangeSet();
            }
            LightSet();
            if (timer <= 0f)
            {
                TimerEnd();
            }
        }

        protected virtual void TimeCheck()
        {
            timer -= Time.deltaTime;
        }
        
        public void LightOn()
        {
            LightEnd();
            _lightTime = 0;
            _coroutine =  StartCoroutine(LightOnSet());
        }

        public virtual HashSet<Transform> FireCheck()
        {
            if (!CheckFire())
                return null;
            nowFireCheckables.Clear();
            var colliders = Physics.OverlapSphere(transform.position, _nowRange);
        
            foreach (var collider in colliders)
            {
                if (!collider.TryGetComponent(out IFireChekable checkable))
                    continue;
            
                nowFireCheckables.Add(collider.transform);
            }

            return nowFireCheckables;
        }

        protected virtual bool CheckFire()
        {
            return true;
        }

        public void LightUP()
        {
            LightEnd();
            _lightTime = 0;
            _coroutine = StartCoroutine(LightUPSet());
        }

        private IEnumerator LightUPSet()
        {
            _isLightCange = true;
            while (_lightTime * 2 < 1f)
            {
                _lightRange = Mathf.Lerp(_lightRange, _nowRange * 10.61f, _lightTime * 2f);
                yield return null;
                _lightTime += Time.deltaTime;
            }
            _lightRange = _nowRange * 10.61f;
            _isLightCange = false;
        }

        public void LightDown()
        {
            LightEnd();
            _lightTime = 0;
            _coroutine = StartCoroutine(LightDownSet());
        }

        public void LightEnd()
        {
            if(_coroutine == null)
                return;
            StopCoroutine(_coroutine);
            _isLightCange = false;
        }

        protected IEnumerator LightOnSet()
        {
            _isLightCange = true;
            while (_lightTime < 1f)
            {
                _lightRange = Mathf.Lerp(0f, _nowRange * 10.61f, _lightTime);
                yield return null;
                _lightTime += Time.deltaTime;
            }
            _lightRange = _nowRange * 10.61f;
            _isLightCange = false;
        }
        
        protected IEnumerator LightDownSet()
        {
            _isLightCange = true;
            while (_lightTime < 1f)
            {
                _lightRange = Mathf.Lerp(_lightRange, _nowRange * 10.61f, _lightTime);
                yield return null;
                _lightTime += Time.deltaTime;
            }
            _lightRange = _nowRange * 10.61f;
            _isLightCange = false;
        }

        public void LightOff()
        {
            fireLightManagerFinder.GetTarget<FireLightManager>().FireDie(lightSource.transform.position , _lightRange , nowRangeSmallPercent , lightSource.intensity , lightSource.range , lightSource.color);
        }

        protected virtual void RangeSet()
        {
            _nowRange = Mathf.Clamp(timer / maxRangeTime, 0.0f, 1.0f) * maxRange;
        }

        protected virtual void LightRangeSet()
        {
            _lightRange = _nowRange * 10.61f;
        }

        protected virtual void LightSet()
        {
            lightSource.innerSpotAngle = NowRangeSmallPercent;
            lightSource.spotAngle = _lightRange;
        }

        public void LightRemove()
        {
            fireManagerFinder.GetTarget<FireManager>().RemoveFire(this);
            _player.ScMental.TryRemoveLight(this);
        }

        protected virtual void PlayerInCheck()
        {
            float distance = Vector3.Distance(transform.position, playerFinder.GetTargetTransform().position);
            if (distance <= _nowRange)
            {
                _player.ScMental.TryAddLight(this);
            }
            else
            {
                _player.ScMental.TryRemoveLight(this);
            }
        }

        protected virtual void TimerEnd()
        {
            _player.ScMental.TryRemoveLight(this);
            Destroy(gameObject);
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, maxRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _nowRange);
        }
    }
}