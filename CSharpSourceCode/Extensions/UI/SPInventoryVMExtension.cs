using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TOR_Core.CampaignMechanics.Crafting;
using TOR_Core.Utilities;

namespace TOR_Core.Extensions.UI;
[ViewModelExtension(typeof(SPInventoryVM))]
public class SPInventoryVMExtension : BaseViewModelExtension
{
    public SPInventoryVMExtension(ViewModel vm) : base(vm)
    {


    }

    public override void OnFinalize()
    {
        base.OnFinalize();

        var t = InventoryManager.InventoryLogic.GetElementsInRoster(InventoryLogic.InventorySide.PlayerInventory);

        foreach (var element in t)
        {
            var item = element.EquipmentElement.Item;
            var number = MBRandom.RandomInt();

            var newItem = TORCraftingCampaignBehavior.CreateItemCopy(item, item.StringId + number, item.Name + "Copy" + number);

            if (newItem == null)
                continue;

            Hero.MainHero.PartyBelongedTo.ItemRoster.AddToCounts(newItem, 1);
            TORCampaignEvents.Instance.OnItemDuplicated(newItem, item);

            break;
        }
    }

    private ItemModifier GetRandomModifierWithTarget(ItemModifierGroup modifierGroup, int modifierTier)
    {
        var results = modifierGroup.ItemModifiers.OrderBy(mod => mod.PriceMultiplier);

        //check if there are more than 6 modifiers

        return results.ElementAt(Math.Min(results.Count() - 1, modifierTier));
    }


    private int GetItemFieldInt(ItemModifier item, string _fieldName)
    {
        var value = item.GetType()?.GetProperty(_fieldName)?.GetValue(item);
        if (value is not null)
            return (int)value;
        else return 0;
    }

}