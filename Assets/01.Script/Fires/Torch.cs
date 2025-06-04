using System;
using _01.Script.Items;
using _01.Script.Players;
using UnityEngine;

namespace _01.Script.Fires
{
    public class Torch : Fire
    {
        private void FixedUpdate()
        {
            lightSource.transform.rotation = Quaternion.LookRotation(-Vector3.up, -Vector3.forward);
            lightSource.transform.position = new Vector3(transform.position.x, 10.62f, transform.position.z);
        }

        public void SetTorch(float changeTime)
        {
            timer = changeTime;
        }
        public float GetTime(float multiply)
        {
            return timer * multiply;
        }

        protected override void TimerEnd()
        {
            _player.ScMental.TryRemoveLight(this);
            if (_player.ScInventory.GetCurrentItem() == GetComponent<Item>())
            {
                _player.ScInventory.DestroyedItem(_player.ScInventory.InventoryPoint);
            }
            else
            {
                base.TimerEnd();
            }
        }

        public void LightRemove()
        {
            _player.ScMental.TryRemoveLight(this);
        }
    }
}