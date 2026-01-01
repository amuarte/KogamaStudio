//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using HarmonyLib;
//using Il2CppCodeStage.AntiCheat.Detectors;

//namespace KogamaStudio.AntiBan
//{
//    [HarmonyPatch]
//    internal class AntiCheatPatches
//    {
//        [HarmonyPatch(typeof(InjectionDetector), "StartDetection")]
//        [HarmonyPrefix]
//        private static bool StartDetectionPrefix()
//        {
//            return false;
//        }

//        [HarmonyPatch(typeof(InjectionDetector), "OnCheatingDetected")]
//        [HarmonyPrefix]
//        private static bool OnCheatingDetectedPrefix(string cause)
//        {
//            return false;
//        }

//        [HarmonyPatch(typeof(InjectionDetector), "FindInjectionInCurrentAssemblies")]
//        [HarmonyPrefix]
//        private static bool FindInjectionPrefix(out string cause)
//        {
//            cause = "";
//            return false;
//        }
//    }
//}
