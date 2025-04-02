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
        [XmlAttribute]
        public string ItemTraitName { get; set; }
        [XmlElement]
        public string ItemTraitDescription { get; set; }
        [XmlElement]
        public ResistanceTuple ResistanceTuple { get; set; }
        [XmlElement]
        public AmplifierTuple AmplifierTuple { get; set; }
        [XmlElement]
        public DamageProportionTuple AdditionalDamageTuple { get; set; }
        [XmlElement]
        public ScriptTuple OnWeaponHitScript { get; set; }
        [XmlElement]
        public ScriptTuple OnInventoryUseScript { get; set; }
        [XmlAttribute]
        public string ImbuedStatusEffectId { get; set; } = "none";
        [XmlAttribute]
        public float ImbuedStatusEffectChance { get; set; } = 0.25f;
        [XmlAttribute]
        public string IconName { get; set; } = "none";
        [XmlElement]
        public WeaponParticlePreset WeaponParticlePreset { get; set; }

        private ItemTrait() { }

        public static List<ItemTrait> All => ItemTraitManager.Instance.GetItemTraits();

        public bool Equals(ItemTrait other)
        {
            if (other == null) return false;

            return ItemTraitName == other.ItemTraitName &&
                   ItemTraitDescription == other.ItemTraitDescription &&
                   Equals(ResistanceTuple, other.ResistanceTuple) &&
                   Equals(AmplifierTuple, other.AmplifierTuple) &&
                   Equals(AdditionalDamageTuple, other.AdditionalDamageTuple) &&
                   Equals(OnWeaponHitScript, other.OnWeaponHitScript) &&
                   Equals(OnInventoryUseScript, other.OnInventoryUseScript) &&
                   ImbuedStatusEffectId == other.ImbuedStatusEffectId &&
                   ImbuedStatusEffectChance == other.ImbuedStatusEffectChance &&
                   IconName == other.IconName &&
                   Equals(WeaponParticlePreset, other.WeaponParticlePreset);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ItemTrait);
        }

        public override int GetHashCode()
        {
            return (ItemTraitName, ItemTraitDescription, ResistanceTuple, AmplifierTuple, AdditionalDamageTuple, OnWeaponHitScript, OnInventoryUseScript, ImbuedStatusEffectId, ImbuedStatusEffectChance, IconName, WeaponParticlePreset).GetHashCode();
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
    public class ScriptTuple : IEquatable<ScriptTuple>
    {
        [XmlAttribute]
        public string ScriptName { get; set; }
        [XmlElement("Arguments")]
        public List<string> ScriptArguments { get; set; }

        public bool Equals(ScriptTuple other)
        {
            if (other == null) return false;
            return ScriptName == other.ScriptName &&
                   ScriptArguments.SequenceEqual(other.ScriptArguments);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ScriptTuple);
        }

        public override int GetHashCode()
        {
            return (ScriptName, ScriptArguments).GetHashCode();
        }

        public static bool operator ==(ScriptTuple left, ScriptTuple right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(ScriptTuple left, ScriptTuple right)
        {
            return !(left == right);
        }
    }
    
    public class SkillTuple
    {
        public bool IsAbility { get; set; } = false;
        public string SkillId { get; set; }
        public float SkillExp { get; set; } = 0;
        public float LearningTime { get; set; } = 1;
    }
}
