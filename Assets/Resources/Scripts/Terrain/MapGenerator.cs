using UnityEngine;

namespace Terrain
{
    public abstract class MapGenerator : IMapGenerator
    {
        public Vector2Int StartPosition 
        { 
            get => _startPosition;
            private set => _startPosition = value;
        }
        protected Vector2Int _startPosition;

        protected int WidthX;
        protected int HightY;
        protected byte[] Map;
        public enum PointType : byte { asr,jh}
        protected void SetWallAroundGround()
        {
        }
        private void FillSquareAroundThePoint(Vector3Int pointPosition, int radius)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    _tilemapGround.SetTile(new(x, y, pointPosition.z), _tileGround);
                    _tilesLeft--;
                }
            }
        }
    }
}