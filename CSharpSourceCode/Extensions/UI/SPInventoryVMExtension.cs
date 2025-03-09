using System;
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


        ItemObject newItem = null;

        foreach (var element in t)
        {
            var item = element.EquipmentElement.Item;
            
            
            //ItemObject.InitAsPlayerCraftedItem(ref item);

            var ta = item.IsCraftedWeapon;
            
 
           // item.StringId = item.StringId + "_001";



           newItem = new ItemObject(item);
           
           Crafting.GenerateItem(item.WeaponDesign,new TextObject("test"),Hero.MainHero.Culture,new ItemModifierGroup(), ref newItem);
           
           

          
    
            break;
        }
        if(t.IsEmpty())
            return;
        
        
        Hero.MainHero.PartyBelongedTo.ItemRoster.AddToCounts(newItem,1);
        
       // InventoryManager.InventoryLogic.TransferOne(rosterElement);

       //Hero.MainHero.PartyBelongedTo.ItemRoster.AddToCounts(item, 1);
        
        TORCommon.Say("test");
    }
}