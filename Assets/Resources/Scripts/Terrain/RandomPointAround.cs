using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Terrain
{
    public class RandomPointAround : MapGenerator
    {
        private List<Vector2Int> FindAllInRectangularNotSafety(Vector2Int begin, Vector2Int end, List<PointType> pointTypes)
        {
            return FindAllInRectangularNotSafety(begin.x, begin.y, end.x, end.y, pointTypes);

        }
        private List<Vector2Int> FindAllInRectangularNotSafety(int x1, int y1, int x2, int y2, List<PointType> pointTypes)
        {
            List<Vector2Int> result = new List<Vector2Int>(9);
            CorrectMinMax(ref x1, ref x2);
            CorrectMinMax(ref y1, ref y2);
            for (i = x1; i <= x2; i++)
            {
                for (j = y1; j <= y2; j++)
                {
                    if (pointTypes.Contains(_map[i, j]))
                    {
                        result.Add(new Vector2Int(i, j));
                    }
                }
            }
            return result;
        }
        private List<Vector2Int> FindAllInRadius(Vector2Int centralPosition, int radius, List<PointType> pointTypes)
        {
            if (!IsPointInMap(centralPosition))
            {
                return new();
            }
            minX = centralPosition.x - radius;
            maxX = centralPosition.x + radius;
            minY = centralPosition.y - radius;
            maxY = centralPosition.y + radius;
            CorrectBorderForMap(ref minX, ref minY);
            CorrectBorderForMap(ref maxX, ref maxY);
            return FindAllInRectangularNotSafety(minX, minY, maxX, maxY, pointTypes)
                .Where(v => (v - centralPosition).sqrMagnitude <= radius * radius)
                .ToList<Vector2Int>();
        }
        private List<Vector2Int> FindNearestIn
        private List<Vector2Int> FindAllAroundTerrain(Vector2Int startPoint, List<PointType> pointTypes)
        {

            return new();
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