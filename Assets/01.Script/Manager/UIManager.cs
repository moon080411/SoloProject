using System.Collections.Generic;
using _01.Script.Items;
using _01.Script.Players;
using _01.Script.Pooling;
using _01.Script.SO.Item;
using Plugins.SerializedFinder.RunTime.Finder;
using UnityEngine;

namespace _01.Script.Manager
{
    public class UIManager : MonoBehaviour
    {
        
        [SerializeField] private ScriptFinderSO playerFinder;

        private Inventory _inventory;

        private ItemBag _currentSelected;
        
        List<ItemBag> _items = new List<ItemBag>();
        
        private Dictionary<Item , Transform> _itemTooltips = new Dictionary<Item, Transform>();
        
        [SerializeField]private Transform tooltipPrefab;
        [SerializeField] private Transform itemBagPrefab;
        [SerializeField] private Sprite nullImage;
        [SerializeField] private Transform itemTooltipParent;
        [SerializeField] private Transform itemBagParent;
        
        private Pool _pool;

        private void Awake()
        {
            _pool = new Pool(tooltipPrefab, itemTooltipParent);
            
        }
        
        private void Start()
        {
            _inventory = playerFinder.GetTarget<Player>().ScInventory;
            
            SpawnBag(_inventory.MaxCapacity);
            
            _inventory.OnInventoryPointChanged +=  SelectItemBag;
        }

        private void OnDestroy()
        {
            _inventory.OnInventoryPointChanged -= SelectItemBag;
        }

        private void SpawnBag(int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                _items.Add(Instantiate(itemBagPrefab, Vector3.zero, Quaternion.identity , itemBagParent).GetComponent<ItemBag>());
                _items[i].SetSelectorActive(false);
            }
            _currentSelected = _items[0];
            _currentSelected.SetSelectorActive(true);
        }

        private void ReSpawnBag(int amount = 1)
        {
            int nowAmount = _items.Count;
            for (int i = nowAmount; i < nowAmount + amount; i++)
            {
                _items.Add(Instantiate(itemBagPrefab, Vector3.zero, Quaternion.identity , itemBagParent).GetComponent<ItemBag>());
                _items[i].SetSelectorActive(false);
            }
        }
        
        private void SelectItemBag()
        {
            _currentSelected.SetSelectorActive(false);
            _currentSelected = _items[_inventory.InventoryPoint];
            _currentSelected.SetSelectorActive(true);
        }

        public void SetItemIcon(int where, ItemSO item)
        {
            _items[where].SetImage(item);
        }
        
        public void DropItemIcon(int where)
        {
            _items[where].SetImage(nullImage);
        }

        

        public Transform ShowItemTooltip(Item item)
        {
            _itemTooltips.Add(item, _pool.Pop());
            _itemTooltips[item].GetComponent<ItemTooltip>().SetTooltip(item.ItemSo);
            return _itemTooltips[item];
        }
        
        public void HideItemTooltip(Item item)
        {
            if (_itemTooltips.TryGetValue(item, out var tooltip))
            {
                _pool.Push(tooltip);
                _itemTooltips.Remove(item);
            }
            else
            {
                Debug.LogWarning($"Tooltip for item {item.ItemSo.ItemName} not found.");
            }
        }
    }
}