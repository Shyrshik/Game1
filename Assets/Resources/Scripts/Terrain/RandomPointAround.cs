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
            List<Vector2Int> newPoints = new List<Vector2Int>(1) {startPoint};
            List<PointType> terrainType = new List<PointType>(1){ _map[startPoint.x,startPoint.y] };
            Vector2Int point;

            #region debag1
            List<Vector2Int> l1;
            List<Vector2Int> l2;
            List<Vector2Int>l3;
            List<Vector2Int>l4;
            List<Vector2Int>l5;
            List<Vector2Int>l6;
            List<Vector2Int>l7;
            List<Vector2Int>l8;
            List<Vector2Int>l9;
            List<Vector2Int>FAIR;
            List < Vector2Int >EXRez;
            int ee =0;
            #endregion
            int index;
            do
            {
                //point = newPoints.Last();
                //newPoints = newPoints.Union(FindAllInRadius(point, 1, terrainType).Except(result));
                //result = result.Union(newPoints);
                //newPoints = newPoints.Where(x => x != point);
                #region debag1
                index = newPoints.Count-1;
                point = newPoints[index];
                l8 = result.ToList();
                l1 = newPoints.ToList();


                FAIR= FindAllInRadius(point, 1, terrainType).ToList();
                l5 = result.ToList();
                EXRez = FindAllInRadius(point, 1, terrainType).Except(result).ToList();
                newPoints.AddRange(FindAllInRadius(point, 1, terrainType).Except(result));
                 l2 = newPoints.ToList();
                l6 = result.ToList();

                result = result.Union(newPoints);
                l3 = result.ToList();

                newPoints.RemoveAt(index);
                l4 = newPoints.ToList(); ;
                l7 = result.ToList();
                if (ee > 100)
                    break;
                ee++;
                #endregion
            }
            while (newPoints.Count() > 0);
            return result;
        }
        private IEnumerable<Vector2Int> FindAllAroundTerrain(Vector2Int startPoint, List<PointType> pointTypes = null)
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
            List<PointType> PointTypeEmpty = new List<PointType>(1) { PointType.Empty };

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
                emptyPositions.Union(FindAllInRadius(positionNew, 1, PointTypeEmpty));
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