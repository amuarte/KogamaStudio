
namespace KogamaStudio
{
    public static class PathHelper
    {
        public static string GetPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "KogamaStudio"       
            );
        }
    }
}
