using HarmonyLib;
using Il2Cpp;
using Il2CppSystem.Collections.Generic;
using MV.Common;

namespace KogamaStudio.Debugging;

[HarmonyPatch]
public class CreateAndEquipPatch
{
    [HarmonyPatch(typeof(MVPickupOwner), nameof(MVPickupOwner.CreateAndEquipNewItem))]
    [HarmonyPrefix]
    static void Prefix(
        MVPickupOwner __instance,
        Il2CppSystem.Collections.Generic.Dictionary<Il2CppSystem.Object, Il2CppSystem.Object> newState)
    {
        if (newState == null)
        {
            KogamaStudio.Log.LogInfo("[EquipPatch] newState == null");
            return;
        }

        KogamaStudio.Log.LogInfo($"[EquipPatch] wywołane na: {__instance?.name}");

        foreach (var kvp in newState)
        {
            string key = kvp.Key?.ToString() ?? "null";
            string value;

            try
            {
                value = kvp.Value.Unbox<int>().ToString();
            }
            catch
            {
                try
                {
                    value = Il2CppSystem.Convert.ToInt32(kvp.Value).ToString();
                }
                catch
                {
                    value = kvp.Value?.ToString() ?? "null";
                }
            }

            KogamaStudio.Log.LogInfo($"[EquipPatch]   [{key}] = {value}");
        }
    }

    public static void GiveItem(MVPickupOwner owner, AvatarItemType itemType, int variantId = 0)
    {
        var state = new Il2CppSystem.Collections.Generic.Dictionary<Il2CppSystem.Object, Il2CppSystem.Object>();

        state.Add("type", (int)itemType);
        state.Add("variantId", variantId);
        state.Add("updateItemState", 4);
        owner.CreateAndEquipNewItem(state);
    }

    public static MVPickupOwner GetLocalPickupOwner()
    {
        var woid = MVGameControllerBase.LocalPlayer.WoId;
        var woc = MVGameControllerBase.WOCM.GetWorldObjectClient(woid);

        if (woc == null) return null;

        return woc.gameObject?.GetComponent<MVPickupOwner>();
    }
}