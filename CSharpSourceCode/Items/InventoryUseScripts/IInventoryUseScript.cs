using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace TOR_Core.Items.InventoryUseScripts
{
    public interface IInventoryUseScript
    {
        void OnUse(Hero userHero, ItemObject item);
    }

    public abstract class BaseInventoryUseScript : IInventoryUseScript
    {
        public BaseInventoryUseScript(string[] arguments) { }

        public abstract void OnUse(Hero userHero, ItemObject item);
    }
}
