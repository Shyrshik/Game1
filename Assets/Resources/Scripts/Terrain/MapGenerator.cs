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
        public PointType[,] Map
        {
            get => _map;
        }
        protected PointType[,] _map;

        protected int CountPoints;
        protected int MapMaxX;
        protected int MapMaxY;
        protected int PointsInstalls;
        protected int WalkerNumber = 0;
        protected int i, j;
        protected void SetWallAroundGround()
        {
        }
        protected void FillSquareAroundThePoint(Vector2Int pointPosition, int radius, PointType type, bool CalculateInstalls = true)
        {
            if (!IsPointInMap(pointPosition))
            {
                return;
            }
            int minX = pointPosition.x - radius;
            int maxX = pointPosition.x + radius;
            int minY = pointPosition.y - radius;
            int maxY = pointPosition.y + radius;
            minX = minX < 0 ? 0 : minX;
            maxX = maxX >= MapMaxX ? MapMaxX - 1 : maxX;
            minY = minY < 0 ? 0 : minY;
            maxY = maxY >= MapMaxY ? MapMaxY - 1 : maxY;
            for (i = minX; i <= maxX; i++)
            {
                for (j = minY; j <= maxY; j++)
                {
                    _map[i, j] = type;
                    if (CalculateInstalls)
                        PointsInstalls++;
                }
            }
        }
        protected bool IsPointInMap(Vector2Int pointPosition)
        {
            return IsPointInMap(pointPosition.x, pointPosition.y);
        }
        protected bool IsPointInMap(int x, int y)
        {
            if (x < 0 ||
                x > MapMaxX ||
                y < 0 ||
                y > MapMaxY)
            {
                return false;
            }
            return true;
        }
        public abstract void Build(Vector2Int worldSize, int countPoints, Vector2Int startPosition);
    }
}