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
        private List<GameItem> _quickChoiceItems = null; //LIFO

        public QuickChoiceItems(int size)
        {
            this.size = size;
            _quickChoiceItems = CreateList(this.size);
        }

        public List<GameItem> CreateList(int size)
        {
            List<GameItem> list = new List<GameItem>();
            for(int i=0; i < size; i++)
            {
                list.Add(new GameItem());
            }

            return list;
        }

        public void InsertItem(int index, GameItem item)
        {           
            if(_quickChoiceItems != null)
            {
                if (!string.IsNullOrEmpty(_quickChoiceItems[index].Name))
                {
                    _quickChoiceItems.RemoveAt(index);
                }

                if (index < size)
                {
                    _quickChoiceItems.Insert(index, item);
                }                          
            }
        }

        public List<GameItem> getQuickChoiceItems()
        {
            return this._quickChoiceItems;
        }
    }
}
