using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace MonoGameTest2.Levels
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
            public TileTypes TileType;
            public uint TextureIndex;
            public Portal? Portal;

            public Tile(TileTypes tileType)
            {
                TileType = tileType;
                TextureIndex = 0;
                Portal = null;
            }
        }

        public struct Portal
        {
            public byte TargetMapID;
        }

        public byte MapID;
        public byte TileSet;
        public uint Width;
        public uint Height;

        private readonly List<Tile>[,] _tiles;

        public Level(uint width, uint height)
        {
            Width = width;
            Height = height;
            _tiles = new List<Tile>[width, height];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    _tiles[x, y] = new List<Tile>();
                }
            }
        }

        public Tile? GetTile(uint x, uint y)
        {
            if (x >= Width || y >= Height)
            {
                return null;
            }

            var tiles = _tiles[x, y];

            if (tiles.Count > 0)
            {
                return _tiles[x, y][0];
            }

            return null;
        }

        public void SetTile(uint x, uint y, Tile newTile)
        {
            if (x >= Width || y >= Height)
            {
                return;
            }

            if (_tiles[x, y].Count == 0)
            {
                _tiles[x, y] = new List<Tile> { newTile };
            }
            else 
            {
                var tile = _tiles[x, y][0];
                if (tile.TileType == newTile.TileType && tile.TextureIndex == newTile.TextureIndex)
                {
                    return;
                }
                else
                {
                    _tiles[x, y][0] = newTile;
                }
            }

        }

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
