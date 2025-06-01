using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TOR_Core.CampaignMechanics
{
    public class TORPartyUpgraderCampaignBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(MapEventEnded));
            CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(DailyTickParty));
        }

        private void MapEventEnded(MapEvent mapEvent)
        {
            foreach (PartyBase party in mapEvent.InvolvedParties)
            {
                UpgradeReadyTroops(party);
            }
        }

        private void DailyTickParty(MobileParty party)
        {
            if (party.MapEvent == null)
            {
                UpgradeReadyTroops(party.Party);
            }
        }

        public void UpgradeReadyTroops(PartyBase party)
        {
            if (party != PartyBase.MainParty && party.IsActive)
            {
                TroopRoster memberRoster = party.MemberRoster;
                PartyTroopUpgradeModel partyTroopUpgradeModel = Campaign.Current.Models.PartyTroopUpgradeModel;
                for (int i = 0; i < memberRoster.Count; i++)
                {
                    TroopRosterElement elementCopyAtIndex = memberRoster.GetElementCopyAtIndex(i);
                    if (partyTroopUpgradeModel.IsTroopUpgradeable(party, elementCopyAtIndex.Character))
                    {
                        List<TORTroopUpgradeArgs> possibleUpgradeTargets = GetPossibleUpgradeTargets(party, elementCopyAtIndex);
                        if (possibleUpgradeTargets.Count > 0)
                        {
                            TORTroopUpgradeArgs upgradeArgs = SelectPossibleUpgrade(possibleUpgradeTargets);
                            /* this is removed atm due to bottlenecking troops at a tier if their upgrade target exceeds some ratio; because it doesn't account for troops of further upgrades, it might stop a t1->t2 upgrade despite t3+ having no troops because the t2 troops are too slow to upgrade
                            if (party.IsMobile && party.MobileParty.IsLordParty)
                            {
                                if (party.LeaderHero == null)
                                {
                                    break;
                                }
                                if (memberRoster.Contains(upgradeArgs.UpgradeTarget))
                                {
                                    var partyTemplate = party.LeaderHero.Clan.DefaultPartyTemplate; // either takes clan, or if not take the culture one
                                    
                                    float currentTotalTroopCount = memberRoster.TotalManCount;

                                    var ratio = 0f;

                                    if (partyTemplate.Stacks.Any(x => x.Character == upgradeArgs.UpgradeTarget))   
                                    {
                                        float maximumTotalTroopCountOfTemplate = 0f;

                                        PartyTemplateStack targetStack = new PartyTemplateStack();
                                        foreach (var stack in partyTemplate.Stacks)
                                        {
                                            if (stack.Character == upgradeArgs.UpgradeTarget)
                                            {
                                                targetStack = stack;
                                            }

                                            maximumTotalTroopCountOfTemplate+=stack.MaxValue;
                                        }

                                        if (maximumTotalTroopCountOfTemplate == 0)
                                        {
                                            continue;
                                        }
                                    
                                        ratio = targetStack.MaxValue / maximumTotalTroopCountOfTemplate;

                                        var potentialCountOfTemplate =  ratio * currentTotalTroopCount;

                                        if (!(memberRoster.GetElementCopyAtIndex(i).Number < potentialCountOfTemplate))
                                        {
                                            continue;
                                        }   
                                    }
                                    else //case ror are recruited or anything outside the regular roster
                                    {
                                        var count = memberRoster.GetTroopCount(upgradeArgs.UpgradeTarget);

                                        ratio = 0.1f;
                                        if (count > ratio * currentTotalTroopCount)
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            */
                            UpgradeTroop(party, i, upgradeArgs);
                           
                        }
                    }
                }
            }
        }

        private List<TORTroopUpgradeArgs> GetPossibleUpgradeTargets(PartyBase party, TroopRosterElement rosterElement)
        {
            PartyWageModel partyWageModel = Campaign.Current.Models.PartyWageModel;
            List<TORTroopUpgradeArgs> list = [];
            CharacterObject troopCharacter = rosterElement.Character;
            int numPossibleTroopsToUpgrade = rosterElement.Number - rosterElement.WoundedNumber;
            if (numPossibleTroopsToUpgrade > 0)
            {
                PartyTroopUpgradeModel partyTroopUpgradeModel = Campaign.Current.Models.PartyTroopUpgradeModel;
                for(int i = 0; i < troopCharacter.UpgradeTargets.Length; i++)
                {
                    CharacterObject upgradeTargetCharacter = troopCharacter.UpgradeTargets[i];
                    int upgradeXpCost = troopCharacter.GetUpgradeXpCost(party, i);
                    if (upgradeXpCost > 0) { numPossibleTroopsToUpgrade = MathF.Min(numPossibleTroopsToUpgrade, rosterElement.Xp / upgradeXpCost); }
                    //if(upgradeXpCost <= 0 || numPossibleTroopsToUpgrade * rosterElement.Xp < upgradeXpCost)

                    int upgradeGoldCost = troopCharacter.GetUpgradeGoldCost(party, i);
                    if (upgradeGoldCost > 0 && party.LeaderHero != null && numPossibleTroopsToUpgrade * upgradeGoldCost > party.LeaderHero.Gold)
                    {
                        numPossibleTroopsToUpgrade = party.LeaderHero.Gold / upgradeGoldCost;
                        if (numPossibleTroopsToUpgrade <= 1) { continue; }
                    }

                    //MaxWage for "unlimited" wages is 10k, but what does that matter? why would native call mobParty.HasLimitedWage which would ignore the conditional and allow an ai party to surpass the 10k limit anyways?
                    if (upgradeTargetCharacter.Tier > troopCharacter.Tier &&
                        party.MobileParty.HasLimitedWage() &&
                        party.MobileParty.CanPayMoreWage() &&
                        party.MobileParty.TotalWage + numPossibleTroopsToUpgrade * (partyWageModel.GetCharacterWage(upgradeTargetCharacter) - partyWageModel.GetCharacterWage(troopCharacter)) > party.MobileParty.PaymentLimit)
                    {
                        numPossibleTroopsToUpgrade =  (party.MobileParty.PaymentLimit - party.MobileParty.TotalWage) / (partyWageModel.GetCharacterWage(upgradeTargetCharacter) - partyWageModel.GetCharacterWage(troopCharacter));
                        if (numPossibleTroopsToUpgrade <= 1) { continue; }
                    }
                    
                    if ((!party.Culture.IsBandit || upgradeTargetCharacter.Culture.IsBandit) && (troopCharacter.Occupation != Occupation.Bandit || partyTroopUpgradeModel.CanPartyUpgradeTroopToTarget(party, troopCharacter, upgradeTargetCharacter)))
                    {
                        float upgradeChanceForTroopUpgrade = Campaign.Current.Models.PartyTroopUpgradeModel.GetUpgradeChanceForTroopUpgrade(party, troopCharacter, i);
                        list.Add(new TORTroopUpgradeArgs(troopCharacter, upgradeTargetCharacter, numPossibleTroopsToUpgrade, upgradeGoldCost, upgradeXpCost, upgradeChanceForTroopUpgrade));
                    }
                }
            }
            return list;
        }

        private TORTroopUpgradeArgs SelectPossibleUpgrade(List<TORTroopUpgradeArgs> possibleUpgrades)
        {
            TORTroopUpgradeArgs result = possibleUpgrades[0];
            if (possibleUpgrades.Count > 1)
            {
                float num = 0f;
                foreach (TORTroopUpgradeArgs troopUpgradeArgs in possibleUpgrades)
                {
                    num += troopUpgradeArgs.UpgradeChance;
                }
                float num2 = num * MBRandom.RandomFloat;
                foreach (TORTroopUpgradeArgs troopUpgradeArgs2 in possibleUpgrades)
                {
                    num2 -= troopUpgradeArgs2.UpgradeChance;
                    if (num2 <= 0f)
                    {
                        result = troopUpgradeArgs2;
                        break;
                    }
                }
            }
            return result;
        }

        private void UpgradeTroop(PartyBase party, int rosterIndex, TORTroopUpgradeArgs upgradeArgs)
        {
            TroopRoster memberRoster = party.MemberRoster;
            CharacterObject upgradeTarget = upgradeArgs.UpgradeTarget;
            int possibleUpgradeCount = upgradeArgs.PossibleUpgradeCount;
            int xpToUpgradeCount = upgradeArgs.UpgradeXpCost * possibleUpgradeCount;
            if (xpToUpgradeCount > 0)
            {
                memberRoster.SetElementXp(rosterIndex, memberRoster.GetElementXp(rosterIndex) - xpToUpgradeCount);
                //memberRoster.AddToCounts(upgradeArgs.Target, -possibleUpgradeCount, false, 0, 0, true, -1);
                party.AddMember(upgradeArgs.Target, -possibleUpgradeCount, 0);
                //memberRoster.AddToCounts(upgradeTarget, possibleUpgradeCount, false, 0, 0, true, -1);
                party.AddMember(upgradeArgs.UpgradeTarget, possibleUpgradeCount, 0);

            /* removed because the ai doesn't care about items, and the player's upgrades never pass through this method
            if (party.Owner != null && party.Owner.Clan == Clan.PlayerClan && upgradeTarget.UpgradeRequiresItemFromCategory != null)
            {
                int num2 = possibleUpgradeCount;
                foreach (ItemRosterElement itemRosterElement in party.ItemRoster)
                {
                    if (itemRosterElement.EquipmentElement.Item.ItemCategory == upgradeTarget.UpgradeRequiresItemFromCategory && itemRosterElement.EquipmentElement.ItemModifier == null)
                    {
                        int num3 = MathF.Min(num2, itemRosterElement.Amount);
                        party.ItemRoster.AddToCounts(itemRosterElement.EquipmentElement.Item, -num3);
                        num2 -= num3;
                        if (num2 == 0)
                        {
                            break;
                        }
                    }
                }
            }
            */
            
                ApplyEffects(party, upgradeArgs);
            }
        }

        private void ApplyEffects(PartyBase party, TORTroopUpgradeArgs upgradeArgs)
        {
            if (party.Owner != null && party.Owner.IsAlive)
            {
                SkillLevelingManager.OnUpgradeTroops(party, upgradeArgs.Target, upgradeArgs.UpgradeTarget, upgradeArgs.PossibleUpgradeCount);
                GiveGoldAction.ApplyBetweenCharacters(party.Owner, null, upgradeArgs.UpgradeGoldCost * upgradeArgs.PossibleUpgradeCount, true);
                return;
            }
            if (party.LeaderHero != null && party.LeaderHero.IsAlive)
            {
                SkillLevelingManager.OnUpgradeTroops(party, upgradeArgs.Target, upgradeArgs.UpgradeTarget, upgradeArgs.PossibleUpgradeCount);
                GiveGoldAction.ApplyBetweenCharacters(party.LeaderHero, null, upgradeArgs.UpgradeGoldCost * upgradeArgs.PossibleUpgradeCount, true);
            }
        }

        private readonly struct TORTroopUpgradeArgs(CharacterObject target, CharacterObject upgradeTarget, int possibleUpgradeCount, int upgradeGoldCost, int upgradeXpCost, float upgradeChance)
        {
            public readonly CharacterObject Target = target;
            public readonly CharacterObject UpgradeTarget = upgradeTarget;
            public readonly int PossibleUpgradeCount = possibleUpgradeCount;
            public readonly int UpgradeGoldCost = upgradeGoldCost;
            public readonly int UpgradeXpCost = upgradeXpCost;
            public readonly float UpgradeChance = upgradeChance;
        }

        public override void SyncData(IDataStore dataStore) { }
    }
}
