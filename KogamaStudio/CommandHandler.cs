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
using Il2CppSystem.Collections.Generic;
using KogamaStudio.Objects;
using KogamaStudio.Translator;

namespace KogamaStudio
{
    public class CommandHandler
    {
        private static void HandleCloneObject(int id)
        {
            var wo = MVGameControllerBase.WOCM?.GetWorldObjectClient(id);
            if (wo == null) return;
            MVGameControllerBase.OperationRequests.CloneWorldObjectTree(wo, false, false, false);
            TextCommand.NotifyUser($"Cloned {id}");
        }

        private static void HandleRemove(int id)
        {
            var wo = MVGameControllerBase.WOCM?.GetWorldObjectClient(id);
            string error = "";

            if (wo.Delete(MVGameControllerBase.WOCM, ref error))
            {
                string message = $"Removed {id}";
                MelonLogger.Msg(message);
                TextCommand.NotifyUser(message);
            }
        }

        private static void HandleGetAllWoIds()
        {
            Il2CppSystem.Collections.Generic.HashSet<int> ids =
                new Il2CppSystem.Collections.Generic.HashSet<int>();
            MVGameControllerBase.WOCM.GetAllWoIds(75578, ids);

            foreach (int id in ids)
            {
                var wo = MVGameControllerBase.WOCM?.GetWorldObjectClient(id);

                TextCommand.NotifyUser($"{id} {wo.type}");
                MelonLogger.Msg($"{id} {wo.type}");
            }
        }

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
        public static int targetWoId = -1;
        public static string targetPlayerName = null;
        public static bool previousVisible = true;
        public static MVWorldObjectClient savedWo = null;


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

                        MelonLogger.Msg("[CommandHandler] test");

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
                    // SPEED
                    case "option_custom_speed_enabled":
                        EditModeSpeed.MultiplierEnabled = param == "true";
                        break;
                    case "option_custom_speed_value":
                        EditModeSpeed.Multiplier = float.Parse(param);
                        break;
                    case "generate_model":
                        if (!ModelBuilder.IsBuilding)
                        {
                            var cubes = ModelLoader.LoadModel(param);
                            if (cubes != null) 
                            { 
                                MelonCoroutines.Start(ModelBuilder.Build(cubes)); 
                            }
                        }
                        break;
                    case "generate_cancel":
                        
                        ModelBuilder.CancelGeneration = true;
                        break;
                    case "objects_wo_id":
                        targetWoId = int.Parse(param);
                        break;
                    case "objects_get_all_wo_ids":
                        HandleGetAllWoIds();
                        break;
                    case "objects_clone":
                        HandleCloneObject(targetWoId); 
                        break;
                    case "objects_remove":
                        HandleRemove(targetWoId);
                        break;
                    case "objects_visible":
                        break; 
                    case "test":
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
