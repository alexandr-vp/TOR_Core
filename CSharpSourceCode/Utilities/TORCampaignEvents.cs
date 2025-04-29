using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TOR_Core.CampaignMechanics.Religion;

namespace TOR_Core.Utilities
{
    public class TORCampaignEvents
    {
        public static TORCampaignEvents Instance;
        public event EventHandler<DevotionLevelChangedEventArgs> DevotionLevelChanged;
        public event EventHandler<ChaosUprisingStartedEventArgs> ChaosUprisingStarted;
        public event EventHandler<ItemDuplicatedEventArgs> ItemDuplicated;

        public TORCampaignEvents()
        {
            Instance = this;
        }

        public void OnDevotionLevelChanged(Hero hero, ReligionObject religion, DevotionLevel oldDevotionLevel, DevotionLevel newDevotionLevel)
        {
            var args = new DevotionLevelChangedEventArgs(hero, religion, oldDevotionLevel, newDevotionLevel);
            var devotionEvent = DevotionLevelChanged;
            if(devotionEvent != null)
            {
                devotionEvent(this, args);
            }
        }

        public void OnChaosUprisingStarted(Settlement settlement)
        {
            var args = new ChaosUprisingStartedEventArgs(settlement);
            var uprisingEvent = ChaosUprisingStarted;
            if(uprisingEvent != null)
            {
                uprisingEvent(this, args);
            }
        }

        public void OnItemDuplicated(ItemObject newItem, ItemObject oldItem, List<string> traits)
        {
            var args = new ItemDuplicatedEventArgs(newItem, oldItem, traits);
            var itemDuplicatedEvent = ItemDuplicated;
            if (itemDuplicatedEvent != null)
            {
                itemDuplicatedEvent(this, args);
            }
        }
    }

    public class DevotionLevelChangedEventArgs : EventArgs
    {
        public DevotionLevelChangedEventArgs(Hero hero, ReligionObject religion, DevotionLevel oldDevotionLevel, DevotionLevel newDevotionLevel)
        {
            Hero = hero;
            Religion = religion;
            OldDevotionLevel = oldDevotionLevel;
            NewDevotionLevel = newDevotionLevel;
        }

        public Hero Hero { get; set; }
        public ReligionObject Religion { get; set; }
        public DevotionLevel OldDevotionLevel { get; set; }
        public DevotionLevel NewDevotionLevel { get; set; }
    }

    public class ChaosUprisingStartedEventArgs : EventArgs
    {
        public ChaosUprisingStartedEventArgs(Settlement settlement)
        {
            Settlement = settlement;
        }

        public Settlement Settlement { get; set; }
    }

    public class ItemDuplicatedEventArgs : EventArgs
    {
        public ItemDuplicatedEventArgs(ItemObject newItem, ItemObject oldItem, List<string> traits)
        {
            NewItem = newItem;
            OldItem = oldItem;
            Traits = traits;
        }

        public ItemObject NewItem { get; set; }
        public ItemObject OldItem { get; set; }
        public List<string> Traits { get; set; }
    }
}
