using System;
using System.Collections.Generic;
using _01.Script.Entities;
using _01.Script.Items;
using _01.Script.Manager;
using _01.Script.SO;
using Plugins.SerializedFinder.RunTime.Finder;
using UnityEngine;
using System.Linq;
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
            return CheckItemCategory(category, _items[InventoryPoint].ItemSo);
        }

        private void ChangePoint(float plus)
        {
            int nowPlus;
            nowPlus = Mathf.Clamp(Mathf.RoundToInt(plus), -1, 1);
            InventoryPoint = (InventoryPoint + nowPlus) % _maxCapacity;
            if (InventoryPoint < 0)
            {
                InventoryPoint += _maxCapacity;
            }
            
            if (NowCheckItemCategory("TORCH"))
            {
                
            }
                
            OnInventoryPointChanged?.Invoke();
        }

        public void DestroyedItem(int point)
        {
            _items[point] = null;
            uiManagerFinder.GetTarget<UIManager>().DropItemIcon(point);
        }

        private void SetPoint(int point)
        {
            if (point < _maxCapacity)
            {
                InventoryPoint = point;
                OnInventoryPointChanged?.Invoke();
            }
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
                if (!item.GetComponent<Rigidbody>())
                {
                    item.SetRigidbody();
                }
            }
        }
        
        public void AddItem(Item item)
        {
            int filledCount = _items.Count(item => item != null);
            if (filledCount >= _maxCapacity)
                return;
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
        }
    }
}