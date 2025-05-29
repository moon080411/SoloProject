using System;
using Plugins.SerializedFinder.RunTime.Finder;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01.Script.Items
{
    public abstract class Item : MonoBehaviour
    {
        [field:SerializeField] public string ItemName { get; private set; }
        [SerializeField] public ScriptFinderSO uiManagerFinder;
        [SerializeField] private int amount = 1;

        private void OnMouseDown()
        {
            //여기에 빛나게 되는 함수를 넣으면 됨
        }
    }
}
