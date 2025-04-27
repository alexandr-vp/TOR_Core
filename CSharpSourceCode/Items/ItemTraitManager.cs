using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TaleWorlds.LinQuick;
using TOR_Core.Utilities;

namespace TOR_Core.Items
{
    public class ItemTraitManager
    {
        private static readonly Lazy<ItemTraitManager> _instance = new(() => new ItemTraitManager());
        private static readonly string _fileName = "tor_itemtraits.xml";
        private List<ItemTrait> _itemTraits;

        public static ItemTraitManager Instance => _instance.Value;

        private ItemTraitManager()
        {
            _itemTraits = [];
        }

        public static void LoadItemTraits()
        {
            var path = TORPaths.TORCoreModuleExtendedDataPath + _fileName;
            if (File.Exists(path))
            {
                XmlSerializer serializer = new(typeof(List<ItemTrait>), new XmlRootAttribute("ItemTraits"));
                using FileStream fileStream = new(path, FileMode.Open);
                Instance._itemTraits = (List<ItemTrait>)serializer.Deserialize(fileStream);
            }
            else
            {
                throw new FileNotFoundException($"The file at path {path} does not exist.");
            }
        }

        public List<ItemTrait> GetItemTraits()
        {
            return _itemTraits;
        }
    }
}
