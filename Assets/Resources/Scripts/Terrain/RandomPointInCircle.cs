using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Terrain
{
    public class RandomPointInCircle : MapGenerator
    {
        public override bool ConcreteBuild()
        {
            // generates a circle with blurred edges
            //caching

            TerrainType[,] map = Map;
            Vector2Int positionNew;
            List< Vector2Int> emptyPositions = new(CountPoints);
            TerrainType[] PointTypeEmpty = { TerrainType.Empty };

            //Set start platform.
            FillSquareAroundThePoint(StartPosition, 1, TerrainType.AnyGround, true);
            emptyPositions.AddRange(FindAllAroundTerrain(StartPosition, PointTypeEmpty));
            emptyPositions = emptyPositions.Where(n => IsPointInMapAndNotInBorder(n)).ToList();
            // Generate
            int random;
            while (PointsInstalls < CountPoints)
            {
                random = UnityEngine.Random.Range(0, emptyPositions.Count);
                positionNew = emptyPositions[random];
                map[positionNew.x, positionNew.y] = TerrainType.AnyGround;
                emptyPositions.RemoveAt(random);
                emptyPositions = emptyPositions.Union(FindAllInRadius(positionNew, 1, PointTypeEmpty))
                    .Where(n => IsPointInMapAndNotInBorder(n)).ToList();
                PointsInstalls++;
                if (emptyPositions.Count<1)
                {
                    break;
                }
            }
            SetPointAroundAllAnotherPointsInMap(TerrainType.AnyWall, new TerrainType[] { TerrainType.Empty },
                new TerrainType[] { TerrainType.AnyGround });
            return true;
        }
            }
}