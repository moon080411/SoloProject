using System.Collections;
using System.Collections.Generic;
using _01.Script.Pooling;
using UnityEngine;

namespace _01.Script.Manager
{
    public class FireLightManager : MonoBehaviour
    {
        [SerializeField] private Transform prefab;
        private Pool _fire;

        private void Awake()
        {
            _fire = new Pool(prefab, transform, 3);
        }
        public void FireDie(Vector3 pos , float range , float smallPercent, float intensity, float lightSourceRange, Color color)
        {
            if(range <= 0) return;
            StartCoroutine(FireDieStart(pos, range , smallPercent, intensity, lightSourceRange , color));
        }
        
        private IEnumerator FireDieStart(Vector3 pos, float range, float smallPercent, float intensity, float lightSourceRange , Color color)
        {
            Transform fire = _fire.Pop();
            float timer = 0f;
            fire.position = pos;
            Light light = fire.GetComponentInChildren<Light>();
            light.color = color;
            light.range = lightSourceRange;
            light.intensity = intensity;
            var vector3 = fire.position;
            vector3.y = fire.position.y;
            fire.position = vector3;
            while (timer < 1f)
            {
                light.innerSpotAngle = Mathf.Lerp(range * smallPercent, 0f, timer);
                light.spotAngle = Mathf.Lerp(range, 0f, timer);
                timer += Time.deltaTime;
                yield return null;
            }
            _fire.Push(fire);
        }
    }
}
