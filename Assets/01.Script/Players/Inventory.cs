using System;
using System.Collections.Generic;
using _01.Script.Entities;
using _01.Script.Items;
using _01.Script.Manager;
using _01.Script.SO;
using Plugins.SerializedFinder.RunTime.Finder;
using UnityEngine;
using System.Linq;
using _01.Script.Fires;
using _01.Script.SO.Item;
using Unity.VisualScripting;

namespace _01.Script.Players
{
    public class Inventory : MonoBehaviour , IEntityComponent
    {
        private int _maxCapacity = 3;
        
        public int MaxCapacity => _maxCapacity;

        [SerializeField]private List<Item> _items;
        [SerializeField] private ScriptFinderSO uiManagerFinder;
        [SerializeField] private InventoryInputSO inventoryInput;
        [SerializeField] private ItemCategoryListSO itemCategoryList;
        [SerializeField] private Transform handTrans;

        public int InventoryPoint { get; private set; } = 0;
        
        
        
        public event Action OnInventoryPointChanged;
        
        private Entity _entity;
        
        public void Initialize(Entity entity)
        {
            _items = new List<Item>(new Item[_maxCapacity]);
            _entity = entity;
            inventoryInput.OnNumberKeyPressed += SetPoint;
            inventoryInput.OnScrollWheel += ChangePoint;
            inventoryInput.OnQKeyPressed += DropItem;
        }
        
        private void OnDestroy()
        {
            inventoryInput.OnNumberKeyPressed -= SetPoint;
            inventoryInput.OnScrollWheel -= ChangePoint;
            inventoryInput.OnQKeyPressed -= DropItem;
        }

        public bool CheckItemCategory(string category, ItemSO itemSO)
        {
            return itemCategoryList.Items[category].Items.Contains(itemSO);
        }
        
        public bool NowCheckItemCategory(string category)
        {
            if(_items[InventoryPoint] == null)
                return false;
            return CheckItemCategory(category, _items[InventoryPoint].ItemSo);
        }
        
        public Item GetItem(int point)
        {
            if (point < 0 || point >= _maxCapacity || _items[point] == null)
                return null;
            return _items[point];
        }

        public Item GetCurrentItem()
        {
            if (_items[InventoryPoint] == null)
                return null;
            return _items[InventoryPoint];
        }

        private void ChangePoint(float plus)
        {
            int nowPlus;
            nowPlus = Mathf.Clamp(Mathf.RoundToInt(plus), -1, 1);
            if (nowPlus == 0)
                return;
            ItemSet(false);
            InventoryPoint = (InventoryPoint + nowPlus) % _maxCapacity;
            if (InventoryPoint < 0)
            {
                InventoryPoint += _maxCapacity;
            }
            ItemSet(true);
                
            OnInventoryPointChanged?.Invoke();
        }
        public void SetPoint(int point)
        {
            if(point < 0 || point == InventoryPoint)
                return;
            if (point < _maxCapacity)
            {
                ItemSet(false);
                InventoryPoint = point;
                OnInventoryPointChanged?.Invoke();
                ItemSet(true);
            }
        }

        private void ItemSet(bool isActive)
        {
            if (_items[InventoryPoint] == null)
                return;
            _items[InventoryPoint].gameObject.SetActive(isActive);
            if (isActive)
            {
                SetTransformToHand(_items[InventoryPoint].transform);
            }
            else
            {
                if (NowCheckItemCategory("TORCH"))
                {
                    _items[InventoryPoint].GetComponent<Torch>().LightOff();
                    _items[InventoryPoint].GetComponent<Torch>().LightRemove();
                }
                RemoveTransformToHand(_items[InventoryPoint].transform);
            }
        }

        public void DestroyedItem(int point)
        {
            if(!_items[point] || point >= _maxCapacity || point < 0)
                return;
            Destroy(_items[point].gameObject);
            _items[point] = null;
            uiManagerFinder.GetTarget<UIManager>().DropItemIcon(point);
        }

        private void SetTransformToHand(Transform trans)
        {
            trans.parent = handTrans;
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
        }

        private void RemoveTransformToHand(Transform trans)
        {
            trans.parent = null;
        }

        
        public void AddMaxCapacity(int amount)
        {
            _maxCapacity += amount;
            int addCount = _maxCapacity - _items.Count;
            for (int i = 0; i < addCount; i++)
            {
                _items.Add(null);
            }
        }

        public void SetMaxCapacity(int amount)
        {
            if (amount > _maxCapacity)
            {
                _maxCapacity = amount;
                _items.Capacity = _maxCapacity;
                int addCount = _maxCapacity - _items.Count;
                for (int i = 0; i < addCount; i++)
                {
                    _items.Add(null);
                }
            }
        }

        private void DropItem()
        {
            if (_items[InventoryPoint] != null)
            {
                Item item = _items[InventoryPoint];
                _items[InventoryPoint] = null;
                item.gameObject.SetActive(true);
                item.GetOutOfInventory(_entity.transform);
                uiManagerFinder.GetTarget<UIManager>().DropItemIcon(InventoryPoint);
                item.GetComponent<Collider>().isTrigger = false;
                if (!item.GetComponent<Rigidbody>())
                {
                    item.SetRigidbody();
                }
            }
        }
        
        public void AddItem(Item item)
        {
            int filledCount = _items.Count(item => item != null);
            if (filledCount >= _maxCapacity || _items.Contains(item))
                return;
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Destroy(rb);
            }
            item.GetComponent<Collider>().isTrigger = true;
            if (_items[InventoryPoint] == null)
            {
                _items[InventoryPoint] = item;
                uiManagerFinder.GetTarget<UIManager>().SetItemIcon(InventoryPoint, item.ItemSo);
            }
            else
            {
                int emptyIndex = _items.FindIndex(i => i == null);
                if (emptyIndex != -1)
                {
                    _items[emptyIndex] = item;
                    uiManagerFinder.GetTarget<UIManager>().SetItemIcon(emptyIndex, item.ItemSo);
                }
            }
            item.gameObject.SetActive(false);
            _items[InventoryPoint].gameObject.SetActive(true);
            SetTransformToHand(_items[InventoryPoint].transform);
        }

        public int FindItemIndex(Item item)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] != null && _items[i] == item)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}