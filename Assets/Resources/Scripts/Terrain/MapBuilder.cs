using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileBase = UnityEngine.Tilemaps.TileBase;

namespace Terrain
{
    [DisallowMultipleComponent]
    public class MapBuilder : MonoBehaviour
    {
        [SerializeField] private MapGeneratorEnum _mapGenerator;
        [SerializeField] private int _sizeMap = 10;
        [SerializeField] private int _countTiles = 100;
        [SerializeField] private int _countWalkers=1;
        [SerializeField] private Tilemap _tilemapGround;
        [SerializeField] private List<KeyTile> _groundTiles;
        [SerializeField] private Tilemap _tilemapWall;
        [SerializeField] private List<KeyTile> _wallTiles;
        [SerializeField] private Tilemap _tilemapWallTransparent;
        [SerializeField] private List<KeyTile> _WallTransparentTiles;
        [SerializeField] private Tilemap _tilemapCollider;
        [SerializeField] private List<KeyTile> _colliderTiles;
        [HideInInspector] public IMap Map { get; private set; }
        #region deleteLater
        [SerializeField] private TileBase _tileGround;
        [SerializeField] private TileBase _tileWall;
        //[SerializeField] private TileBase _tileWallTransparent;
        [SerializeField] private TileBase _tileMapGround;
        [SerializeField] private TileBase _tileMapWall;


        private int _tilesLeft;
        private int _i,_j;
        private readonly Vector3Int[] _tablePositions = { Vector3Int.left , Vector3Int.up , Vector3Int.right , Vector3Int.down };
        private readonly List<Vector3Int> _emptyPositions = new(4);
        private int _random;
        private Vector3Int _position;
        private int _sizeSelfMap;
        #endregion
        [Serializable]
        private struct KeyTile
        {
            [field: SerializeField] public TerrainType TerrainType { get; private set; }
            [field: SerializeField] public TileBase Tile { get; private set; }
        }
        private enum MapGeneratorEnum
        {
            RandomPointInCircle,
            Walkers,

            legacy_Walkers,
        }
        private void OnValidate()
        {
            if (_tilemapGround is null ||
                 _tilemapWall is null ||
                 _tilemapCollider is null ||
                 _tilemapWallTransparent is null)
            {
                throw new Exception("Not set Tilemap in MapBuilder");
            }

        }
        private IMapGenerator CreateMapGenerator()
        {
            return _mapGenerator switch
            {
                MapGeneratorEnum.RandomPointInCircle => new RandomPointInCircle(),
                MapGeneratorEnum.legacy_Walkers => new legacy_Walkers(),
                MapGeneratorEnum.Walkers => new Walkers(),
                _ => new RandomPointInCircle(),
            };
        }
        private void Awake()
        {
            #region reliseAndDelite
            //_tilesLeft = _countTiles;
            //_sizeSelfMap = _sizeMap / 2;
            ////RandomPoint1();
            ////SetWallAroundGround();
            //RandomPointAround randomPointAround = new RandomPointAround();
            //Vector3Int correct =- Vector3Int.one* _sizeSelfMap;
            //correct.z = 0;
            ////randomPointAround.Build(Vector2Int.one * _sizeMap, _countTiles, (Vector2Int)(-correct));
            //for (_i = 0; _i < randomPointAround.Map.GetLength(0); _i++)
            //{
            //    for (_j = 0; _j < randomPointAround.Map.GetLength(1); _j++)
            //    {
            //        if (randomPointAround.Map[_i, _j] == TerrainType.AnyGround)
            //        {
            //            _tilemapGround.SetTile(correct + new Vector3Int(_i, _j, 0), _tileGround);
            //        }
            //    }
            //}
            //SetWallAroundGround();
            ////..
            #endregion
            if (!Generate())
            {
                throw new Exception("Map not building!");
            }
            SetTilesInTilemaps();
        }

        private void SetTilesInTilemaps()
        {
            Vector3Int correction = (Vector3Int) (- Map.StartPosition);
            int xCount = Map.Map.GetLength(0);
            int yCount = Map.Map.GetLength(1);
            TerrainType terrainKey;
            int x = 0;
            int y = 0;
            void SetOnTilemap(Tilemap tilemap, List<KeyTile> tiles)
            {
                foreach (KeyTile keyTile in tiles)
                {
                    if (terrainKey == keyTile.TerrainType)
                    {
                        tilemap.SetTile(correction + new Vector3Int(x, y, 0), keyTile.Tile);
                    }

                }
            }
            for (x = 0; x < xCount; x++)
            {
                for (y = 0; y < yCount; y++)
                {
                    terrainKey = Map.Map[x, y];
                    if (terrainKey == TerrainType.Empty)
                    {
                        continue;
                    }
                    SetOnTilemap(_tilemapGround, _groundTiles);
                    SetOnTilemap(_tilemapWall, _wallTiles);
                    SetOnTilemap(_tilemapWallTransparent, _WallTransparentTiles);
                    SetOnTilemap(_tilemapCollider, _colliderTiles);
                }
            }
        }
        private bool Generate()
        {
            IMapGenerator mapGenerator = CreateMapGenerator();
            if (mapGenerator.SetParams(_sizeMap * Vector2Int.one, _countTiles, _countWalkers) &&
             mapGenerator.Build())
            {
                Map = mapGenerator;
            }
            else
            {
                return false;
            }
            return true;
        }

    }
}