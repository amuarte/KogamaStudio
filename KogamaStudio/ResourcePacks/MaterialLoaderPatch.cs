using Il2Cpp;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace KogamaStudio.Textures
{
    [HarmonyPatch(typeof(MaterialLoader), "SetMainTexture")]

    public class MaterialLoaderPatch
    {
        public static void Prefix(Texture texture)
        {
            //maybe i will do it in the future
        }
    }
}
