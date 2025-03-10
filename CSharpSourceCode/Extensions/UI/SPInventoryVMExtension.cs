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
    
    private delegate T GetItemFieldDelegate<out T>(ItemModifier item, string _fieldName);
    
    private static readonly Dictionary<ArmorComponent.ArmorMaterialTypes, string> MaterialTypeMap = new()
    {
        { ArmorComponent.ArmorMaterialTypes.Plate, "plate" },
        { ArmorComponent.ArmorMaterialTypes.Chainmail, "chain" },
        { ArmorComponent.ArmorMaterialTypes.Leather, "leather" },
        { ArmorComponent.ArmorMaterialTypes.Cloth, "cloth" },
        { ArmorComponent.ArmorMaterialTypes.None, "cloth_unarmored" }
    };

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



           newItem = Game.Current.ObjectManager.GetObject<ItemObject>(item.StringId);
           
           
           EquipmentElement elem = new(newItem);
           if (newItem.ArmorComponent != null)
           {
               var modifier = new ItemModifier();       //Retrieve from a XML for base setup , can be also contain some base extras, like enchanced item value or 

               modifier.StringId = "test"+MBRandom.RandomInt(); //freshly created item trait id, matches modifier stringID, for having an unique identifier
               
               
               elem.SetModifier(modifier);
               
               
             //  Hero.MainHero.PartyBelongedTo.ItemRoster.AddToCounts(elem,1);
               Hero.MainHero.PartyBelongedTo.ItemRoster.AddToCounts(elem,1);
           }
           else
           {
              
           
               Crafting.GenerateItem(item.WeaponDesign,new TextObject("test"),Hero.MainHero.Culture,new ItemModifierGroup(), ref newItem);
               
               Hero.MainHero.PartyBelongedTo.ItemRoster.AddToCounts(newItem,1);
           }
  
           
           
           

          
    
            break;
        }
        if(t.IsEmpty())
            return;
        
        
        
        
       // InventoryManager.InventoryLogic.TransferOne(rosterElement);

       //Hero.MainHero.PartyBelongedTo.ItemRoster.AddToCounts(item, 1);
        
        TORCommon.Say("test");
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