using System;
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
        [SerializeField] private ScriptFinderSO itemManagerFinder;
        [SerializeField]private Outline _outline;
        
        private Transform _itemTooltip;
        
        public UnityEvent OnItemAction;

        private void Awake()
        {
            _outline.enabled = false;
            itemManagerFinder.GetTarget<ItemManager>().AddItem(gameObject);
        }

        private void OnDestroy()
        {
            itemManagerFinder.GetTarget<ItemManager>().RemoveItem(gameObject);
        }

        private void OnMouseEnter()
        {
            if(!fireManagerFinder.GetTarget<FireManager>().CheckInFire(transform))
                return;
            _outline.enabled = true;
            uiManagerFinder.GetTarget<UIManager>().ShowItemTooltip(this);
        }

        private void OnMouseExit()
        {
            _outline.enabled = false;
            uiManagerFinder.GetTarget<UIManager>().HideItemTooltip(this);
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
            _outline.enabled = false;
            uiManagerFinder.GetTarget<UIManager>().HideItemTooltip(this);
            resourceManagerFinder.GetTarget<ResourceManager>().AddItemToInventory(this);
        }

        public void ItemAction(Item item)
        {
            
        }
    }
}
