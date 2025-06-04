using System;
using _01.Script.Players;
using Plugins.SerializedFinder.RunTime.Finder;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01.Script.Fires
{
    public abstract class Fire : MonoBehaviour
    {
        [SerializeField] protected ScriptFinderSO playerFinder;
        [SerializeField] protected float timer = 150f;
        [SerializeField] protected float maxRangeTime = 450f;
        [SerializeField] protected float maxRange = 15f;
        [SerializeField] protected float nowRangeSmallPercent = 0.35f;
        [SerializeField] protected Light lightSource;
        protected float _nowRange = 0f;
        
        protected Player _player;
        
        protected float NowRangeSmallPercent => _nowRange * nowRangeSmallPercent;

        protected virtual void Awake()
        {
            _player = playerFinder.GetTarget<Player>();
        }

        protected virtual void Update()
        {
            timer -= Time.deltaTime;
            RangeSet();
            PlayerInCheck();
            LightSet();
            if (timer <= 0f)
            {
                TimerEnd();
            }
        }

        protected virtual void RangeSet()
        {
            _nowRange = (timer / maxRangeTime) * maxRange;
        }

        protected virtual void LightSet()
        {
            lightSource.innerSpotAngle = NowRangeSmallPercent * 10.61f;
            lightSource.spotAngle = _nowRange * 10.61f;
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