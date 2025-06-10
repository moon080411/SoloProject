using System;
using System.Collections.Generic;
using _01.Script.Fires;
using Unity.VisualScripting;
using UnityEngine;

namespace _01.Script.Manager
{
    public class FireManager : MonoBehaviour
    {
        HashSet<Transform> inFire = new HashSet<Transform>();
        
        [SerializeField]List<Fire> firelist = new List<Fire>();


        public void AddFire(Fire fire)
        {
            if (!firelist.Contains(fire))
            {
                firelist.Add(fire);
            }
        }
        
        public void RemoveFire(Fire fire)
        {
            if (firelist.Contains(fire))
            {
                firelist.Remove(fire);
            }
        }

        public bool CheckInFire(Transform item)
        {
            inFire.Clear();
            foreach (var fire in firelist)
            {
                HashSet<Transform> fireItem = fire.FireCheck();
                foreach (var fireItemTransform in fireItem)
                {
                    inFire.Add(fireItemTransform);
                }
            }
            if (inFire.Contains(item))
            {
                return true;
            }
            return false;
        }
    }
}