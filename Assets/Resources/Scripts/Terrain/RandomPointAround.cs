using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Terrain
{
    public class RandomPointAround : MapGenerator
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

            // Generate
            int random;
            while (PointsInstalls <= CountPoints)
            {
                random = UnityEngine.Random.Range(0, emptyPositions.Count);
                positionNew = emptyPositions[random];
                map[positionNew.x, positionNew.y] = TerrainType.AnyGround;
                emptyPositions.RemoveAt(random);
                emptyPositions = emptyPositions.Union(FindAllInRadius(positionNew, 1, PointTypeEmpty)).ToList();
                            PointsInstalls++;
            }
            return true;
        }
    }
}