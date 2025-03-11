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
            
            var newItem = new ItemObject(item);         //here is the problem , the copied item is not "deep" enough and crashs at reload
            // var newItem = Game.Current.ObjectManager.GetObject<ItemObject>(item.StringId);           //CASE B
            
            
           newItem.StringId = item.StringId + number;       // in case B : it changes the whole "template" not just the single item instance!

           newItem.DetermineItemCategoryForItem();
           newItem.IsReady = true;
           ItemObject.InitAsPlayerCraftedItem(ref newItem);
           Game.Current.ObjectManager.RegisterObject(newItem);

         
     
           if(newItem==null)
               continue;

           //var equipmentItem = new EquipmentElement(newItem);
           
           Hero.MainHero.PartyBelongedTo.ItemRoster.AddToCounts(newItem,1);
           
           
           // create an item trait that gets assigned to the newItem string id - based on a blue print with modified values
           
           //store the trait in a map serialized by the game
           
           //voila - freshly unique item with a dynamic item trait
           
            break;
        }
        if(t.IsEmpty())
            return;
        
        TORCommon.Say("item created");
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