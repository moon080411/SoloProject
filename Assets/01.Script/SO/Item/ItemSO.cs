using UnityEngine;

namespace _01.Script.SO.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "SO/Item", order = 0)]
    public class ItemSO : ScriptableObject
    {
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public Color ItemColor { get; private set; } = Color.white;
        [field: SerializeField] public string ItemTooltip { get; private set; } 
        [field: SerializeField] public Color TooltipColor { get; private set; } = Color.white;
        [field: SerializeField] public Color BackgroundColor { get; private set; } = Color.white;
        [field: SerializeField] public Sprite ItemIcon { get; private set; }
        [field: SerializeField] public float Mass { get; private set; } = 1f;
    }
}