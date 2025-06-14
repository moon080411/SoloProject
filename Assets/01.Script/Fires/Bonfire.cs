using System.Collections;
using System.Collections.Generic;
using _01.Script.Items;
using _01.Script.Manager;
using _01.Script.SO.Item;
using Plugins.ScriptFinder.RunTime.Finder;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ColorUtility = UnityEngine.ColorUtility;

namespace _01.Script.Fires
{
    public class Bonfire : Fire , IActionable , IFireChekable
    {
        [SerializeField] private ScriptFinderSO uiManagerFinder;
        [SerializeField] private GameObject bigBonFire;
        [SerializeField] private GameObject smallBonFire;
        [SerializeField] private GameObject fireGone;
        [SerializeField] private float bigTime = 100f;
        [SerializeField] private ItemCategoryListSO itemCategoryList;
        [SerializeField] private float torchGiveTime = 60;
        [SerializeField] private GameObject torchPrefab;
        [SerializeField] private TextMeshPro fireText;
        [SerializeField] private List<ItemSO> upgradeItems;
        [SerializeField] private List<int> upgradeCosts;
        [SerializeField] private List<float> upgradeTimeMax;
        [SerializeField] private List<float> upgradeMaxRange;
        [SerializeField] private List<float> upgradeTorchGiveTimeMultiply;
        [SerializeField] private Slider fireSlider;
        [SerializeField] private Sprite fireGoneImage;
        [SerializeField] private Sprite fireSmallImage;
        [SerializeField] private Sprite fireBigImage;
        [SerializeField] private Image arrowImage;
        private Image _fillImage;
        private Image _handleImage;
        private RectTransform _rectTransform;
        private Coroutine _fireCoroutine;
        private Coroutine _colorCoroutine;
        private Coroutine _sliderCoroutine;
        private float _torchGiveTimeMultiply = 1f;
        private Transform _currentBonFire;
        private FireManager _fireManager;
        private int _upgradeCount = 0;
        private UIManager _uiManager;

        protected override void Awake()
        {
            base.Awake();
            bigBonFire.SetActive(false);
            smallBonFire.SetActive(false);
            fireGone.SetActive(false);
            maxRangeTime = upgradeTimeMax[0];
            maxRange = upgradeMaxRange[0];
            bigTime = upgradeTimeMax[0] * 0.65f;
            _torchGiveTimeMultiply = upgradeTorchGiveTimeMultiply[0];
            FireText();
            _fireManager = fireManagerFinder.GetTarget<FireManager>();
            _uiManager = uiManagerFinder.GetTarget<UIManager>();
            _rectTransform = fireSlider.GetComponent<RectTransform>();
            _fillImage = fireSlider.fillRect.GetComponent<Image>();
            _handleImage = fireSlider.handleRect.GetComponent<Image>();
        }

        protected override void Update()
        {
            FireSliderSet();
            if (timer <= 0f)
            {
                if (_currentBonFire != fireGone.transform)
                {
                    LightGone();
                }
                return;
            }
            base.Update();
            if (timer >= bigTime && _currentBonFire != bigBonFire.transform)
            {
                _fireManager.AddFire(this);
                lightSource.enabled = true;
                fireGone.SetActive(false);
                smallBonFire.SetActive(false);
                bigBonFire.SetActive(true);
                fireText.gameObject.SetActive(true);
                _currentBonFire = bigBonFire.transform;
                _handleImage.sprite = fireBigImage;
                arrowImage.sprite = fireBigImage;
            }
            else if (timer < bigTime && _currentBonFire != smallBonFire.transform)
            {
                _fireManager.AddFire(this);
                lightSource.enabled = true;
                fireGone.SetActive(false);
                bigBonFire.SetActive(false);
                smallBonFire.SetActive(true);
                fireText.gameObject.SetActive(true);
                _currentBonFire = smallBonFire.transform;
                _handleImage.sprite = fireSmallImage;
                arrowImage.sprite = fireSmallImage;
            }
        }
        
        private void FireSliderSet()
        {
            if (_sliderCoroutine != null)
                return;
            if (_currentBonFire == fireGone.transform)
            {
                fireSlider.value = 0;
            }
            else
            {
                fireSlider.value = timer / maxRangeTime;
            }
        }

        private void FireText()
        {
            if (upgradeItems.Count > _upgradeCount)
            {
                Color c = upgradeItems[_upgradeCount].ItemColor;
                string hex = ColorUtility.ToHtmlStringRGBA(c);
                fireText.text = $"다음 강화까지 남은재료 \n<color=#{hex}>{upgradeItems[_upgradeCount].ItemName}:{upgradeCosts[_upgradeCount]}</color>";
            }
            else
            {
                fireText.text = "최대 강화?";
            } 
        }
        
        protected override void TimeCheck()
        {
            if(timer >= maxRangeTime)
                timer = maxRangeTime;
            timer -= Time.deltaTime / (_upgradeCount + 1);
        }

        protected void LightGone()
        {
            _currentBonFire = fireGone.transform;
            _fireManager.RemoveFire(this);
            smallBonFire.SetActive(false);
            bigBonFire.SetActive(false);
            fireGone.SetActive(true);
            fireText.gameObject.SetActive(false);
            lightSource.enabled = false;
            _handleImage.sprite = fireGoneImage;
            arrowImage.sprite = fireGoneImage;
            _player.ScMental.SetBornFireSafe(false);
            _uiManager.FireTextActive(true);
        }

        public void Action()
        {
        }

        protected override bool CheckFire()
        {
            return _currentBonFire != fireGone.transform;
        }

        private void FireCoroutine()
        {
            CoroutineCheck(_fireCoroutine);
            _fireCoroutine = StartCoroutine(FireUpgrade());
        }
        
        private void SliderCoroutine()
        {
            CoroutineCheck(_sliderCoroutine);
            _sliderCoroutine = StartCoroutine(SliderUpgrade());
        }

        private IEnumerator SliderUpgrade()
        {
            float time = 0;
            float baseValue = fireSlider.value;
            while(time < 1)
            {
                time += Time.deltaTime;
                yield return null;
                fireSlider.value = Mathf.Lerp(baseValue, timer / maxRangeTime, time);
            }
            fireSlider.value = timer / maxRangeTime;
        }

        private void CoroutineCheck(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }

        private IEnumerator FireUpgrade()
        {
            float time = 0;
            float baseWidth = _rectTransform.sizeDelta.x;
            float target = upgradeTimeMax[_upgradeCount] / upgradeTimeMax[_upgradeCount - 1] * baseWidth;
            while (time < 1)
            {
                time += Time.deltaTime;
                yield return null;
                var delta = _rectTransform.sizeDelta;
                delta.x = Mathf.Lerp(baseWidth, target, time);
                _rectTransform.sizeDelta = delta;
            }
            _rectTransform.sizeDelta = new Vector2(target, _rectTransform.sizeDelta.y);
        }
        
        private void ColorCorutine()
        {
            CoroutineCheck(_colorCoroutine);
            _colorCoroutine = StartCoroutine(ColorChange());
        }

        private IEnumerator ColorChange()
        {
            float time = 0;
            Color startColor = _fillImage.color;
            Color targetColor = upgradeItems[_upgradeCount - 1].ItemColor;
            while (time < 1f)
            {
                time += Time.deltaTime;
                _fillImage.color = Color.Lerp(startColor, targetColor, time);
                yield return null;
            }
            _fillImage.color = targetColor;
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
                    torch.SetTorch(torchGiveTime * _torchGiveTimeMultiply);
                    timer -= torchGiveTime;
                    _player.ScInventory.AddItem(torch.GetComponent<Item>());
                    LightDown();
                    return;
                }

                if (timer > 0)
                {
                    Torch torch = Instantiate(torchPrefab).GetComponentInChildren<Torch>();
                    torch.SetTorch(timer * _torchGiveTimeMultiply);
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
                            Upgrade();
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
                if (timer + item.ItemSo.FloatValue["FIREPOWER"] >= upgradeTimeMax[_upgradeCount])
                {
                    timer = upgradeTimeMax[_upgradeCount];
                }
                else
                {
                    timer += item.ItemSo.FloatValue["FIREPOWER"];
                }
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
                    _player.ScMental.SetBornFireSafe(true);
                    LightOn();
                    _uiManager.FireTextActive(false);
                    multiply = 0.5f;
                }
                else
                {
                    LightUP();
                }
                if (timer + item.GetComponent<Torch>().GetTime(item.ItemSo.FloatValue[$"FIREMULTIPLY{_upgradeCount}"] * multiply) >= upgradeTimeMax[_upgradeCount])
                {
                    timer = upgradeTimeMax[_upgradeCount];
                }
                else
                {
                    timer += item.GetComponent<Torch>().GetTime(item.ItemSo.FloatValue[$"FIREMULTIPLY{_upgradeCount}"] * multiply);
                }
                item.GetComponent<Torch>().LightOff();
                ItemDestroy(item);
            }
        }

        private void Upgrade()
        {
            UpgradeValueSet();
            UpgradeCoroutine();
        }
        
        private void UpgradeValueSet()
        {
            _upgradeCount++;
            maxRangeTime = upgradeTimeMax[_upgradeCount];
            maxRange = upgradeMaxRange[_upgradeCount];
            _torchGiveTimeMultiply = upgradeTorchGiveTimeMultiply[_upgradeCount];
            bigTime = upgradeTimeMax[_upgradeCount] * 0.65f;
        }
        
        private void UpgradeCoroutine()
        {
            SliderCoroutine();
            ColorCorutine();
            FireCoroutine();
            RangeSet();
            LightUP();
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
