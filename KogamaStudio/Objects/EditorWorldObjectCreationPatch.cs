using HarmonyLib;
using UnityEngine;
using Il2Cpp;
using MelonLoader;


namespace KogamaStudio.Objects
{
    [HarmonyPatch]
    internal class EditorWorldObjectCreationPatch
    {
        public static EditorWorldObjectCreation savedEditorWOC;

        [HarmonyPatch(typeof(EditorWorldObjectCreation), "Initialize")]
        [HarmonyPostfix]
        private static void SendPackagePrefix(EditorWorldObjectCreation __instance)
        {
            savedEditorWOC = __instance;
            MelonLogger.Msg("EditorWorldObjectCreation captured on Initialize!");
        }
    }
}
