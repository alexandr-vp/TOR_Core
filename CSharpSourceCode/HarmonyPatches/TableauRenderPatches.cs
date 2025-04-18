using HarmonyLib;
using SandBox.View.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Tableaus;

namespace TOR_Core.HarmonyPatches
{
    [HarmonyPatch]
    public static class TableauRenderPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapConversationTableau), "FirstTimeInit")]
        public static void PostfixMapConversationRender(ref Camera ____continuousRenderCamera, List<AgentVisuals> ____agentVisuals)
        {
            if(____continuousRenderCamera != null && ____agentVisuals != null && ____agentVisuals.Count > 0)
            {
                var eyePos = ____agentVisuals[0].GetGlobalStableEyePoint(true);
                var cameraFrame = ____continuousRenderCamera.Frame;
                cameraFrame.origin.z = eyePos.z - 0.15f;
                ____continuousRenderCamera.Frame = cameraFrame;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TableauCacheManager), "CreateCharacterBaseEntity")]
        public static void PostFixCreateCharacter(CharacterCode characterCode, Scene scene, ref Camera camera, bool isBig)
        {
            if(FaceGen.GetRaceNames()[characterCode.Race] == "dwarf")
            {
                var cameraFrame = camera.Frame;
                cameraFrame.origin.z -= 0.22f;
                camera.Frame = cameraFrame;
            }
        }
    }
}
