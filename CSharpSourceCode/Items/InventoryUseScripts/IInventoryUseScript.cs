using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace TOR_Core.Items.InventoryUseScripts
{
    public interface IInventoryUseScript
    {
        void OnUse(MobileParty userParty, ItemObject item);
        void OnHourlyTick(MobileParty party);
        void OnDailyTick(MobileParty party);
    }

    public class BaseInventoryUseScript(string[] arguments) : IInventoryUseScript, IEquatable<BaseInventoryUseScript>
    {
        [SaveableField(99)]
        protected string[] _arguments = arguments;
        public virtual void OnUse(MobileParty userParty, ItemObject item) { }
        public virtual void OnHourlyTick(MobileParty party) { }
        public virtual void OnDailyTick(MobileParty party) { }
        
        public bool Equals(BaseInventoryUseScript other)
        {
            if (other == null) return false;

            if (_arguments == null && other._arguments == null && GetType() == other.GetType()) return true;
            if (_arguments == null && other._arguments != null) return false;
            if (_arguments != null && other._arguments == null) return false;

            return GetType() == other.GetType() && _arguments.Length == other._arguments.Length &&
                   _arguments.SequenceEqual(other._arguments);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseInventoryUseScript);
        }

        public override int GetHashCode()
        {
            return GetType().ToString().GetHashCode() + (_arguments != null ? string.Join(",", _arguments) : 0.ToString()).GetHashCode();
        }

        public static bool operator ==(BaseInventoryUseScript left, BaseInventoryUseScript right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(BaseInventoryUseScript left, BaseInventoryUseScript right)
        {
            return !(left == right);
        }
    }
}
