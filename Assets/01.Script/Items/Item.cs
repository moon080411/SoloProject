using _01.Script.Manager;
using _01.Script.SO.Item;
using Plugins.SerializedFinder.RunTime.Finder;
using Unity.VisualScripting;
using UnityEngine;

namespace _01.Script.Items
{
    public class Item : MonoBehaviour
    {
        [field:SerializeField] public ItemSO ItemSo { get; private set; }
        [SerializeField] private ScriptFinderSO uiManagerFinder;
        [SerializeField] private ScriptFinderSO resourceManagerFinder;
        [SerializeField] private int amount = 1;

        private void OnMouseEnter()
        {
            //여기에 빛나게 되는 함수를 넣으면 됨
            //uiManagerFinder.GetTarget<UIManager>().ShowItemTooltip(this);
        }

        private void OnMouseDown()
        {
            resourceManagerFinder.GetTarget<ResourceManager>().AddItemToInventory(this);
        }

        private void OnMouseExit()
        {
            //uiManagerFinder.GetTarget<UIManager>().HideItemTooltip(this);
        }

        public void GetOutOfInventory(Transform entity)
        {
            transform.position = entity.position + Vector3.Scale(transform.localScale * 0.75f , entity.forward);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.75f , transform.position.z);
            //transform.rotation = entity.rotation;
        }

        public void SetRigidbody()
        {
            Rigidbody rb = transform.AddComponent<Rigidbody>();
            rb.mass = ItemSo.Mass;
            rb.freezeRotation = true;
            rb.linearDamping = 15;
            rb.angularDamping = 15;
        }
    }
}
