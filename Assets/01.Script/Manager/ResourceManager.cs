using _01.Script.Items;
using _01.Script.Players;
using _01.Script.Pooling;
using Plugins.ScriptFinder.RunTime.Finder;
using UnityEngine;

namespace _01.Script.Manager
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] private ScriptFinderSO playerFinder;
        
        public Player Player { get; private set; }
        
        
        [SerializeField] private Transform woodPrefab;

        private Pool _woodPool;
        
        private Transform _woodParent;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            //_woodParent = new GameObject("WoodPool").transform;
            //_woodParent.SetParent(transform);
            //_woodPool = new Pool(_woodParent, woodPrefab, 20);
            Player = playerFinder.GetTarget<Player>();
        }

        public void AddItemToInventory(Item item)
        {
            Player.ScInventory.AddItem(item);
        }
    }
}