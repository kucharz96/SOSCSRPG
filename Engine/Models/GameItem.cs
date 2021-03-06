﻿using Engine.Actions;
using Newtonsoft.Json;

namespace Engine.Models
{
    public class GameItem
    {
        public enum ItemCategory
        {
            Miscellaneous,
            Weapon,
            Consumable,
            None
        }

        [JsonIgnore]
        public ItemCategory Category { get; }
        public int ItemTypeID { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public int Price { get; }
        [JsonIgnore]
        public bool IsUnique { get; }


        [JsonIgnore]
        public IAction Action { get; set; }

        [JsonIgnore]
        public string ImagePath { get; set; }
        


        public bool IsInQuickChoose { get; set; }
        public GameItem() {
            this.Category = ItemCategory.None;
        }

        public GameItem(ItemCategory category, int itemTypeID, string name, int price,
                        bool isUnique = false, IAction action = null, string imagePath = null)
        {
            Category = category;
            ItemTypeID = itemTypeID;
            Name = name;
            Price = price;
            IsUnique = isUnique;
            Action = action;
            ImagePath = imagePath;
        }

        public void PerformAction(LivingEntity actor, LivingEntity target)
        {
            Action?.Execute(actor, target);
        }

        public GameItem Clone()
        {
            return new GameItem(Category, ItemTypeID, Name, Price, IsUnique, Action,ImagePath);
        }
    }
}