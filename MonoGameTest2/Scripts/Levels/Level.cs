using Microsoft.Xna.Framework;
using MonoGameTest2.Managers;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace MonoGameTest2.Levels
{
    public enum TileTypes
    {
        Floor,
        Wall,
        TileTypeCount,
    }

    public struct Portal
    {
        public byte TargetMapID;
    }

    [Serializable]
    public struct Tile
    {
        public uint X;
        public uint Y;
        public TileTypes TileType;
        public uint TextureIndex;
        public bool Solid => TileType == TileTypes.Wall;

        public Portal? Portal { get; set; }

        public Tile(uint x, uint y, TileTypes tileType, uint textureIndex = 0)
        {
            X = x;
            Y = y;
            TileType = tileType;
            TextureIndex = textureIndex;
            Portal = null;
        }

        public bool ShouldSerializePortal()
        {
            return Portal.HasValue;
        }
    }

    [Serializable]
    public class Level
    {
        public byte MapID;
        public Vector2 PlayerSpawn;
        public uint Width;
        public uint Height;

        // TODO: Maybe have tile sets for different enviroments?
        //public byte TileSet;

        private readonly Tile[,] _tiles;

        public Level(uint width, uint height)
        {
            Width = width;
            Height = height;
            _tiles = new Tile[width, height];
        }

        public Tile? GetTile(uint x, uint y)
        {
            if (x >= Width || y >= Height)
            {
                return null;
            }

            return _tiles[x, y];
        }

        public void SetTile(uint x, uint y, Tile newTile)
        {
            if (x >= Width || y >= Height)
            {
                return;
            }

            _tiles[x, y] = newTile;
        }

        public void Serialize(string fileName)
        {
            var serializer = new XmlSerializer(typeof(Level));
            using (var writer = XmlWriter.Create(fileName))
            {
                serializer.Serialize(writer, this);
            }
        }

        public IEnumerable<Tile> GetAdjacentTiles(int x, int y)
        {
            for (var adjX = x - 1; adjX <= x + 1; adjX++)
            {
                if (adjX < 0 || adjX >= Width)
                {
                    continue;
                }

                for (var adjY = y - 1; adjY <= y + 1; adjY++)
                {
                    if (adjY < 0 || adjY >= Height)
                    {
                        continue;
                    }

                    var tile = GetTile((uint)adjX, (uint)adjY);

                    if (tile.HasValue)
                    {
                        yield return tile.Value;
                    }
                }
            }
        }

        public IEnumerable<Rectangle> GetPossibleCollisions(int x, int y)
        {
            foreach (var tile in GetAdjacentTiles(x, y))
            {
                if (!tile.Solid)
                {
                    continue;
                }

                yield return new Rectangle(
                    (int)(tile.X * LevelManager.TileSize.X),
                    (int)(tile.Y * LevelManager.TileSize.Y),
                    (int)LevelManager.TileSize.X,
                    (int)LevelManager.TileSize.Y
                );
            }
        }
    }
}
