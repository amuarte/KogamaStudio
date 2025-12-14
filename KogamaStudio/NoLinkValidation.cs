using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MV.WorldObject;

namespace KogamaStudio
{
    [HarmonyPatch]
    internal static class NoLinkValidation
    {
        [HarmonyPatch(typeof(LogicObjectManager), "ValidateLink", 
            new[] { typeof(int), typeof(int), typeof(IWorldObjectManager), typeof(bool) },
            new[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Ref })]
        [HarmonyPrefix]
        private static bool ValidateLink(ref bool loopDetected)
        {
            loopDetected = false;
            return false;
        }
    }
}
