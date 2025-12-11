using Il2Cpp;
using MelonLoader;
using System.IO.Pipes;


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
            string[] parts = cmd.Split(':');
            string command = parts[0];

            MelonCoroutines.Start(ExecuteCommand(command, parts.Length > 1 ? parts[1] : ""));
        }

        private static System.Collections.IEnumerator ExecuteCommand(string command, string param)
        {
            yield return null;

            try { 
                switch (command)
                {
                    case "swap_texture":
                        string basePath = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            "KogamaStudio",
                            "ResourcePacks"
                        );

                        string texturePath = Path.Combine(basePath, "fire_pack", "Fire.png");

                        TextureSwapper.SwapFireTexture(texturePath);
                        break;
                    case "test_message":
                        TextCommand.NotifyUser("Hello World!");
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
