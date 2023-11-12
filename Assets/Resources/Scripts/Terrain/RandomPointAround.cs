using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Terrain
{
    public class RandomPointAround : MapGenerator
    {
        private List<Vector2Int> result = new List<Vector2Int>(9);
        private List<Vector2Int> FindInRadius(Vector2Int centralPosition, int radius, List<PointType> pointTypes)
        {
            result.Clear();
            if (!IsPointInMap(centralPosition))
            {
                return result;
            }
            minX = centralPosition.x - radius;
            maxX = centralPosition.x + radius;
            minY = centralPosition.y - radius;
            maxY = centralPosition.y + radius;
            CorrectBorderForMap(ref minX, ref maxX);
            CorrectBorderForMap(ref minY, ref maxY);
            for (i = minX; i <= maxX; i++)
            {
                for (j = minY; j <= maxY; j++)
                {
                    if (pointTypes.Contains(_map[i, j]))
                    {
                        result.Add(new Vector2Int(i, j));
                    }
                }
            }
            return result;
        }
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