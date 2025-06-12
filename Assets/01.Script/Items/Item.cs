using System;
using System.Collections.Generic;
using _01.Script.Fires;
using _01.Script.Manager;
using _01.Script.SO.Item;
using Plugins.ScriptFinder.RunTime.Finder;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace _01.Script.Items
{
    public class Item : MonoBehaviour,IActionable , IFireChekable
    {
        [field:SerializeField] public ItemSO ItemSo { get; private set; }
        [SerializeField] private ScriptFinderSO uiManagerFinder;
        [SerializeField] private ScriptFinderSO fireManagerFinder;
        [SerializeField] private ScriptFinderSO resourceManagerFinder;
        [SerializeField] private int amount = 1;
        
        private Transform _itemTooltip;
        
        public UnityEvent OnItemAction;

        private void OnMouseEnter()
        {
            //여기에 빛나게 되는 함수를 넣으면 됨
            //uiManagerFinder.GetTarget<UIManager>().ShowItemTooltip(this);
        }

        private void OnMouseExit()
        {
            //uiManagerFinder.GetTarget<UIManager>().HideItemTooltip(this);
        }

        public void GetOutOfInventory(Transform entity)
        {
            transform.parent = null;
            transform.position = entity.position + Vector3.Scale(transform.localScale * 0.75f , entity.forward);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.75f , transform.position.z);
            //transform.rotation = entity.rotation;
        }

        public void SetRigidbody()
        {
            Rigidbody rb = transform.AddComponent<Rigidbody>();
            rb.mass = ItemSo.Mass;
            rb.linearDamping = 15;
            rb.angularDamping = 15;
        }

        public void Action()
        {
            if(!fireManagerFinder.GetTarget<FireManager>().CheckInFire(transform))
                return;
            OnItemAction?.Invoke();
            resourceManagerFinder.GetTarget<ResourceManager>().AddItemToInventory(this);
        }

        public void ItemAction(Item item)
        {
            
        }
    }
}
