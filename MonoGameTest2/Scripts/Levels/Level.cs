using Microsoft.Xna.Framework;
using MonoGameTest2.Managers;
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
            public uint X;
            public uint Y;
            public TileTypes TileType;
            public uint TextureIndex;
            public Portal? Portal;
            public bool Solid => TileType == TileTypes.Wall;

            public Tile(uint x, uint y, TileTypes tileType)
            {
                X = x;
                Y = y;
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

                var tileRectangle = new Rectangle((int)(tile.X * LevelManager.TileSize.X), (int)(tile.Y * LevelManager.TileSize.Y), (int)LevelManager.TileSize.X, (int)LevelManager.TileSize.Y);
                yield return tileRectangle;
            }
        }
    }
}
