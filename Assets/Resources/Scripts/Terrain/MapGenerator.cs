using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        private Vector2Int _startPosition;
        public TerrainType[,] Map
        {
            get => _map;
        }
        private TerrainType[,] _map;
        protected int CountPoints { get; private set; }
        protected int WalkerNumber { get; private set; }
        protected int PointsInstalls;
        private int MapMaxX;
        private int MapMaxY;

        protected int i, j;
        protected Vector2Int vector;
        protected int minX ;
        protected int maxX;
        protected int minY;
        protected int maxY;
        public MapGenerator() { }
        public MapGenerator(Vector2Int worldSize, int countPoints, int walkerNumber = 1)
        {
            SetParams(worldSize, countPoints, walkerNumber);
        }
        public bool SetParams(Vector2Int worldSize, int countPoints, int walkerNumber = 1)
        {
            if (worldSize.x < 1 || worldSize.y < 1 || countPoints < 1 || _map is not null)
            {
                return false;
            }
            _startPosition = worldSize / 2;
            WalkerNumber = walkerNumber;
            MapMaxX = worldSize.x;
            MapMaxY = worldSize.y;
            _map = new TerrainType[MapMaxX, MapMaxY];
            return true;
        }
        public bool Build()
        {
            if (Map is null)
            {
                return false;
            }
            return ConcreteBuild();
        }
        public abstract bool ConcreteBuild();

        #region setPoints

        protected void SetWallAroundGround()
        {
        }
        protected void FillSquareAroundThePoint(Vector2Int pointPosition, int radius, TerrainType type, bool CalculateInstalls = true)
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

        #endregion

        #region checking
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
        #endregion

        #region correct

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
        protected void CorrectMinMax(ref int nMin, ref int nMax)
        {
            if (nMin > nMax)
            {
                i = nMin;
                nMin = nMax;
                nMax = i;
            }
        }

        #endregion

        #region find

        protected List<Vector2Int> FindAllInRectUnsafe(Vector2Int begin, Vector2Int end, IEnumerable<TerrainType> pointTypes = null)
        {
            return FindAllInRectUnsafe(begin.x, begin.y, end.x, end.y, pointTypes);

        }
        protected List<Vector2Int> FindAllInRectUnsafe(int x1, int y1, int x2, int y2, IEnumerable<TerrainType> pointTypes = null)
        {
            List<Vector2Int> result = new List<Vector2Int>(9);
            CorrectMinMax(ref x1, ref x2);
            CorrectMinMax(ref y1, ref y2);
            bool pointTypesIsEmpty = pointTypes is null || pointTypes.Count() == 0;
            for (i = x1; i <= x2; i++)
            {
                for (j = y1; j <= y2; j++)
                {
                    if (pointTypesIsEmpty ||
                        pointTypes.Contains(_map[i, j]))
                    {
                        result.Add(new Vector2Int(i, j));
                    }
                }
            }
            return result;
        }
        protected IOrderedEnumerable<Vector2Int> FindAllInRadius(Vector2Int centralPosition, int radius, IEnumerable<TerrainType> pointTypes = null)
        {
            if (!IsPointInMap(centralPosition))
            {
                //return new Vector2Int[0].OrderBy(x=>x);
                return (IOrderedEnumerable<Vector2Int>)Enumerable.Empty<Vector2Int>();
            }
            minX = centralPosition.x - radius;
            maxX = centralPosition.x + radius;
            minY = centralPosition.y - radius;
            maxY = centralPosition.y + radius;
            CorrectBorderForMap(ref minX, ref minY);
            CorrectBorderForMap(ref maxX, ref maxY);
            radius *= radius;
            return FindAllInRectUnsafe(minX, minY, maxX, maxY, pointTypes)
                .Where(v => (v - centralPosition).sqrMagnitude <= radius)
                .OrderBy(v => (v - centralPosition).sqrMagnitude);
        }
        protected IOrderedEnumerable<Vector2Int> FindNearest(Vector2Int centralPosition, IEnumerable<TerrainType> pointTypes = null)
        {
            IOrderedEnumerable<Vector2Int>  result;
            j = MapMaxX > MapMaxY ? MapMaxX : MapMaxY;
            j = (int)(j * 1.5f);
            i = 2;
            do
            {
                result = FindAllInRadius(centralPosition, i, pointTypes);
                if (result.Count() > 0)
                {
                    break;
                }
                i *= 2;
            }
            while (i <= j);
            return result;
        }
        protected List<Vector2Int> FindTerrain(Vector2Int startPoint)
        {
            List<Vector2Int> result = new List<Vector2Int>() { startPoint};
            IEnumerable<Vector2Int> newPoints = result;
            IEnumerable<Vector2Int> oldPoints;
            TerrainType[] terrainType = { _map[startPoint.x,startPoint.y] };
            do
            {
                oldPoints = newPoints;
                result = result.Union(oldPoints).ToList();
                foreach (var p in oldPoints)
                {
                    newPoints = newPoints.Union(FindAllInRadius(p, 1, terrainType));
                }
                newPoints = newPoints.Except(result).Distinct().ToArray();
            }
            while (newPoints.Count() > 0);
            return result;
        }
        protected IEnumerable<Vector2Int> FindAllAroundTerrain(Vector2Int startPoint, IEnumerable<TerrainType> pointTypes = null)
        {
            IEnumerable<Vector2Int> result = new Vector2Int[0];
            foreach (Vector2Int point in FindTerrain(startPoint))
            {
                result = result.Union(FindAllInRadius(point, 1, pointTypes));
            }
            return result;
        }

        #endregion

    }
}