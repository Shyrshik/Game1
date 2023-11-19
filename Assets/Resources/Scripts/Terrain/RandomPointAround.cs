using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Terrain
{
    public class RandomPointAround : MapGenerator
    {
        private List<Vector2Int> FindAllInRectUnsafe(Vector2Int begin, Vector2Int end, List<PointType> pointTypes = null)
        {
            return FindAllInRectUnsafe(begin.x, begin.y, end.x, end.y, pointTypes);

        }
        private List<Vector2Int> FindAllInRectUnsafe(int x1, int y1, int x2, int y2, List<PointType> pointTypes = null)
        {
            List<Vector2Int> result = new List<Vector2Int>(9);
            CorrectMinMax(ref x1, ref x2);
            CorrectMinMax(ref y1, ref y2);
            bool pointTypesIsEmpty = pointTypes is null || pointTypes.Count == 0;
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
        private IOrderedEnumerable<Vector2Int> FindAllInRadius(Vector2Int centralPosition, int radius, List<PointType> pointTypes = null)
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
        private IOrderedEnumerable<Vector2Int> FindNearest(Vector2Int centralPosition, List<PointType> pointTypes = null)
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
        private IEnumerable<Vector2Int> FindTerrain(Vector2Int startPoint)
        {
            IEnumerable<Vector2Int> result = new Vector2Int[] { startPoint};
            List<Vector2Int> newPoints = new List<Vector2Int>(16) {startPoint};
            List<PointType> terrainType = new List<PointType>(1){ _map[startPoint.x,startPoint.y] };
            Vector2Int point;
            do
            {
                point = newPoints[^1];
                newPoints.Union(FindAllInRadius(point, 1, terrainType).Except(result));
                result.Union(newPoints);
                newPoints.Remove(point);
            }
            while (newPoints.Count > 0);
            return result;
        }
        private IEnumerable<Vector2Int> FindAllAroundTerrain(Vector2Int startPoint, List<PointType> pointTypes = null)
        {
            IEnumerable<Vector2Int> result = new Vector2Int[0]; 
            //FindTerrain(startPoint)
            //    .Select(x=> FindAllInRadius(x,1,pointTypes))
                foreach (Vector2Int point in FindTerrain(startPoint))
            {
                result.Union(FindAllInRadius(point, 1, pointTypes));
            }
            return result;
            //IEnumerable<Vector2Int> allAroundPoint;
            //List<Vector2Int> allAroundTerrain = new List<Vector2Int>(16);
            //PointType terrainType = _map[startPoint.x,startPoint.y];
            //List<Vector2Int> allTerrain = new List<Vector2Int>(16);
            //List<Vector2Int> newPoints = new List<Vector2Int>(16);
            //allTerrain.Add(startPoint);
            //newPoints.Add(startPoint);
            //Vector2Int point;
            //while (newPoints.Count > 0)
            //{
            //    point = newPoints[^1];
            //    newPoints.Remove(point);

            //    allAroundPoint = FindAllInRadius(point, 1);
            //    allAroundTerrain.AddRange(allAroundPoint.FindAll(v => pointTypes.Contains(_map[v.x, v.y])).ToArray());
            //    allAroundTerrain = allAroundTerrain.Distinct().ToList();
            //    allTerrain.AddRange(allAroundPoint.FindAll(v => _map[v.x, v.y] == terrainType).ToArray());
            //    allTerrain = allTerrain.Distinct().ToList();

            //}
            //return Enumerable.Empty<Vector2Int>();
        }
        public override void Build(Vector2Int worldSize, int countPoints, Vector2Int startPosition)
        {
            // generates a circle with blurred edges
            //caching
            _startPosition = startPosition;
            MapMaxX = worldSize.x;
            MapMaxY = worldSize.y;
            countPoints = CountPoints;
            _map = new PointType[MapMaxX, MapMaxY];
            Vector3Int positionNew;
            List< Vector3Int> emptyPositions = new(countPoints*2);

            //Set start platform.
            FillSquareAroundThePoint(startPosition, 1, PointType.AnyGround, true);
            emptyPositions.Add(new(-1, -2, 0));
            emptyPositions.Add(new(0, -2, 0));
            emptyPositions.Add(new(1, -2, 0));
            emptyPositions.Add(new(-1, 2, 0));
            emptyPositions.Add(new(0, 2, 0));
            emptyPositions.Add(new(1, 2, 0));
            emptyPositions.Add(new(-2, -1, 0));
            emptyPositions.Add(new(-2, 0, 0));
            emptyPositions.Add(new(-2, 1, 0));
            emptyPositions.Add(new(2, -1, 0));
            emptyPositions.Add(new(2, 0, 0));
            emptyPositions.Add(new(2, 1, 0));

            // Generate
            while (_tilesLeft > 0)
            {
                _random = UnityEngine.Random.Range(0, emptyPositions.Count);
                positionNew = emptyPositions[_random];
                _tilemapGround.SetTile(positionNew, _tileGround);
                emptyPositions.RemoveAt(_random);

                for (_i = 0; _i < _tablePositions.Length; _i++)
                {
                    _position = _tablePositions[_i] + positionNew;
                    if (_tilemapGround.GetTile(_position) is null &&
                        !emptyPositions.Contains(_position))
                    {
                        emptyPositions.Add(_position);
                    }
                }
                _tilesLeft--;
            }
        }
    }
}