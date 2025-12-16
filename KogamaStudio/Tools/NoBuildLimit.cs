using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace KogamaStudio.Tools
{
    // thanks kogamatools for this code xd

    [HarmonyPatch]
    internal static class NoBuildLimit
    {
        public static bool Enabled = false;

        [HarmonyPatch(typeof(ConstraintVisualizer), "Init")]
        [HarmonyPrefix]
        private static bool Init()
        {
            return !Enabled;
        }

        [HarmonyPatch(typeof(ModelingDynamicBoxConstraint), "CanAddCubeAt")]
        [HarmonyPatch(typeof(ModelingBoxCountConstraint), "CanAddCubeAt")]
        [HarmonyPatch(typeof(ModelingBoxCountConstraint), "CanRemoveCubeAt")]
        [HarmonyPostfix]
        private static void NoBuildLimitPatches(ref bool __result)
        {
            __result |= Enabled;
        }
    }
}
