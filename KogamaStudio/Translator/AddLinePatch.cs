using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Il2Cpp;
using Il2CppSystem;
using MelonLoader;

namespace KogamaStudio.Translator
{
    [HarmonyPatch]
    internal class AddLinePatch
    {
        public static SendMessageControl SendMessageControlInstance = null;

        [HarmonyPatch(typeof(ChatControllerBase), "AddChatLine")]
        [HarmonyPrefix]
        private static void AddChatLinePrefix(Il2CppSystem.Collections.Generic.Dictionary<Il2CppSystem.Object, Il2CppSystem.Object> data)
        {
            MelonLogger.Msg($"Dictionary: {data}");
        }

        [HarmonyPatch(typeof(ChatControllerBase), "FormatSayChatMessage")]
        [HarmonyPostfix]
        private static void SayChatPostfix(ref string __result)
        {
            TextCommand.NotifyUser(__result);
        }

        [HarmonyPatch(typeof(ChatControllerBase), "FormatTeamChatMessage")]
        [HarmonyPostfix]
        private static void TeamChatPostfix(ref string __result)
        {
            TextCommand.NotifyUser(__result);
        }

        [HarmonyPatch(typeof(SendMessageControl), "SendChatMessage")]
        [HarmonyPrefix]
        private static bool SendMessagePrefix(ref string chatMsg)
        {
            try
            {
                chatMsg = MessageTranslator.Translate(chatMsg).Result;
                return true;
            }
            catch (System.Exception e)
            {
                MelonLogger.Error($"Translation failed: {e}");
                return true;
            }
        }
    }
}
