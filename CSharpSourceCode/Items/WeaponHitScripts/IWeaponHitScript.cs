using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem;
using TOR_Core.Items.InventoryUseScripts;

namespace TOR_Core.Items.WeaponHitScripts
{
    public interface IWeaponHitScript
    {
        void OnHit(Agent attackingAgent, Agent attackedAgent, int inflictedDamge, MissionWeapon missionWeapon);
    }

    public class BaseWeaponHitScript(string[] arguments) : IWeaponHitScript
    {
        protected string[] _arguments = arguments;

        public virtual void OnHit(Agent attackingAgent, Agent attackedAgent, int inflictedDamge, MissionWeapon missionWeapon) { }
    }
}
