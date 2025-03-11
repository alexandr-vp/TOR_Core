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
        
        foreach (var element in t)
        {
            var item = element.EquipmentElement.Item;
            
           // item.StringId = item.StringId + "_001";

           var number = MBRandom.RandomInt(); 
           
           
           var newItem = Game.Current.ObjectManager.GetObject<ItemObject>(item.StringId);



           newItem.StringId = item.StringId + number;
        
        
               
               
           
           newItem = Game.Current.ObjectManager.RegisterPresumedObject(newItem);
           
            
           
     
           if(newItem==null)
               continue;
           
           
           
           //ItemObject.InitAsPlayerCraftedItem(ref newItem);
           EquipmentElement elem = new(newItem);
           
           
           var modifier = new ItemModifier(); 
           modifier = Game.Current.ObjectManager.GetObject<ItemModifier>("tor_epic_enchantment");       //works like a charm no need to get more complex on the item modifer groups
           modifier.StringId = "test"+MBRandom.RandomInt(); //freshly created item trait id, matches modifier stringID, for having an unique identifier
           
           
               
           //elem.SetModifier(modifier);


           
           
               
               
           //  Hero.MainHero.PartyBelongedTo.ItemRoster.AddToCounts(elem,1);
           Hero.MainHero.PartyBelongedTo.ItemRoster.AddToCounts(newItem,1);
           
           
           /*if (newItem.ArmorComponent != null)
           {
               var modifier = new ItemModifier();       //Retrieve from a XML for base setup , can be also contain some base extras, like enchanced item value or 
               
               modifier = Game.Current.ObjectManager.GetObject<ItemModifier>("tor_epic_enchantment");       //works like a charm no need to get more complex on the item modifer groups
               modifier.StringId = "test"+MBRandom.RandomInt(); //freshly created item trait id, matches modifier stringID, for having an unique identifier
               
               
               elem.SetModifier(modifier);
               
               
             //  Hero.MainHero.PartyBelongedTo.ItemRoster.AddToCounts(elem,1);
               Hero.MainHero.PartyBelongedTo.ItemRoster.AddToCounts(elem,1);
           }
           else
           {
               //old working only for crafted swords.
               Crafting.GenerateItem(item.WeaponDesign,new TextObject("test"),Hero.MainHero.Culture,new ItemModifierGroup(), ref newItem);
               
               Hero.MainHero.PartyBelongedTo.ItemRoster.AddToCounts(newItem,1);
           }*/
  
           
           
           

          
    
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