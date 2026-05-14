using HarmonyLib;
using Assets.Scripts.WorldObjectTypes.CustomGun;
using UnityEngine;

namespace KogamaStudio.Tools.Build;

[HarmonyPatch(typeof(MVCustomGunBlueprint), "SetMuzzlePointPosition")]
internal static class UnlimitedMuzzlePosition
{
    public static bool Enabled = false;

    [HarmonyPrefix]
    private static bool Prefix()
    {
        return !Enabled;
    }
}
