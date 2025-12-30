using Il2Cpp;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace KogamaStudio.Debugging
{
    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests), "UpdateWorldObjectDataPartial",
        new System.Type[] { typeof(int), typeof(Il2CppSystem.Collections.Generic.Dictionary<Il2CppSystem.Object, Il2CppSystem.Object>) })]
    class DebugUpdateDictPatch
    {
        static void Prefix(int worldObjectID, Il2CppSystem.Collections.Generic.Dictionary<Il2CppSystem.Object, Il2CppSystem.Object> woData)
        {
            var wo = MVGameControllerBase.WOCM?.GetWorldObjectClient(worldObjectID);
            if (wo != null)
            {
                MelonLogger.Msg($"[DEBUG] {wo.GetType().Name} (ID:{worldObjectID}):");

                if (woData != null && woData.Count > 0)
                {
                    foreach (var kvp in woData)
                    {
                        string key = kvp.Key?.ToString() ?? "null";
                        MelonLogger.Msg($"  Key: '{key}'");
                    }
                }
            }
        }
    }
}
