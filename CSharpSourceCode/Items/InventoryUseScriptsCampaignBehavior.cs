using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TOR_Core.Items.InventoryUseScripts;
using TOR_Core.Utilities;

namespace TOR_Core.Items
{
    public class InventoryUseScriptsCampaignBehavior : CampaignBehaviorBase
    {
        private Dictionary<string, List<BaseInventoryUseScript>> _activeScripts = [];
        private Dictionary<string, List<BaseInventoryUseScript>> _scriptsToRemove = [];

        public static InventoryUseScriptsCampaignBehavior Instance => Campaign.Current.GetCampaignBehavior<InventoryUseScriptsCampaignBehavior>();

        public override void RegisterEvents()
        {
            CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, OnHourlyTick);
            CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, OnDailyTick);
        }

        private void OnDailyTick(MobileParty party)
        {
            if (_activeScripts.TryGetValue(party.StringId, out var scripts))
            {
                foreach (var script in scripts)
                {
                    script.OnDailyTick(party);
                }
            }
        }

        private void OnHourlyTick(MobileParty party)
        {
            if (_activeScripts.TryGetValue(party.StringId, out var scripts))
            {
                foreach (var script in scripts)
                {
                    script.OnHourlyTick(party);
                }
            }
            if (_scriptsToRemove.TryGetValue(party.StringId, out var removeScripts))
            {
                foreach (var script in removeScripts)
                {
                    _activeScripts[party.StringId].Remove(script);
                }
                _scriptsToRemove[party.StringId].Clear();
            }
        }

        public void AddScriptToParty(MobileParty party, BaseInventoryUseScript script)
        {
            if (!_activeScripts.ContainsKey(party.StringId))
            {
                _activeScripts[party.StringId] = [];
            }
            if (_activeScripts[party.StringId].Contains(script))
            {
                TORCommon.Say($"Script {script} already exists in party {party.StringId}. Not adding again.");
                return;
            }
            _activeScripts[party.StringId].Add(script);
        }

        public void RemoveScriptFromParty(MobileParty party, BaseInventoryUseScript script)
        {
            if (!_scriptsToRemove.ContainsKey(party.StringId))
            {
                _scriptsToRemove[party.StringId] = [];
            }
            _scriptsToRemove[party.StringId].Add(script);
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("_activeScripts", ref _activeScripts);
        }
    }
}
