using _01.Script.Items;
using _01.Script.Players;
using _01.Script.SO.Item;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Script.Fires
{
    public class Bonfire : Fire , IActionable
    {
        [SerializeField] private GameObject bigBonFire;
        [SerializeField] private GameObject smallBonFire;
        [SerializeField] private GameObject fireGone;
        [SerializeField] private float BigTime = 100f;
        [SerializeField] private ItemCategoryListSO itemCategoryList;
        [SerializeField] private float torchGiveTime = 60;
        [SerializeField] private GameObject torchPrefab;
        private Transform _currentBonFire;

        protected override void Awake()
        {
            base.Awake();
            bigBonFire.SetActive(false);
            smallBonFire.SetActive(false);
            fireGone.SetActive(false);
        }

        protected override void Update()
        {
            if (timer <= 0f)
            {
                if (_currentBonFire != fireGone.transform)
                {
                    LightGone();
                }
                return;
            }
            base.Update();
            if (timer >= BigTime && _currentBonFire != bigBonFire.transform)
            {
                lightSource.enabled = true;
                fireGone.SetActive(false);
                smallBonFire.SetActive(false);
                bigBonFire.SetActive(true);
                _currentBonFire = bigBonFire.transform;
            }
            else if (timer < BigTime && _currentBonFire != smallBonFire.transform)
            {
                lightSource.enabled = true;
                fireGone.SetActive(false);
                bigBonFire.SetActive(false);
                smallBonFire.SetActive(true);
                _currentBonFire = smallBonFire.transform;
            }
        }

        protected void LightGone()
        {
            _currentBonFire = fireGone.transform;
            smallBonFire.SetActive(false);
            bigBonFire.SetActive(false);
            fireGone.SetActive(true);
            lightSource.enabled = false;
        }

        public void Action()
        {
        }

        public void ItemAction(Item item)
        {
            if (item == null)
            {
                if (timer >= torchGiveTime)
                {
                    Torch torch = Instantiate(torchPrefab).GetComponentInChildren<Torch>();
                    torch.SetTorch(torchGiveTime);
                    timer -= torchGiveTime;
                    _player.ScInventory.AddItem(torch.GetComponent<Item>());
                    return;
                }

                if (timer > 0)
                {
                    Torch torch = Instantiate(torchPrefab).GetComponentInChildren<Torch>();
                    torch.SetTorch(timer);
                    timer = -1;
                    _player.ScInventory.AddItem(torch.GetComponent<Item>());
                    return;
                }

                return;
            }
            if (itemCategoryList.Items["WOOD"].Items.Contains(item.ItemSo))
            {
                if (timer <= 0)
                {
                    return;
                }
                timer += item.ItemSo.FloatValue["FIREPOWER"];
                ItemDestroy(item);
            }
            else if (itemCategoryList.Items["TORCH"].Items.Contains(item.ItemSo))
            {
                if (timer <= 0)
                {
                    timer = 0;
                    lightSource.enabled = true;
                }
                timer += item.GetComponent<Torch>().GetTime(item.ItemSo.FloatValue["FIREMULTIPLY"]);
                ItemDestroy(item);
            }
        }

        protected override void TimerEnd()
        {
            _player.ScMental.TryRemoveLight(this);
        }

        private void ItemDestroy(Item item)
        {
            int index = playerFinder.GetTarget<Player>().ScInventory.FindItemIndex(item);
            playerFinder.GetTarget<Player>().ScInventory.DestroyedItem(index);
        }
    }
}
