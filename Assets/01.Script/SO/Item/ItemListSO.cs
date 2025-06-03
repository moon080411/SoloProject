using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _01.Script.SO.Item
{
    [CreateAssetMenu(fileName = "ItemList", menuName = "SO/ItemList", order = 0)]
    public class ItemListSO : ScriptableObject
    {
        [field: SerializeField]
        public List<ItemSO> Items { get; private set; }
    }
}