namespace Terrain
{
    public interface IMapGenerator
    {
        PointType[,] Map { get; }
    }
}