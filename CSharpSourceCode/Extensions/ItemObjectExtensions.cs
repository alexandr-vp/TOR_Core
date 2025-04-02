using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library.NewsManager;
using TaleWorlds.MountAndBlade;
using TOR_Core.BattleMechanics.DamageSystem;
using TOR_Core.Items;

namespace TOR_Core.Extensions
{
    public static class ItemObjectExtensions
    {
        public static List<ItemTrait> GetTraits(this ItemObject item)
        {
            var result = ExtendedItemObjectManager.GetAdditionalProperties(item.StringId);
            if (result == null) result = ExtendedItemObjectProperties.CreateDefault(item.StringId);
            return result.ItemTraits.ToList();
        }

        public static List<ItemTrait> GetTraits(this ItemObject item, Agent agent)
        {
            var result = item.GetTraits();
            var comp = agent.GetComponent<ItemTraitAgentComponent>();
            if (comp != null)
            {
                if (result == null) result = new List<ItemTrait>();
                result.AddRange(comp.GetDynamicTraits(item));
            }

            return result;
        }

        public static ExtendedItemObjectProperties GetTorSpecificData(this ItemObject item)
        {
            var result = ExtendedItemObjectManager.GetAdditionalProperties(item.StringId);
            if (result == null) result = ExtendedItemObjectProperties.CreateDefault(item.StringId);
            return result;
        }

        public static ExtendedItemObjectProperties GetTorSpecificData(this ItemObject item, Agent agent)
        {
            var result = item.GetTorSpecificData();
            if (result == null) result = ExtendedItemObjectProperties.CreateDefault(item.StringId);
            var comp = agent.GetComponent<ItemTraitAgentComponent>();
            if (comp != null)
            {
                result.ItemTraits.AddRange(comp.GetDynamicTraits(item));
            }

            return result;
        }

        public static bool HasAnyTrait(this ItemObject item)
        {
            if (item.GetTraits() != null)
            {
                return item.GetTraits().Count > 0;
            }
            else return false;
        }

        public static bool HasAnyTrait(this ItemObject item, Agent agent)
        {
            if (item.GetTraits(agent) != null)
            {
                return item.GetTraits(agent).Count > 0;
            }
            else return false;
        }

        public static bool IsTorItem(this ItemObject item)
        {
            return item.StringId.StartsWith("tor_");
        }

        public static bool IsMagicalItem(this ItemObject item)
        {
            var info = item.GetTorSpecificData();
            if(info != null)
            {
                return info.DamageProportions.Any(x => x.DamageType != DamageType.Physical) || info.ItemTraits.Count > 0;
            }
            return false;
        }

        public static bool IsExplosiveAmmunition(this ItemObject itemObject)
        {
            return IsAmmunitionItem(itemObject) && itemObject.StringId.Contains("grenade") ||
                itemObject.WeaponComponent?.PrimaryWeapon.WeaponClass == WeaponClass.Boulder;
        }

        public static bool IsSmallArmsAmmunition(this ItemObject itemObject)
        {
            return itemObject.WeaponComponent.PrimaryWeapon.IsSmallArmsAmmunition() && !itemObject.IsExplosiveAmmunition();
        }

        private static bool IsSmallArmsAmmunition(this WeaponComponentData weapon)
        {
            bool result = false;
            switch (weapon.WeaponClass)
            {
                case WeaponClass.Arrow:
                case WeaponClass.Bolt:
                case WeaponClass.Cartridge:
                    result = true;
                    break;
                default:
                    break;
            }
            return result;
        }

        public static bool IsMagicalStaff(this ItemObject itemObject)
        {
            if (itemObject == null) return false;
            return itemObject.StringId.Contains("staff") && itemObject.WeaponComponent.PrimaryWeapon.IsMeleeWeapon;
        }

        public static bool IsGunPowderWeapon(this ItemObject itemObject)
        {
            return (bool)(itemObject.WeaponComponent?.PrimaryWeapon?.IsGunPowderWeapon());
        }

        public static bool IsGunPowderWeapon(this WeaponComponentData weapon)
        {
            if (weapon == null || !weapon.IsRangedWeapon) return false;
            return weapon.WeaponClass == WeaponClass.Cartridge || weapon.AmmoClass == WeaponClass.Cartridge;
        }

        /// <summary>
        /// Checks if the current weapon is shooting scatter shots or grenades, or is scatter/grenade ammunition
        /// </summary>
        /// <param name="itemObject"></param>
        /// <returns></returns>
        public static bool IsSpecialAmmunitionItem(this ItemObject itemObject)
        {
            if (!IsAmmunitionItem(itemObject))
                return false;

            if (itemObject.ToString().Contains("blunderbuss"))
            {
                return true;
            }

            return itemObject.StringId.Contains("grenade") || itemObject.StringId.Contains("scatter");
        }
        
        public static bool IsFlameThrowerItem(this ItemObject itemObject)
        {
            if(!IsAmmunitionItem(itemObject)) return false;
            
            return itemObject.StringId.Contains("canister") || itemObject.StringId.Contains("drakegun");
        }

        public static bool IsAmmunitionItem(this ItemObject itemObject)
        {
            if (itemObject?.WeaponComponent?.PrimaryWeapon == null) return false;

            return itemObject.WeaponComponent.PrimaryWeapon.IsRangedWeapon ||
                   itemObject.WeaponComponent.PrimaryWeapon.IsAmmo;
        }

        public static void CopyPropertiesFrom(this ItemObject item, ItemObject other)
        {
            AccessTools.Property(typeof(ItemObject), "Culture").SetValue(item, other.Culture);
            AccessTools.Property(typeof(ItemObject), "ItemComponent").SetValue(item, other.ItemComponent);
            AccessTools.Property(typeof(ItemObject), "MultiMeshName").SetValue(item, other.MultiMeshName);
            AccessTools.Property(typeof(ItemObject), "HolsterMeshName").SetValue(item, other.HolsterMeshName);
            AccessTools.Property(typeof(ItemObject), "HolsterWithWeaponMeshName").SetValue(item, other.HolsterWithWeaponMeshName);
            AccessTools.Property(typeof(ItemObject), "ItemHolsters").SetValue(item, other.ItemHolsters);
            AccessTools.Property(typeof(ItemObject), "HolsterPositionShift").SetValue(item, other.HolsterPositionShift);
            AccessTools.Property(typeof(ItemObject), "FlyingMeshName").SetValue(item, other.FlyingMeshName);
            AccessTools.Property(typeof(ItemObject), "BodyName").SetValue(item, other.BodyName);
            AccessTools.Property(typeof(ItemObject), "HolsterBodyName").SetValue(item, other.HolsterBodyName);
            AccessTools.Property(typeof(ItemObject), "CollisionBodyName").SetValue(item, other.CollisionBodyName);
            AccessTools.Property(typeof(ItemObject), "RecalculateBody").SetValue(item, other.RecalculateBody);
            AccessTools.Property(typeof(ItemObject), "PrefabName").SetValue(item, other.PrefabName);
            AccessTools.Property(typeof(ItemObject), "Name").SetValue(item, other.Name);
            AccessTools.Property(typeof(ItemObject), "ItemFlags").SetValue(item, other.ItemFlags);
            AccessTools.Property(typeof(ItemObject), "Value").SetValue(item, other.Value);
            AccessTools.Property(typeof(ItemObject), "Weight").SetValue(item, other.Weight);
            AccessTools.Property(typeof(ItemObject), "Difficulty").SetValue(item, other.Difficulty);
            AccessTools.Property(typeof(ItemObject), "ArmBandMeshName").SetValue(item, other.ArmBandMeshName);
            AccessTools.Property(typeof(ItemObject), "IsFood").SetValue(item, other.IsFood);
            AccessTools.Property(typeof(ItemObject), "ScaleFactor").SetValue(item, other.ScaleFactor);
            AccessTools.Property(typeof(ItemObject), "IsUniqueItem").SetValue(item, other.IsUniqueItem);
            item.Type = other.Type;
        }
    }
}