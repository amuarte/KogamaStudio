
namespace KogamaStudio.Generating.Models
{
    internal class ModelCubeData
    {
        private static readonly byte[] DefaultMaterials = { 21, 21, 21, 21, 21, 21 };
        private static readonly byte[] DefaultCorners = { 20, 120, 124, 24, 4, 104, 100, 0 };

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public byte[] Materials { get; set; }
        public byte[] Corners { get; set; }

        public ModelCubeData(int x, int y, int z, byte[] materials = null, byte[] corners = null) 
        { 
            X = x; Y = y; Z = z;
            Materials = materials ?? DefaultMaterials;
            Corners = corners ?? DefaultCorners;
        }

    }
}
