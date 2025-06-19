using System;
using UnityEngine;

namespace _01.Script
{
    public class TimeSet : MonoBehaviour
    {
        private void Awake()
        {
            Time.timeScale = 1f;
        }
    }
}
