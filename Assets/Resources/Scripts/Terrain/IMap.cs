using UnityEngine;

namespace Terrain
{
    public interface IMap
    {
        public Vector2Int StartPosition { get; }
        TerrainType[,] Map { get; }
    }
}