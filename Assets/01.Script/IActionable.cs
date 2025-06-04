using _01.Script.Items;
using _01.Script.SO.Item;

namespace _01.Script
{
    public interface IActionable
    {
        public void Action();
        public void ItemAction(Item item);
    }
}