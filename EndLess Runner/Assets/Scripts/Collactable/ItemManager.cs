using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Singleton;

namespace Items
{
    public class ItemManager : Singleton<ItemManager>
    {
        public List<ItemSetup> itemSetups;

        public ItemSetup GetItemSetupByType(ItemType type)
        {
            return itemSetups.Find(setup => setup.itemType == type);
        }
    }

    [System.Serializable]
    public class ItemSetup
    {
        public ItemType itemType;
        public SoInt soInt;
        public int currentValue;
    }
}
