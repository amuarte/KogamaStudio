//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using HarmonyLib;
//using Il2Cpp;
//using MelonLoader;
//using UnityEngine;

//namespace KogamaStudio.Objects
//{
//    [HarmonyPatch]
//    internal class AddItemToWorldPatch
//    {
//        [HarmonyPatch(typeof(MVNetworkGame.OperationRequests), "AddItemToWorld")]
//        [HarmonyPrefix]
//        static bool AddItemToWorldPrefix(int itemId, int groupId, Vector3 position, Quaternion rotation, bool localOwner, bool transferOwnershipToServerOnLeave, bool isPreviewItem)
//        {
//            string message = $"Added item to world!\n" +
//                $"\titemId: {itemId}\n" +
//                $"\tgroupId: {groupId}\n" +
//                $"\tposition: {position}\n" +
//                $"\trotation: {rotation}\n" +
//                $"\tlocalOwner: {localOwner}\n" +
//                $"\ttransferOwnershipToServerOnLeave: {transferOwnershipToServerOnLeave}\n" +
//                $"\tisPreviewItem: {isPreviewItem}";

//            TextCommand.NotifyUser(message);
//            MelonLogger.Msg(message);
//            itemId = 82;
//            return true;
//        }
//    }
//}
