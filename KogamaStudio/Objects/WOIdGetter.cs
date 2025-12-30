//using System;
//using HarmonyLib;
//using Il2Cpp;
//using MelonLoader;

//namespace KogamaStudio.Objects
//{
//    [HarmonyPatch]
//    internal class WOIdGetter
//    {
//        public static int? LastSelectedWOId;
//        public static bool Enabled = false;


//        [HarmonyPatch(typeof(SelectionController), "SelectWO",
//    new Type[] { typeof(int), typeof(bool), typeof(bool) })]
//        [HarmonyPrefix]
//        static bool SelectPrefix(int id, bool addToSelection, bool showVisuals)
//        {
//            TextCommand.NotifyUser($"selected woid: {id}");
//            MelonLogger.Msg($"selected woid: {id}");
//            LastSelectedWOId = id;
//            return false;
//        }
//    }
//}