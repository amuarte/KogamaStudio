
namespace KogamaStudio
{

    internal class DirectoryManager
    {
        private static string _basePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "KogamaStudio"
        );

        public static void Initialize()
        {
            Directory.CreateDirectory(_basePath);
            Directory.CreateDirectory(Path.Combine(_basePath, "ResourcePacks"));
            Directory.CreateDirectory(Path.Combine(_basePath, "Generate", "Models"));

        }
    }
}
