using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace TOR_Core.Items.WeaponHitScripts
{
    public interface IWeaponHitScript
    {
        void OnHit(Agent attackingAgent, Agent attackedAgent, int inflictedDamge, MissionWeapon missionWeapon);
    }
}
