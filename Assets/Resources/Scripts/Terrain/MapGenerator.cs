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
        protected Vector2Int vector;
        protected int minX ;
        protected int maxX;
        protected int minY;
        protected int maxY;
        protected void SetWallAroundGround()
        {
        }
        protected void FillSquareAroundThePoint(Vector2Int pointPosition, int radius, PointType type, bool CalculateInstalls = true)
        {
            if (!IsPointInMap(pointPosition))
            {
                return;
            }
            minX = pointPosition.x - radius;
            maxX = pointPosition.x + radius;
            minY = pointPosition.y - radius;
            maxY = pointPosition.y + radius;
            CorrectBorderForMap(ref minX, ref maxX);
            CorrectBorderForMap(ref minY, ref maxY);
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
            return x >= 0 ||
                x < MapMaxX ||
                y >= 0 ||
                y < MapMaxY;
        }
        protected void CorrectBorderForMap(ref Vector2Int point)
        {
            point.x = point.x < 0 ? 0 : point.x >= MapMaxX ? MapMaxX - 1 : point.x;
            point.y = point.y < 0 ? 0 : point.y >= MapMaxY ? MapMaxY - 1 : point.y;

        }
        protected void CorrectBorderForMap(ref int x, ref int y)
        {
            vector = new Vector2Int(x, y);
            CorrectBorderForMap(ref vector);
            x = vector.x;
            y = vector.y;
        }
        public abstract void Build(Vector2Int worldSize, int countPoints, Vector2Int startPosition);
    }
}