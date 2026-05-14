using HarmonyLib;
using MV.Common;
using MV.WorldObject;

using UnityEngine;

namespace KogamaStudio.Tools.Build;

// ty becko
[HarmonyPatch]
internal static class CustomWOScale
{
    internal static bool Enabled = false;
    internal static Vector3 Scale = Vector3.one;

    private static MVGroup scaledGroup = null;
    private static bool waitingForGroup = false;

    static CustomWOScale()
    {
        WOReciever.OnWORecieved += OnWORecieved;
    }

    internal static void RequestNewGroupIfNecessary()
    {
        if (scaledGroup == null)
        {
            RequestNewGroup();
            return;
        }

        if (scaledGroup.scale != Scale)
        {
            DeleteScaledGroup();
            RequestNewGroup();
        }
    }

    private static void RequestNewGroup()
    {
        MVGameControllerBase.OperationRequests.RequestBuiltInItem(
            BuiltInItem.Group,
            CommandHandler.EditorStateMachine.ParentGroupID,
            new Il2CppSystem.Collections.Generic.Dictionary<Il2CppSystem.Object, Il2CppSystem.Object>(),
            Vector3.zero,
            Quaternion.identity,
            Scale,
            false,
            true
        );

        waitingForGroup = true;
    }

    private static void DeleteScaledGroup()
    {
        MVGameControllerBase.OperationRequests.UnregisterWorldObject(scaledGroup.id);
        scaledGroup = null;
    }

    private static void OnWORecieved(MVWorldObjectClient root, int instigatorActorNumber)
    {
        if (instigatorActorNumber != MVGameControllerBase.LocalPlayer.ActorNr) return;
        if (!Enabled || !waitingForGroup) return;

        MVGroup group = null;
        try { group = root.Cast<MVGroup>(); } catch { }
        if (group == null) return;

        scaledGroup = group;
        scaledGroup.scale = Scale;
        if (scaledGroup.transform != null)
            scaledGroup.transform.localScale = Scale;
        waitingForGroup = false;
        KogamaStudio.Log.LogInfo($"[CustomWOScale] Scaled group ready: {scaledGroup.id}, scale applied: {Scale}");
    }

    [HarmonyPatch(typeof(MVNetworkGame.OperationRequests), "AddItemToWorld")]
    [HarmonyPrefix]
    private static void AddItemToWorld(ref int groupId, ref Vector3 position)
    {
        if (!Enabled || scaledGroup == null) return;
        groupId = scaledGroup.id;
        // Position arrives in world space; convert to the scaled group's local space
        // so the object appears where the cursor is, not Scale times further away.
        var s = Scale;
        var gp = scaledGroup.Position;
        position = new Vector3(
            (position.x - gp.x) / s.x,
            (position.y - gp.y) / s.y,
            (position.z - gp.z) / s.z);
    }

}
