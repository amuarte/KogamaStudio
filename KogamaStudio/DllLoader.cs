using MelonLoader;
using System.Runtime.InteropServices;

namespace KogamaStudio
{
    internal class DllLoader
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        public static void Load(string dllName)
        {

            IntPtr handle = LoadLibrary(dllName);
            if (handle == IntPtr.Zero)
                MelonLogger.Error($"Failed to load {dllName}");
        }
    }
}
