using UnityEngine;

namespace Terrain
{
    public interface IMapGenerator : IMap
    {
        public bool SetParams(Vector2Int worldSize, int countPoints, int walkerNumber = 1);
        public bool Build();
    }
}
