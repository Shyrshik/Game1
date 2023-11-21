using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Terrain
{
    public class RandomPointAround : MapGenerator
    {
        private List<Vector2Int> FindAllInRectUnsafe(Vector2Int begin, Vector2Int end, IEnumerable<PointType> pointTypes = null)
        {
            return FindAllInRectUnsafe(begin.x, begin.y, end.x, end.y, pointTypes);

        }
        private List<Vector2Int> FindAllInRectUnsafe(int x1, int y1, int x2, int y2, IEnumerable<PointType> pointTypes = null)
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
        private IOrderedEnumerable<Vector2Int> FindAllInRadius(Vector2Int centralPosition, int radius, IEnumerable<PointType> pointTypes = null)
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
        private IOrderedEnumerable<Vector2Int> FindNearest(Vector2Int centralPosition, IEnumerable<PointType> pointTypes = null)
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
        private List<Vector2Int> FindTerrain(Vector2Int startPoint)
        {
            List<Vector2Int> result = new List<Vector2Int>() { startPoint};
            IEnumerable<Vector2Int> newPoints = result;
            IEnumerable<Vector2Int> oldPoints;
            PointType[] terrainType = { _map[startPoint.x,startPoint.y] };
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
        private IEnumerable<Vector2Int> FindAllAroundTerrain(Vector2Int startPoint, IEnumerable<PointType> pointTypes = null)
        {
            IEnumerable<Vector2Int> result = new Vector2Int[0];
            foreach (Vector2Int point in FindTerrain(startPoint))
            {
                result = result.Union(FindAllInRadius(point, 1, pointTypes));
            }
            return result;
        }
        public override void Build(Vector2Int worldSize, int countPoints, Vector2Int startPosition)
        {
            // generates a circle with blurred edges
            //caching
            _startPosition = startPosition;
            MapMaxX = worldSize.x;
            MapMaxY = worldSize.y;
            CountPoints = countPoints;
            _map = new PointType[MapMaxX, MapMaxY];
            Vector2Int positionNew;
            List< Vector2Int> emptyPositions = new(countPoints*2);
            PointType[] PointTypeEmpty = { PointType.Empty };

            //Set start platform.
            FillSquareAroundThePoint(startPosition, 1, PointType.AnyGround, true);
            emptyPositions.AddRange(FindAllAroundTerrain(startPosition, PointTypeEmpty));

            // Generate
            int random;
            while (PointsInstalls <= countPoints)
            {
                random = UnityEngine.Random.Range(0, emptyPositions.Count);
                positionNew = emptyPositions[random];
                _map[positionNew.x, positionNew.y] = PointType.AnyGround;
                //_tilemapGround.SetTile(positionNew, _tileGround);
                emptyPositions.RemoveAt(random);
                emptyPositions = emptyPositions.Union(FindAllInRadius(positionNew, 1, PointTypeEmpty)).ToList();
                //for (i = 0; i < _tablePositions.Length; i++)
                //{
                //    _position = _tablePositions[i] + positionNew;
                //    if (_tilemapGround.GetTile(_position) is null &&
                //        !emptyPositions.Contains(_position))
                //    {
                //        emptyPositions.Add(_position);
                //    }
                //}
                PointsInstalls++;
            }
        }
    }
}