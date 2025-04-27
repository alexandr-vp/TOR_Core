using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TOR_Core.Extensions.ExtendedInfoSystem;

namespace TOR_Core.Items
{
    [Serializable]
    [XmlInclude(typeof(ItemTrait))]
    public class ItemTrait : IEquatable<ItemTrait>
    {
        private static ItemTrait _invalid;

        [XmlAttribute]
        public string ItemTraitStringId { get; set; }
        [XmlAttribute]
        public string ItemTraitName { get; set; } = "Invalid ItemTrait";
        [XmlElement]
        public string ItemTraitDescription { get; set; } = "invalid";
        [XmlElement]
        public ResistanceTuple ResistanceTuple { get; set; }
        [XmlElement]
        public AmplifierTuple AmplifierTuple { get; set; }
        [XmlElement]
        public DamageProportionTuple AdditionalDamageTuple { get; set; }
        [XmlElement("OnWeaponHitScript")]
        public WeaponScriptTuple OnWeaponHitScript { get; set; }
        [XmlElement("OnInventoryUseScript")]
        public InventoryScriptTuple OnInventoryUseScript { get; set; }
        [XmlAttribute]
        public string ImbuedStatusEffectId { get; set; } = "none";
        [XmlAttribute]
        public float ImbuedStatusEffectChance { get; set; } = 0.25f;
        [XmlAttribute]
        public string IconName { get; set; } = "none";
        [XmlElement("WeaponParticlePreset")]
        public WeaponParticlePreset WeaponParticlePreset { get; set; }
        [XmlAttribute]
        public bool IsCraftable { get; set; } = false;

        private ItemTrait() { }

        public static List<ItemTrait> All => ItemTraitManager.Instance.GetItemTraits();

        public static ItemTrait Invalid
        {
            get
            {
                if (_invalid == null)
                {
                    _invalid = new ItemTrait
                    {
                        ItemTraitStringId = "invalid",
                        ItemTraitName = "Invalid ItemTrait",
                        ItemTraitDescription = "Invalid ItemTrait",
                        ResistanceTuple = new ResistanceTuple(),
                        AmplifierTuple = new AmplifierTuple(),
                        AdditionalDamageTuple = new DamageProportionTuple(),
                        OnWeaponHitScript = new WeaponScriptTuple(),
                        OnInventoryUseScript = new InventoryScriptTuple(),
                        ImbuedStatusEffectId = "none",
                        ImbuedStatusEffectChance = 0.25f,
                        IconName = "none",
                        WeaponParticlePreset = new WeaponParticlePreset(),
                        IsCraftable = false
                    };
                }
                return _invalid;
            }
        }

        public bool Equals(ItemTrait other)
        {
            if (other == null) return false;

            return ItemTraitStringId == other.ItemTraitStringId &&
                   ItemTraitName == other.ItemTraitName &&
                   ItemTraitDescription == other.ItemTraitDescription &&
                   Equals(ResistanceTuple, other.ResistanceTuple) &&
                   Equals(AmplifierTuple, other.AmplifierTuple) &&
                   Equals(AdditionalDamageTuple, other.AdditionalDamageTuple) &&
                   Equals(OnWeaponHitScript, other.OnWeaponHitScript) &&
                   Equals(OnInventoryUseScript, other.OnInventoryUseScript) &&
                   ImbuedStatusEffectId == other.ImbuedStatusEffectId &&
                   ImbuedStatusEffectChance == other.ImbuedStatusEffectChance &&
                   IconName == other.IconName &&
                   Equals(WeaponParticlePreset, other.WeaponParticlePreset) &&
                   IsCraftable == other.IsCraftable;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ItemTrait);
        }

        public override int GetHashCode()
        {
            return (ItemTraitStringId, ItemTraitName, ItemTraitDescription, ResistanceTuple, AmplifierTuple, AdditionalDamageTuple, OnWeaponHitScript, OnInventoryUseScript, ImbuedStatusEffectId, ImbuedStatusEffectChance, IconName, WeaponParticlePreset, IsCraftable).GetHashCode();
        }

        public static bool operator ==(ItemTrait left, ItemTrait right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(ItemTrait left, ItemTrait right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public class WeaponParticlePreset : IEquatable<WeaponParticlePreset>
    {
        [XmlAttribute]
        public string ParticlePrefab { get; set; } = "invalid";
        [XmlAttribute]
        public bool IsUniqueSingleCopy { get; set; } = false;

        public WeaponParticlePreset() { }

        public bool Equals(WeaponParticlePreset other)
        {
            if (other == null) return false;
            return ParticlePrefab == other.ParticlePrefab &&
                   IsUniqueSingleCopy == other.IsUniqueSingleCopy;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as WeaponParticlePreset);
        }

        public override int GetHashCode()
        {
            return (ParticlePrefab, IsUniqueSingleCopy).GetHashCode();
        }

        public static bool operator ==(WeaponParticlePreset left, WeaponParticlePreset right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(WeaponParticlePreset left, WeaponParticlePreset right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public class WeaponScriptTuple : IEquatable<WeaponScriptTuple>
    {
        [XmlAttribute]
        public string WeaponScriptName { get; set; } = "invalid";
        [XmlArray("WeaponScriptArguments")]
        [XmlArrayItem("WeaponScriptArgument")]
        public List<string> WeaponScriptArguments { get; set; } = [];

        public WeaponScriptTuple() { }

        public bool Equals(WeaponScriptTuple other)
        {
            if (other == null) return false;
            return WeaponScriptName == other.WeaponScriptName &&
                   WeaponScriptArguments.SequenceEqual(other.WeaponScriptArguments);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as WeaponScriptTuple);
        }

        public override int GetHashCode()
        {
            return (WeaponScriptName, WeaponScriptArguments).GetHashCode();
        }

        public static bool operator ==(WeaponScriptTuple left, WeaponScriptTuple right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(WeaponScriptTuple left, WeaponScriptTuple right)
        {
            return !(left == right);
        }
    }

    [Serializable]
    public class InventoryScriptTuple : IEquatable<InventoryScriptTuple>
    {
        [XmlAttribute]
        public string InventoryScriptName { get; set; } = "invalid";
        [XmlArray("InventoryScriptArguments")]
        [XmlArrayItem("InventoryScriptArgument")]
        public List<string> InventoryScriptArguments { get; set; } = [];

        public InventoryScriptTuple() { }

        public bool Equals(InventoryScriptTuple other)
        {
            if (other == null) return false;
            return InventoryScriptName == other.InventoryScriptName &&
                   InventoryScriptArguments.SequenceEqual(other.InventoryScriptArguments);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as InventoryScriptTuple);
        }

        public override int GetHashCode()
        {
            return (InventoryScriptName, InventoryScriptArguments).GetHashCode();
        }

        public static bool operator ==(InventoryScriptTuple left, InventoryScriptTuple right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(InventoryScriptTuple left, InventoryScriptTuple right)
        {
            return !(left == right);
        }
    }
}
