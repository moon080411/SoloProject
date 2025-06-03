using _01.Script.SO.Item;
using UnityEngine;
using UnityEngine.UI;

namespace _01.Script.Items
{
    public class ItemBag : MonoBehaviour
    {
        [field: SerializeField] public Image BackgroundImage { get; private set; }
        [field: SerializeField] public Image ItemIcon { get; private set; }
        [field: SerializeField] public GameObject Selector { get; private set; }

        public void SetImage(ItemSO item)
        {
            ItemIcon.sprite = item.ItemIcon;
            BackgroundImage.color = item.BackgroundColor;
        }

        public void SetImage(Sprite sprite)
        {
            ItemIcon.sprite = sprite;
            BackgroundImage.color = Color.white;
        }
        
        public void SetSelectorActive(bool isActive)
        {
            Selector.SetActive(isActive);
        }
    }
}