using System.Collections.Generic;
using _01.Script.Items;
using _01.Script.Manager;
using _01.Script.SO.Item;
using TMPro;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

namespace _01.Script.Fires
{
    public class Bonfire : Fire , IActionable , IFireChekable
    {
        [SerializeField] private GameObject bigBonFire;
        [SerializeField] private GameObject smallBonFire;
        [SerializeField] private GameObject fireGone;
        [SerializeField] private float BigTime = 100f;
        [SerializeField] private ItemCategoryListSO itemCategoryList;
        [SerializeField] private float torchGiveTime = 60;
        [SerializeField] private GameObject torchPrefab;
        [SerializeField] private TextMeshPro fireText;
        [SerializeField] private List<ItemSO> upgradeItems;
        [SerializeField] private List<int> upgradeCosts;
        private Transform _currentBonFire;
        FireManager fireManager;
        private int _upgradeCount = 0;

        protected override void Awake()
        {
            base.Awake();
            bigBonFire.SetActive(false);
            smallBonFire.SetActive(false);
            fireGone.SetActive(false);
            FireText();
            fireManager = fireManagerFinder.GetTarget<FireManager>();
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
                fireManager.AddFire(this);
                lightSource.enabled = true;
                fireGone.SetActive(false);
                smallBonFire.SetActive(false);
                bigBonFire.SetActive(true);
                fireText.gameObject.SetActive(true);
                _currentBonFire = bigBonFire.transform;
            }
            else if (timer < BigTime && _currentBonFire != smallBonFire.transform)
            {
                fireManager.AddFire(this);
                lightSource.enabled = true;
                fireGone.SetActive(false);
                bigBonFire.SetActive(false);
                smallBonFire.SetActive(true);
                fireText.gameObject.SetActive(true);
                _currentBonFire = smallBonFire.transform;
            }
        }

        private void FireText()
        {
            if (upgradeItems.Count > _upgradeCount)
            {
                Color c = upgradeItems[_upgradeCount].ItemColor;
                string hex = ColorUtility.ToHtmlStringRGBA(c);
                fireText.text = $"next upgrade to \n<color=#{hex}>{upgradeItems[_upgradeCount].ItemName}:{upgradeCosts[_upgradeCount]}</color>";
            }
            else
            {
                fireText.text = "max upgrade";
            } 
        }
        
        protected override void TimeCheck()
        {
            timer -= Time.deltaTime / (_upgradeCount + 1);
        }

        protected void LightGone()
        {
            _currentBonFire = fireGone.transform;
            fireManager.RemoveFire(this);
            smallBonFire.SetActive(false);
            bigBonFire.SetActive(false);
            fireGone.SetActive(true);
            fireText.gameObject.SetActive(false);
            lightSource.enabled = false;
        }

        public void Action()
        {
        }

        protected override bool CheckFire()
        {
            return _currentBonFire != fireGone.transform;
        }

        public void ItemAction(Item item)
        {
            if(_currentBonFire == fireGone.transform && !fireManagerFinder.GetTarget<FireManager>().CheckInFire(transform))
                return;
            if (item == null)
            {
                if (timer >= torchGiveTime)
                {
                    Torch torch = Instantiate(torchPrefab).GetComponentInChildren<Torch>();
                    torch.SetTorch(torchGiveTime);
                    timer -= torchGiveTime;
                    _player.ScInventory.AddItem(torch.GetComponent<Item>());
                    LightDown();
                    return;
                }

                if (timer > 0)
                {
                    Torch torch = Instantiate(torchPrefab).GetComponentInChildren<Torch>();
                    torch.SetTorch(timer);
                    timer = -1;
                    _player.ScInventory.AddItem(torch.GetComponent<Item>());
                    LightDown();
                    return;
                }
                return;
            }

            if (upgradeItems.Count > _upgradeCount)
            {
                if (itemCategoryList.Items["UPGRADE"].Items.Contains(item.ItemSo))
                {
                    if (timer <= 0)
                    {
                        return;
                    }
                    if (item.ItemSo == upgradeItems[_upgradeCount])
                    {
                        ItemDestroy(item);
                        upgradeCosts[_upgradeCount]--;
                        if (upgradeCosts[_upgradeCount] <= 0)
                        {
                            _upgradeCount++;
                        }

                        FireText();
                    }
                }
            }
            if (itemCategoryList.Items["WOOD"].Items.Contains(item.ItemSo))
            {
                if (timer <= 0)
                {
                    return;
                }
                timer += item.ItemSo.FloatValue["FIREPOWER"];
                ItemDestroy(item);
                LightUP();
            }
            else if (itemCategoryList.Items["TORCH"].Items.Contains(item.ItemSo))
            {
                float multiply = 1;
                if (timer <= 0)
                {
                    timer = 0;
                    lightSource.enabled = true;
                    LightOn();
                    multiply = 0.5f;
                    _player.ScMental.AddMental(-25f);
                }
                else
                {
                    LightUP();
                }
                timer += item.GetComponent<Torch>().GetTime(item.ItemSo.FloatValue["FIREMULTIPLY"] * multiply);
                item.GetComponent<Torch>().LightOff();
                ItemDestroy(item);
            }
        }

        protected override void TimerEnd()
        {
            _player.ScMental.TryRemoveLight(this);
        }

        private void ItemDestroy(Item item)
        {
            int index = _player.ScInventory.FindItemIndex(item);
            _player.ScInventory.DestroyedItem(index);
        }

    }
}
