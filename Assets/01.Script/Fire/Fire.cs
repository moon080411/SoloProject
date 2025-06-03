using System;
using UnityEngine;

namespace _01.Script.Fire
{
    public abstract class Fire : MonoBehaviour
    {
        [SerializeField] protected float timer = 150f;
        [SerializeField] protected float maxRangeTime = 300f;
        [SerializeField] protected float maxRange = 10f;
        [SerializeField] protected float nowRangeSmallPercent = 0.35f;
        [SerializeField] protected float nowRangeRandomPercent = 0.1f;
        protected float _nowRange;
        protected float NowRangeSmallPercent => _nowRange * nowRangeSmallPercent;

        protected virtual void Update()
        {
            timer -= Time.deltaTime;
            RangeSet();
            if (timer <= 0f)
            {
                TimerEnd();
            }
        }

        protected virtual void RangeSet()
        {
            
        }

        protected virtual void TimerEnd()
        {
            Destroy(gameObject);
        }
    }
}