using Il2Cpp;
using MelonLoader;
using MV.WorldObject;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace KogamaStudio.Models
{
    internal class ModelBuilder
    {
        public static void Build()
        {
        }

        internal static void PlaceCube()
        {
            try
            {
                var desktopEditModeController = MVGameControllerBase.EditModeUI.Cast<DesktopEditModeController>();
                var editorStateMachine = desktopEditModeController.EditModeStateMachine;
                var cubeModelingStateMachine = editorStateMachine.cubeModelingStateMachine;
                var targetModel = cubeModelingStateMachine.TargetCubeModel;

                if (targetModel == null)
                {
                    return;
                }

                byte[] defaultMaterials = { 21, 21, 21, 21, 21, 21 };
                byte[] defaultCorners = { 20, 120, 124, 24, 4, 104, 100, 0 };

                var cube = new Cube(
                    new Il2CppStructArray<byte>(defaultCorners),
                    new Il2CppStructArray<byte>(defaultMaterials)
                );

                var position = new Il2CppMV.WorldObject.IntVector(0, 0, 0);

                if (targetModel.ContainsCube(position))
                {
                    targetModel.RemoveCube(position);
                }

                targetModel.AddCube(position, cube);
                targetModel.HandleDelta();

                MelonLogger.Msg("[ModelBuilder] Placed cube!");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ModelBuilder] Error: {ex.Message}");
            }
        }
    }
}
