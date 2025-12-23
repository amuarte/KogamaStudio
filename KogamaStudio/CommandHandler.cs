using Harmony;
using Il2Cpp;
using Il2CppBorodar.FarlandSkies.CloudyCrownPro.DotParams;
using Il2CppSystem.Runtime.InteropServices;
using KogamaStudio.Generating.Models;
using KogamaStudio.ResourcePacks.Materials;
using KogamaStudio.Tools;
using MelonLoader;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using UnityEngine;


namespace KogamaStudio
{
    public class CommandHandler
    {
        public static void StartListening()
        {
            Task.Run(() => Listen());
        }

        private static void Listen()
        {
            try
            {
                while (true)
                {
                    using (var pipe = new NamedPipeServerStream("KogamaStudio"))
                    {    
                        pipe.WaitForConnection();
                        using (var reader = new StreamReader(pipe))
                        {
                            string cmd = reader.ReadToEnd();
                            ProcessCommand(cmd);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CommandHandler] Error: {ex.Message}");
            }
        }

        private static void ProcessCommand(string cmd)
        {
            string[] parts = cmd.Split('|');
            string command = parts[0];

            MelonCoroutines.Start(ExecuteCommand(command, parts.Length > 1 ? parts[1] : ""));
        }

        public static CursorLockMode previousLockState = CursorLockMode.None;
        public static bool previousVisible = true;
        private static bool cursorStateRestored = false;

        private static System.Collections.IEnumerator ExecuteCommand(string command, string param)
        {
            yield return null;

            try { 
                switch (command)
                {
                    case "resourcepacks_load":

                        string path = Path.Combine(PathHelper.GetPath(), "ResourcePacks", param, "materials");

                        if (!Directory.Exists(path))
                        {
                            MelonLogger.Error($"Path not found {path}");
                            break;
                        }

                        var files = Enumerable.Range(0, 69)
                            .Select(i => $"{path}/{i}.png")
                            .ToList();

                        MaterialsLoader.LoadTexture(files);
                        break;

                    case "resourcepacks_reset":
                        MVGameControllerBase.MaterialLoader.SetMainTexture(DefaultMaterials.defaultMaterials, true);
                        break;
                    case "imgui_menu":
                        if (param == "true")
                        {

                        }
                         break;
                    case "option_no_build_limit":
                        NoBuildLimit.Enabled = param == "true";
                        break;
                    case "option_single_side_painting":
                        SingleSidePainting.Enabled = param == "true";
                        break;
                    case "option_anti_afk":
                        AntiAFK.Enabled = param == "true";
                        break;
                    // GRID SIZE
                    case "option_custom_grid_size_enabled":
                        CustomGrid.Enabled = param == "true";
                        break;
                    case "option_custom_grid_size":
                        CustomGrid.GridSize = float.Parse(param);
                        break;
                    // ROTATION STEP
                    case "option_custom_rot_step_enabled":
                        RotationStep.Enabled = param == "true";
                        break;
                    case "option_custom_rot_step_size":
                        RotationStep.Step = float.Parse(param);
                        break;
                    case "generate_model":
                        var cubes = ModelLoader.LoadModel(param);
                        if (cubes != null) MelonCoroutines.Start(ModelBuilder.Build(cubes));
                        break;

                    default:
                        MelonLogger.Msg($"[Commands] Unknown: {command}");
                        break;
                }
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"[ExecuteCommand] Error: {ex.Message}");
            }
        }
    }
}
