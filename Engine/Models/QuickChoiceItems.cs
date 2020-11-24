using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class QuickChoiceItems
    {
        private int size;
         List<GameItem> _quickChoiceItems = new List<GameItem>(); //LIFO
        public void InsertItem(GameItem item)
        {
            _quickChoiceItems.Add(item);
            item.IsInQuickChoose = true;

        }

        public void RemoveItem(GameItem item)
        {
            _quickChoiceItems.Remove(item);
            item.IsInQuickChoose = false;

        }

        public List<GameItem> getQuickChoiceItems()
        {
            return this._quickChoiceItems;
        }
    }
}
