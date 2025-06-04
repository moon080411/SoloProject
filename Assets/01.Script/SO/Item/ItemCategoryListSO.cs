using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _01.Script.SO.Item
{
    [CreateAssetMenu(fileName = "ItemCategory", menuName = "SO/Items/Category", order = 0)]
    public class ItemCategoryListSO : ScriptableObject
    {
        [field: SerializeField,SerializedDictionary("CategoryName","ItemListSO")]
        public SerializedDictionary<string, ItemListSO> Items { get; private set; }
    }
}