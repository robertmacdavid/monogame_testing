using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MonoGameTest2.Level
{
    public class Level
    {
        public enum TileTypes
        {
            Floor,
            Wall
        }

        public struct Tile
        {
            public TileTypes Type;
            public uint TextureIndex;
            public uint OverlappingTile;
            public uint Portal;
        }

        public byte MapID;
        public byte TileSet;
        public uint Width;
        public uint Height;
        public Tile[,][] Tiles;

        public void Serialize(string fileName)
        {
            var serializer = new XmlSerializer(typeof(Level));
            using (var writer = XmlWriter.Create(fileName))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}
