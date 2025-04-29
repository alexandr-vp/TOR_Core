using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TOR_Core.Utilities;

namespace TOR_Core.Items
{
    public static class ExtendedItemObjectManager
    {
        private static Dictionary<string, ExtendedItemObjectProperties> _itemToInfoMap = [];
        private static string XMLPath = TORPaths.TORCoreModuleExtendedDataPath + "tor_extendeditemproperties.xml";


        internal static ExtendedItemObjectProperties GetAdditionalProperties(string itemId)
        {
            ExtendedItemObjectProperties info = null;
            _itemToInfoMap.TryGetValue(itemId, out info);
            if(info != null) info = info.Clone();
            return info;
        }

        public static void AddCraftedItem(string oldId, string newId, List<string> traits)
        {
            if (_itemToInfoMap.ContainsKey(newId))
            {
                _itemToInfoMap[newId].ItemTraits = traits ?? new List<string>();
                return;
            }
            if (_itemToInfoMap.TryGetValue(oldId, out ExtendedItemObjectProperties info))
            {
                ExtendedItemObjectProperties newInfo = info.Clone();
                newInfo.ItemStringId = newId;
                newInfo.Description = "Crafted " + info.Description;
                newInfo.ItemTraits = traits ?? new List<string>();
                _itemToInfoMap.Add(newId, newInfo);
            }
            else
            {
                var newInfo = ExtendedItemObjectProperties.CreateDefault(newId);
                newInfo.ItemStringId = newId;
                newInfo.Description = "Crafted " + newInfo.Description;
                newInfo.ItemTraits = traits ?? new List<string>();
                _itemToInfoMap.Add(newId, newInfo);
            }
        }

        public static void LoadXML()
        {
            _itemToInfoMap.Clear();
            if (File.Exists(XMLPath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<ExtendedItemObjectProperties>));
                List<ExtendedItemObjectProperties> list = ser.Deserialize(File.OpenRead(XMLPath)) as List<ExtendedItemObjectProperties>;
                if(list != null && list.Count > 0)
                {
                    foreach(var item in list)
                    {
                        _itemToInfoMap.Add(item.ItemStringId, item);
                    }
                }
            }
        }
    }
}
