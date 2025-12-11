
using MelonLoader;
using HarmonyLib;

[assembly: MelonInfo(typeof(KogamaStudio.Main), "KogamaStudio", "0.1.0-dev", "Amuarte")]
[assembly: MelonGame("Multiverse ApS", "KoGaMa")]

namespace KogamaStudio
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            DirectoryManager.Initialize();
            DllLoader.Load("Mods\\Universal-ImGui-Hook.dll");
            CommandHandler.StartListening();

            //HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("KogamaStudio");
            //harmony.PatchAll();

            //MelonLogger.Msg("KogamaStudio loaded!");
        }

    }
}
