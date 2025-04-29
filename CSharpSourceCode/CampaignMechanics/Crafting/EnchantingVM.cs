using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TOR_Core.Extensions;
using TOR_Core.Items;
using TOR_Core.Utilities;

namespace TOR_Core.CampaignMechanics.Crafting
{
    public class EnchantingVM : ViewModel
    {
        private Action _closeAction;
        private MBBindingList<EnchantableItemVM> _items;
        private EnchantableItemVM _selectedItem;
        private EnchantingItemTableauVM _itemTableau;
        private bool _isPreviewEnabled;
        private MBBindingList<EnchantableTraitVM> _traits;
        private MBBindingList<EnchantableTraitVM> _selectedTraits;

        public EnchantingVM(Action closeScreen)
        {
            _closeAction = closeScreen;
            Items = [];
            Traits = [];
            SelectedTraits = [];
            ItemTableau = new EnchantingItemTableauVM();
            IsPreviewEnabled = false;
            foreach (var item in MobileParty.MainParty.ItemRoster)
            {
                if (item.EquipmentElement.Item.IsEnchantable())
                {
                    EnchantableItemVM enchantableItem = new EnchantableItemVM(item.EquipmentElement, OnItemSelected);
                    Items.Add(enchantableItem);
                }
            }
            foreach (var trait in ItemTrait.All.Where(x => x.IsCraftable).OrderBy(y => y.ItemTraitName))
            {
                Traits.Add(new EnchantableTraitVM(trait, OnTraitSelected));
            }
        }

        private void OnTraitSelected(EnchantableTraitVM itemTrait, bool isSelected)
        {
            if (isSelected)
            {
                if (SelectedTraits.Count >= 3)
                {
                    InformationManager.ShowInquiry(new InquiryData("Enchanting", "You can only select up to 3 traits.", true, false, "OK", null, null, null), true);
                    itemTrait.DeselectTrait();
                    return;
                }
                if (!SelectedTraits.Contains(itemTrait))
                {
                    SelectedTraits.Add(itemTrait);
                }
            }
            else
            {
                if (SelectedTraits.Contains(itemTrait))
                {
                    SelectedTraits.Remove(itemTrait);
                }
            }
            TORArtisanDistrictCampaignBehavior.Instance.ItemBeingCrafted = new TorItemBeingCraftedData { EquipmentElement = SelectedItem.Item, ItemTraits = SelectedTraits.Select(x => x.ItemTrait).ToList() };
            ItemTableau.FillFrom(SelectedItem.Item);
        }

        private void OnItemSelected(EnchantableItemVM item)
        {
            SelectedItem?.DeselectItem();
            SelectedItem = item;
            TORArtisanDistrictCampaignBehavior.Instance.ItemBeingCrafted = new TorItemBeingCraftedData { EquipmentElement = item.Item, ItemTraits = SelectedTraits.Select(x => x.ItemTrait).ToList() };
            ItemTableau.FillFrom(item.Item);
            IsPreviewEnabled = true;
        }

        private void ExecuteEnchant()
        {
            if (SelectedTraits.Count == 0)
            {
                InformationManager.ShowInquiry(new InquiryData("Enchanting", "No traits are selected.", true, false, "OK", null, null, null), true);
                return;
            }
            else
            {
                var item = _selectedItem.Item.Item;
                InformationManager.ShowTextInquiry(new TextInquiryData("Enchanting", "Enter name for your enchanted item", true, false, "OK", null, (name) => CreateNewItem(name, item), null));
                
            }
        }

        private void CreateNewItem(string newItemName, ItemObject item)
        {
            var number = MBRandom.RandomInt();
            List<string> traits = _selectedTraits.Select(x => x.ItemTrait.ItemTraitStringId).ToList();
            var newItem = TORArtisanDistrictCampaignBehavior.CreateItemCopy(item, item.StringId + number, newItemName, traits);

            if (newItem == null)
            {
                InformationManager.ShowInquiry(new InquiryData("Enchanting", "Enchanting failed.", true, false, "OK", null, null, null), true);
                return;
            }

            MobileParty.MainParty.ItemRoster.AddToCounts(newItem, 1);
            TORCampaignEvents.Instance.OnItemDuplicated(newItem, item, traits);
            InformationManager.ShowInquiry(new InquiryData("Enchanting", "Enchanting successful. Item was added to inventory.", true, false, "OK", null, null, null), true);
            MobileParty.MainParty.ItemRoster.AddToCounts(item, -1);
        }

        private void ExecuteClose()
        {
            TORArtisanDistrictCampaignBehavior.Instance.ItemBeingCrafted = null;
            _closeAction();
        }

        [DataSourceProperty]
        public MBBindingList<EnchantableItemVM> Items
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChangedWithValue(value, "Items");
                }
            }
        }

        [DataSourceProperty]
        public EnchantableItemVM SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChangedWithValue(value, "SelectedItem");
                }
            }
        }

        [DataSourceProperty]
        public EnchantingItemTableauVM ItemTableau
        {
            get => _itemTableau;
            set
            {
                if (_itemTableau != value)
                {
                    _itemTableau = value;
                    OnPropertyChangedWithValue(value, "ItemTableau");
                }
            }
        }

        [DataSourceProperty]
        public bool IsPreviewEnabled
        {
            get => _isPreviewEnabled;
            set
            {
                if (_isPreviewEnabled != value)
                {
                    _isPreviewEnabled = value;
                    OnPropertyChangedWithValue(value, "IsPreviewEnabled");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<EnchantableTraitVM> Traits
        {
            get => _traits;
            set
            {
                if (_traits != value)
                {
                    _traits = value;
                    OnPropertyChangedWithValue(value, "Traits");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<EnchantableTraitVM> SelectedTraits
        {
            get => _selectedTraits;
            set
            {
                if (_selectedTraits != value)
                {
                    _selectedTraits = value;
                    OnPropertyChangedWithValue(value, "SelectedTraits");
                }
            }
        }
    }
}
