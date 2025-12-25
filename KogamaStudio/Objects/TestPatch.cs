//using HarmonyLib;
//using UnityEngine;
//using Il2Cpp;
//using Il2CppSystem;
//using Il2CppSystem.Collections.Generic;
//using MelonLoader;


//namespace KogamaStudio.Objects
//{
//    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests), "UpdateWorldObjectDataPartial", new System.Type[]
//    {
//        typeof(int),
//        typeof(Il2CppSystem.Collections.Generic.Dictionary<Il2CppSystem.Object, Il2CppSystem.Object>)
//    })]
//    public class TestPatch
//    {
//        [HarmonyPrefix]
//        public static void Prefix(MVNetworkGame.OperationRequests __instance, int worldObjectID, Il2CppSystem.Collections.Generic.Dictionary<Il2CppSystem.Object, Il2CppSystem.Object>
// woData)
//        {
//            TextCommand.NotifyUser("[TestPatch] Updated object data");
//            MelonLogger.Msg("[TestPatch] Updated object data:");
//            MelonLogger.Msg($"\tworldObjectID: {worldObjectID}");
//            MelonLogger.Msg($"\twoData: {woData}");
//            foreach (var kv in woData)
//                MelonLogger.Msg($"\t{kv.Key}: {kv.Value}");
//        }
//    }
//}
