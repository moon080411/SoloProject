using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _01.Script.SO.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "SO/Items/Item", order = 0)]
    public class ItemSO : ScriptableObject
    {
        [field: Header("Bag And Tooltip")]
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public Color ItemColor { get; private set; } = Color.white;
        [field: SerializeField] public string ItemTooltip { get; private set; } 
        [field: SerializeField] public Color TooltipColor { get; private set; } = Color.white;
        [field: SerializeField] public Color BackgroundColor { get; private set; } = Color.white;
        [field: SerializeField] public Sprite ItemIcon { get; private set; }
        [field: SerializeField] public float Mass { get; private set; } = 1f;

        [field: Header("Values")]
        [field: SerializeField, SerializedDictionary("Name","Value")] 
        public SerializedDictionary<string, string> StringValue {get; private set; }
        
        [field: SerializeField, SerializedDictionary("Name","Value")] 
        public SerializedDictionary<string, int> IntValue {get; private set;}
        
        [field: SerializeField, SerializedDictionary("Name","Value")] 
        public SerializedDictionary<string, float> FloatValue {get; private set; }
        
        [field: SerializeField, SerializedDictionary("Name","Value")] 
        public SerializedDictionary<string, bool> BoolValue {get; private set;}
    }
}