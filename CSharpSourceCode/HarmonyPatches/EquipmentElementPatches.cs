using HarmonyLib;
using TaleWorlds.Core;
using TOR_Core.Utilities;

namespace TOR_Core.HarmonyPatches;

[HarmonyPatch]
public static class EquipmentElementPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(EquipmentElement),"IsEqualTo")]
    public static void Postfix(ref bool __result, EquipmentElement other , EquipmentElement __instance)
    {

        var b = __result;
        if(__instance.Item==null)return;
        if(other.Item ==null)return;
        var item1 = __instance.Item.StringId;
        var item = other.Item.StringId;
        if (__instance.Item.StringId.Contains("leather_studdedhelm_over_thinhide"))
        {
            TORCommon.Say("hello");
        }


        /*
        //__result = false;
        if (__instance.ItemModifier.StringId == other.ItemModifier.StringId)
        {
            __result = false;
        }*/
    }

}