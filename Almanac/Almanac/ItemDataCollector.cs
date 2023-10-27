﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Almanac.Almanac;

public static class ItemDataCollector
{
    public static class IndexedItemData
    {
        public static List<ItemDrop> GetNoneItems()
        {
            return ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.None, "");
        }

        public static List<ItemDrop> GetFishes()
        {
            return ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Fish, "");
        }
        public static List<ItemDrop> GetConsumables()
        {
            return ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Consumable, "");
        }
        
        public static List<ItemDrop> GetAmmunition()
        {
            List<ItemDrop> ammo = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Ammo, "");
            List<ItemDrop> ammoNonEquip = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.AmmoNonEquipable, "");
            
            List<ItemDrop> ammunition = new List<ItemDrop>();
            ammunition.AddRange(ammo);
            ammunition.AddRange(ammoNonEquip);

            return GetValidItemDropList(ammunition);
        }
        public static List<ItemDrop> GetMaterials()
        {
            List<ItemDrop> materials = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Material, "");
            materials.AddRange(GetFilteredMisc(filterOption.toMaterials));

            return materials;
        }

        public static List<ItemDrop> GetWeapons()
        {
            List<ItemDrop> oneHanded = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.OneHandedWeapon, "");
            List<ItemDrop> bow = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Bow, "");
            List<ItemDrop> shield = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Shield, "");
            List<ItemDrop> hands = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Hands, "");
            List<ItemDrop> twoHanded = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.TwoHandedWeapon, "");
            List<ItemDrop> torch = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Torch, "");
            List<ItemDrop> tool = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Tool, "");
            List<ItemDrop> attachAtgeir = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Attach_Atgeir, "");
            List<ItemDrop> twoHandedLeft = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.TwoHandedWeaponLeft, "");
            
            List<ItemDrop> weaponList = new List<ItemDrop>();
            weaponList.AddRange(oneHanded);
            weaponList.AddRange(bow);
            weaponList.AddRange(shield);
            weaponList.AddRange(twoHanded);
            weaponList.AddRange(hands);
            weaponList.AddRange(torch);
            weaponList.AddRange(tool);
            weaponList.AddRange(attachAtgeir);
            weaponList.AddRange(twoHandedLeft);

            return GetValidItemDropList(weaponList);
        }
        public static List<ItemDrop> GetEquipments()
        {
            List<ItemDrop> helmet = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Helmet, "");
            List<ItemDrop> chest = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Chest, "");
            List<ItemDrop> legs = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Legs, "");
            List<ItemDrop> shoulder = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Shoulder, "");
            List<ItemDrop> utility = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Utility, "");
            List<ItemDrop> customization = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Customization, "");
            
            List<ItemDrop> gearList = new List<ItemDrop>();
            gearList.AddRange(helmet);
            gearList.AddRange(chest);
            gearList.AddRange(legs);
            gearList.AddRange(shoulder);
            gearList.AddRange(utility);
            gearList.AddRange(customization);
            gearList.AddRange(GetFilteredMisc(filterOption.toMisc));

            return new List<ItemDrop>(
                GetValidItemDropList(gearList).OrderBy(
                    name => Localization.instance.Localize(name.m_itemData.m_shared.m_name)));
        }

        enum filterOption
        {
            toMisc,
            toMaterials
        }
        private static List<ItemDrop> GetFilteredMisc(filterOption option)
        {
            List<ItemDrop> misc = ObjectDB.instance.GetAllItems(ItemDrop.ItemData.ItemType.Misc, "");

            List<ItemDrop> filteredMisc = new List<ItemDrop>();
            List<ItemDrop> MiscToMaterials = new List<ItemDrop>();
            foreach (ItemDrop drop in misc)
            {
                List<string> toMaterialsMap = new List<string>()
                {
                    "DragonEgg",
                    "ChickenEgg",
                    "Turnip"
                };
                if (toMaterialsMap.Contains(drop.name))
                {
                    MiscToMaterials.Add(drop);
                }
                else
                {
                    filteredMisc.Add(drop);
                }
            }
            
            return option == filterOption.toMisc ? filteredMisc : MiscToMaterials;
        }
        private static List<ItemDrop> GetValidItemDropList(List<ItemDrop> list)
        {
            List<ItemDrop> output = new List<ItemDrop>();
            foreach (var itemDrop in list)
            {
                try
                {
                    ItemDrop data = itemDrop;
                    Sprite sprite = data.m_itemData.GetIcon();
                    if (!sprite) continue;
                    output.Add(data);
                }
                catch (IndexOutOfRangeException)
                {
                    // AlmanacPlugin.AlmanacLogger.Log(LogLevel.Warning, $"invalid item drop data: {i}");
                }
            }
            return output;
        }
    }
}