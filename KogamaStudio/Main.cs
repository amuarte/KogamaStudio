using MelonLoader;
using UnityEngine;
using Il2Cpp;
using ImGuiNET;
using UnityEngine.Windows;
using KogamaStudio.Tools;

[assembly: MelonInfo(typeof(KogamaStudio.Main), "KogamaStudio", "0.3.0-dev", "Amuarte")]
[assembly: MelonGame("Multiverse ApS", "KoGaMa")]

namespace KogamaStudio
{
    public class Main : MelonMod
    {
        public static bool gameInitialized = false;
        private static bool harmonyPatched = false;
        public override void OnInitializeMelon()
        {
            if (harmonyPatched) return;

            DllLoader.Load("KogamaStudio-ImGui-Hook.dll");
            CommandHandler.StartListening();
            DirectoryManager.Initialize();

            var methods = typeof(SelectionController).GetMethods()
    .Where(m => m.Name == "SelectWO");
            foreach (var m in methods)
                MelonLogger.Msg($"{m.Name}({string.Join(", ", m.GetParameters().Select(p => p.ParameterType.Name))})");
        }

        public override void OnUpdate()
        {

            string cursor = Cursor.visible || Cursor.lockState == CursorLockMode.None ? "true" : "false";
            PipeClient.SendCommand($"cursor|{cursor}");

            if (!gameInitialized)
            {
                if (MVGameControllerBase.IsInitialized)
                {
                    //HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("KogamaStudio");
                    //harmony.PatchAll();

                    MelonLogger.Msg("KogamaStudio loaded!");

                    gameInitialized = true;
                    PipeClient.SendCommand("game_initialized");
                    TextCommand.NotifyUser("<b>KogamaStudio</b> v0.3.0 loaded!\nPress <b>F2</b> to open menu.");
                }
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.F2))
            {
                PipeClient.SendCommand("key_down|F2");
            }
        }
    }
}
