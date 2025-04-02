using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace TOR_Core.Items.InventoryUseScripts
{
    public class SkillBookScript : BaseInventoryUseScript
    {
        public SkillBookScript(string[] arguments) : base(arguments)
        {

        }

        public override void OnUse(Hero userHero, ItemObject item)
        {
            throw new NotImplementedException();
        }
    }
}
