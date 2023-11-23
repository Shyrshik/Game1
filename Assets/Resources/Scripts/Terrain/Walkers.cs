using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Terrain
{
    public class Walkers : MapGenerator
    {
        public override bool ConcreteBuild()
        {
            // generates a circle with blurred edges
            //caching

            TerrainType[,] map = Map;
            Vector2Int positionNew;
            List< List< Vector2Int>> emptyPositions = new(WalkerNumber);
            for(int i = 0; i < WalkerNumber;i++)
            {
                emptyPositions.Add(new List<Vector2Int>());
            }
            TerrainType[] PointTypeEmpty = new TerrainType[]{TerrainType.Empty};

            //Set start platform.
            FillSquareAroundThePoint(StartPosition, 1, TerrainType.AnyGround, true);
            //emptyPositions = emptyPositions.Where(n => IsPointInMapAndNotInBorder(n)).ToList();
            // Generate
            int random;
            while (PointsInstalls < CountPoints)
            {
                foreach (var walker in emptyPositions)
                {
                    if (walker.Count == 0)
                    {
                        walker.AddRange(FindNearest(StartPosition, PointTypeEmpty)
                            .Where(n => IsPointInMapAndNotInBorder(n)));
                        if (walker.Count == 0) break;
                    }
                    random = UnityEngine.Random.Range(0, walker.Count);
                    positionNew = walker[random];
                    if (map[positionNew.x, positionNew.y] == TerrainType.AnyGround)
                    {
                        walker.Remove(positionNew);
                        continue;
                    }
                    map[positionNew.x, positionNew.y] = TerrainType.AnyGround;
                    PointsInstalls++;
                    walker.Clear();
                    walker.AddRange(FindAllInRadius(positionNew, 1, PointTypeEmpty)
                        .Where(n => IsPointInMapAndNotInBorder(n)));
                    walker.Remove(positionNew);
                    if (walker.Count == 0)
                    {
                        walker.AddRange(FindNearest(positionNew, PointTypeEmpty)
                            .Where(n => IsPointInMapAndNotInBorder(n)));
                    }
                }
            }
            SetPointAroundAllAnotherPointsInMap(TerrainType.AnyWall, new TerrainType[] { TerrainType.Empty },
                new TerrainType[] { TerrainType.AnyGround });
            return true;
        }
    }
}