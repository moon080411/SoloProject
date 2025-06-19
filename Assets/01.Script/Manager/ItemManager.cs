    using System.Collections.Generic;
using _01.Script.Items;
using _01.Script.Players;
using Plugins.ScriptFinder.RunTime.Finder;
using UnityEngine;

namespace _01.Script.Manager
{
    public class ItemManager : MonoBehaviour
    {
        private HashSet<GameObject> _items = new HashSet<GameObject>();
        private Dictionary<GameObject , Item> _itemsDictionary = new Dictionary<GameObject , Item>();
        [SerializeField] private ScriptFinderSO playerFinder;
        [SerializeField] private float seeDistance = 35f;
        [SerializeField]private float updateInterval = 0.5f;
        private float _updateTimer = 0f;
        private Player _player;

        private void Awake()
        {
            _player = playerFinder.GetTarget<Player>();
        }

        public void AddItem(GameObject item)
        {
            if (item != null)
            {
                if (_items.Add(item))
                {
                    _itemsDictionary.Add(item, item.GetComponent<Item>());
                }
            }
        }


        private void Update()
        {
            _updateTimer += Time.deltaTime;
            if (_updateTimer >= updateInterval)
            {
                _updateTimer = 0f;
                var playerPos = playerFinder.GetTargetTransform().position;
                playerPos.y = 0;
                foreach (GameObject item in _items)
                {
                    if (item == null || _player.ScInventory.CheckItemInInventory(_itemsDictionary[item])) continue;
                    var itemPos = item.transform.position;
                    itemPos.y = 0;
                    float sqrDistance = (playerPos - itemPos).sqrMagnitude;
                    bool shouldBeActive = sqrDistance <= seeDistance * seeDistance;
                    if (item.activeSelf != shouldBeActive)
                    {
                        item.SetActive(shouldBeActive);
                    }
                }
            }
        }

        public void RemoveItem(GameObject item)
        {
            if(item != null && _items.Contains(item))
            {
                _items.Remove(item);
            }
        }
    }
}