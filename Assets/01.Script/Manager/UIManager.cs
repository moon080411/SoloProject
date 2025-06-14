using System;
using System.Collections.Generic;
using _01.Script.Items;
using _01.Script.Players;
using _01.Script.Pooling;
using _01.Script.SO.Item;
using Plugins.ScriptFinder.RunTime.Finder;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _01.Script.Manager
{
    public class UIManager : MonoBehaviour
    {
        
        [SerializeField] private ScriptFinderSO playerFinder;

        private Inventory _inventory;

        private ItemBag _currentSelected;
        
        List<ItemBag> _items = new List<ItemBag>();
        
        [SerializeField]private Transform tooltipPrefab;
        [SerializeField] private Transform itemBagPrefab;
        [SerializeField] private Sprite nullImage;
        [SerializeField] private float tooltipYPlusPos = 20f;
        [SerializeField] private Transform itemTooltipParent;
        [SerializeField] private Transform itemBagParent;
        [SerializeField] private TextMeshProUGUI fogText;
        [SerializeField] private TextMeshProUGUI fireText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private TextMeshProUGUI gameOverText;
        
        private Item _currentItem;

        private Transform _tooltip;

        private void Awake()
        {
            _tooltip = Instantiate(tooltipPrefab, Vector3.zero, Quaternion.identity, itemTooltipParent);
            _tooltip.gameObject.SetActive(false);
            fogText.gameObject.SetActive(false);
            fireText.gameObject.SetActive(false);
            gameOverUI.SetActive(false);
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

        private void FixedUpdate()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            mousePosition.y += tooltipYPlusPos;
            _tooltip.position = mousePosition;
        }


        public void ShowItemTooltip(Item item)
        {
            if (_currentItem != null)
                HideItemTooltip(_currentItem);
            _tooltip.gameObject.SetActive(true);
            _tooltip.GetComponent<ItemTooltip>().SetTooltip(item.ItemSo);
            _currentItem = item;
        }
        
        public void HideItemTooltip(Item item)
        {
            if (_currentItem != item)
                return;
            _tooltip.gameObject.SetActive(false);
            _currentItem = null;
        }
        
        public void SetTimeText(string time)
        {
            timeText.text = time;
        }

        public void SetFogText(bool b)
        {
            fogText.gameObject.SetActive(b);
        }

        public void FireTextActive(bool b)
        {
            fireText.gameObject.SetActive(b);
        }

        public void ShowGameOver(float timer)
        {
            gameOverUI.SetActive(true);
            gameOverText.text = timer.ToString();
        }
    }
}