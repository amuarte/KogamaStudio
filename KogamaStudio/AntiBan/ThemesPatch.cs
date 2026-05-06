using System;
using HarmonyLib;
using UnityEngine;

namespace KogamaStudio.AntiBan;

[HarmonyPatch(typeof(Theme), "Initialize", new Type[] { })]
internal static class BlockThemeInit
{
    [HarmonyPrefix]
    private static bool Prefix() => false;
}

[HarmonyPatch(typeof(Theme), "Initialize", new Type[] { typeof(int) })]
internal static class BlockThemeInitInt
{
    [HarmonyPrefix]
    private static bool Prefix() => false;
}

[HarmonyPatch(typeof(Resources), "UnloadUnusedAssets")]
internal static class BlockUnloadUnusedAssets
{
    [HarmonyPrefix]
    private static bool Prefix() => false;
}

[HarmonyPatch(typeof(Theme), "Activate")]
internal static class FixThemeActivate
{
    [HarmonyPrefix]
    private static void Prefix(Theme __instance, out bool __state)
    {
        __state = __instance.overrideSkyboxManager;
        if (MVGameControllerBase.instance == null && __instance.overrideSkyboxManager)
        {
            var mgr = UnityEngine.Object.FindObjectOfType<SkyboxManager>();
            if (mgr != null) mgr.enabled = false;
            __instance.overrideSkyboxManager = false;
        }
    }

    [HarmonyPostfix]
    private static void Postfix(Theme __instance, bool __state)
    {
        __instance.overrideSkyboxManager = __state;
    }
}

[HarmonyPatch(typeof(Theme), "Deactivate")]
internal static class FixThemeDeactivate
{
    [HarmonyPrefix]
    private static void Prefix(Theme __instance, out bool __state)
    {
        __state = __instance.overrideSkyboxManager;
        if (MVGameControllerBase.instance == null && __instance.overrideSkyboxManager)
        {
            var mgr = UnityEngine.Object.FindObjectOfType<SkyboxManager>();
            if (mgr != null) mgr.enabled = true;
            __instance.overrideSkyboxManager = false;
        }
    }

    [HarmonyPostfix]
    private static void Postfix(Theme __instance, bool __state)
    {
        __instance.overrideSkyboxManager = __state;
    }
}

[HarmonyPatch(typeof(ThemeSkybox), "Activate")]
internal static class FixSkyboxActivate
{
    [HarmonyPrefix]
    private static bool Prefix(ThemeSkybox __instance)
    {
        if (MVGameControllerBase.instance != null)
            return true;

        var cam = UnityEngine.Camera.main;
        if (cam == null) return false;

        var skyboxComp = cam.GetComponent<UnityEngine.Skybox>();
        if (skyboxComp == null) return false;

        __instance.previousSkyboxMaterial = skyboxComp.material;
        __instance.previousClearFlags = cam.clearFlags;
        if (__instance.skyboxMaterial != null)
        {
            skyboxComp.material = __instance.skyboxMaterial;
            cam.clearFlags = UnityEngine.CameraClearFlags.Skybox;
        }
        return false;
    }
}

[HarmonyPatch(typeof(ThemeSkybox), "Deactivate")]
internal static class FixSkyboxDeactivate
{
    [HarmonyPrefix]
    private static bool Prefix(ThemeSkybox __instance)
    {
        if (MVGameControllerBase.instance != null)
            return true;

        var cam = UnityEngine.Camera.main;
        if (cam == null) return false;

        var skyboxComp = cam.GetComponent<UnityEngine.Skybox>();
        if (skyboxComp == null) return false;

        skyboxComp.material = __instance.previousSkyboxMaterial;
        cam.clearFlags = __instance.previousClearFlags;
        return false;
    }
}
