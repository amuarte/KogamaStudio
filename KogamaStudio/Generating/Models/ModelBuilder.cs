using Il2Cpp;
using MelonLoader;
using MV.WorldObject;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using System.Collections;

namespace KogamaStudio.Generating.Models
{
    internal class ModelBuilder
    {
        private static readonly int BatchSize = 500;
        private static readonly int FrameDelay = 500;

        internal static IEnumerator Build(List<ModelCubeData> cubes)
        {
            MVCubeModelBase targetModel = null;

            try
            {
                var desktopEditModeController = MVGameControllerBase.EditModeUI.Cast<DesktopEditModeController>();
                var editorStateMachine = desktopEditModeController.EditModeStateMachine;
                var cubeModelingStateMachine = editorStateMachine.cubeModelingStateMachine;
                targetModel = cubeModelingStateMachine.TargetCubeModel;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ModelBuilder] Error: {ex.Message}");
            }

            if (targetModel == null)
            {
                yield break;
            }

            int placedCube = 0;
            foreach (ModelCubeData cubeData in cubes)
            {
                var cube = new Cube(
                    new Il2CppStructArray<byte>(cubeData.Corners),
                    new Il2CppStructArray<byte>(cubeData.Materials)
                );

                var position = new Il2CppMV.WorldObject.IntVector(cubeData.X, cubeData.Y, cubeData.Z);

                if (targetModel.ContainsCube(position))
                {
                    targetModel.RemoveCube(position);
                }

                targetModel.AddCube(position, cube);
                placedCube++;

                if (placedCube % BatchSize == 0)
                {
                    targetModel.HandleDelta();
                    yield return new WaitForSecondsRealtime(1f / 60f * FrameDelay);
                }

            }

            targetModel.HandleDelta();
            MelonLogger.Msg($"[ModelBuilder] Placed {cubes.Count} cubes");
        }
    }
}
