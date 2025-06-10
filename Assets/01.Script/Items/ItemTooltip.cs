using _01.Script.SO.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Script.Items
{
    public class ItemTooltip : MonoBehaviour
    {
        
        [field:SerializeField] public TextMeshProUGUI NameText { get; private set; }
        [field:SerializeField] public TextMeshProUGUI TooltipText { get; private set; }
        [field: SerializeField] public Image BackgroundImage { get; private set; }

        public void SetTooltip(ItemSO item)
        {
            NameText.text = item.ItemName;
            NameText.color = item.ItemColor;
            TooltipText.text = item.ItemTooltip;
            TooltipText.color = item.TooltipColor;
            BackgroundImage.color = item.BackgroundColor;
        }
    }
}