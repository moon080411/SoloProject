using System;
using UnityEngine;

namespace _01.Script.Fire
{
    public class Bonfire : Fire
    {
        [SerializeField] private GameObject bigBonFire;
        [SerializeField] private GameObject smallBonFire;
        [SerializeField] private float BigTime = 100f;
        private Transform _currentBonFire;

        private void Awake()
        {
            bigBonFire.SetActive(false);
            smallBonFire.SetActive(false);
        }

        protected override void Update()
        {
            if (timer >= BigTime && _currentBonFire != bigBonFire.transform)
            {
                smallBonFire.SetActive(false);
                bigBonFire.SetActive(true);
                _currentBonFire = bigBonFire.transform;
            }
            else if (timer < BigTime && _currentBonFire != smallBonFire.transform)
            {
                bigBonFire.SetActive(false);
                smallBonFire.SetActive(true);
                _currentBonFire = smallBonFire.transform;
            }
        }
    }
}
