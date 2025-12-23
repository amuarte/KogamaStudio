using HarmonyLib;
using Il2Cpp;
using System.Runtime.InteropServices;

namespace KogamaStudio.Tools;

//thanks kogamatools

[HarmonyPatch]
internal static class AntiAFK
{
    public static bool Enabled = false;

    [HarmonyPatch(typeof(AwayMonitor), "Update")]
    [HarmonyPrefix]
    private static bool Update()
    {
        return !Enabled;
    }
}