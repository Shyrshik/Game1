using System;
using System.Collections.Generic;
using Unity.Mathematics;
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

        private IMapGenerator CreateMapGenerator()
        {
            return _mapGenerator switch
            {
                MapGeneratorEnum.RandomPointInCircle => new RandomPointInCircle(),
                MapGeneratorEnum.legacy_Walkers => new legacy_Walkers(),
                _ => new RandomPointInCircle(),
            };

        }

        #region realiseAndDelete
        private void RandomPoint1()
        {
            // 
            //caching
            Vector3Int startPosition = new(0, 0, 0);
            Vector3Int positionNew;
            int walkerNumber = 0;
            List< Vector3Int> emptyPositions = new(_countWalkers*2);

            //Set start platform.
            FillSquareAroundThePoint(startPosition, 1);
            List< Vector3Int> startPositions = new(12);
            startPositions.Add(new(-1, -2, 0));
            startPositions.Add(new(0, -2, 0));
            startPositions.Add(new(1, -2, 0));
            startPositions.Add(new(-1, 2, 0));
            startPositions.Add(new(0, 2, 0));
            startPositions.Add(new(1, 2, 0));
            startPositions.Add(new(-2, -1, 0));
            startPositions.Add(new(-2, 0, 0));
            startPositions.Add(new(-2, 1, 0));
            startPositions.Add(new(2, -1, 0));
            startPositions.Add(new(2, 0, 0));
            startPositions.Add(new(2, 1, 0));
            if (startPositions.Count >= emptyPositions.Capacity)
            {
                _j = emptyPositions.Capacity;
            }
            else
            {
                _j = startPositions.Count;
            }
            for (_i = 0; _i < _j; _i++)
            {
                _random = UnityEngine.Random.Range(0, startPositions.Count);
                emptyPositions.Add(startPositions[_random]);
                startPositions.RemoveAt(_random);
            }

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
                if (emptyPositions.Count > _countWalkers)
                {
                    emptyPositions.RemoveRange(0, emptyPositions.Count - _countWalkers);
                }
                _tilesLeft--;
            }
        }
        private void RandomPoint()
        {
            // generates a circle with blurred edges
            //caching
            Vector3Int startPosition = new(0, 0, 0);
            Vector3Int positionNew;
            int walkerNumber = 0;
            List< Vector3Int> emptyPositions = new(_countTiles*2);

            //Set start platform.
            FillSquareAroundThePoint(startPosition, 1);
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

        #region Walkers
        private void MazeWalkers()
        {
            //caching
            Vector3Int startPosition = new(0, 0, 0);
            Vector3Int positionNew;
            int walkerNumber = 0;
            List< Vector3Int> positionWalker = new(_countWalkers);
            for (_i = 0; _i < _countWalkers; _i++)
            {
                positionWalker.Add(startPosition);
            }
            //Set start platform.
            FillSquareAroundThePoint(startPosition, 1);

            // Generate
            while (_tilesLeft > 0)
            {
                walkerNumber = ++walkerNumber % positionWalker.Count;
                positionNew = positionWalker[walkerNumber];
                positionNew = GetEmptyTitleSquareMethod(positionNew);
                //if (math.abs(positionWalker[walkerNumber].x) < sizeMap || math.abs(positionWalker[walkerNumber].y) < sizeMap)
                //{

                //}
                _tilemapGround.SetTile(positionNew, _tileGround);
                positionWalker[walkerNumber] = positionNew;
                _tilesLeft--;
            }
        }
        private void SetWallAroundGround()
        {
            _position = new();
            Vector3Int positionNew;
            TileBase tileFind;
            for (_position.x = -_sizeSelfMap - 1; _position.x < _sizeSelfMap + 1; _position.x++)
            {
                for (_position.y = -_sizeSelfMap - 1; _position.y < _sizeSelfMap + 1; _position.y++)
                {
                    tileFind = _tilemapGround.GetTile(_position);
                    if (tileFind == null)
                    {
                        continue;
                    }
                    else
                        _tilemapCollider.SetTile(_position, _tileMapGround);
                    positionNew = _position;
                    int x1 = _position.x + 2;
                    int y1 = _position.y + 2;
                    for (_position.x = positionNew.x - 1; _position.x < x1; _position.x++)
                    {

                        for (_position.y = positionNew.y - 1; _position.y < y1; _position.y++)
                        {
                            if (_tilemapGround.GetTile(_position) == null)
                            {
                                _tilemapWall.SetTile(_position, _tileWall);
                                _tilemapWallTransparent.SetTile(_position, _tileWall);
                                _tilemapCollider.SetTile(_position, _tileMapWall);
                            }
                        }
                    }
                    _position = positionNew;
                }
            }
        }

        private Vector3Int GetEmptyTitleSquareMethod(Vector3Int point)
        {
            _emptyPositions.Clear();
            for (_i = 0; _i < 4; _i++)
            {
                _position = _tablePositions[_i] + point;
                if (_tilemapGround.GetTile(_position) is null)
                {
                    _emptyPositions.Add(_position);
                }
            }
            if (_emptyPositions.Count == 1)
            {
                return _emptyPositions[0];
            }
            else if (_emptyPositions.Count > 1)
            {
                _random = UnityEngine.Random.Range(0, _emptyPositions.Count);
                return _emptyPositions[_random];
            }
            for (_j = 1; _j < _countTiles; _j++)
            {
                for (_i = -_j; _i < _j; _i++)
                {
                    _position = point + Vector3Int.left * _j;
                    _position.y = _i;
                    if (_tilemapGround.GetTile(_position) is null)
                    {
                        return _position;
                    }
                    _position = -_position;
                    if (_tilemapGround.GetTile(_position) is null)
                    {
                        return _position;
                    }
                    _position = point + Vector3Int.down * _j;
                    _position.x = _i;
                    if (_tilemapGround.GetTile(_position) is null)
                    {
                        return _position;
                    }
                    _position = -_position;
                    if (_tilemapGround.GetTile(_position) is null)
                    {
                        return _position;
                    }
                }
            }
            return new();
        }

        private void FillSquareAroundThePoint(Vector3Int pointPosition, int radius)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    _tilemapGround.SetTile(new(x, y, pointPosition.z), _tileGround);
                    _tilesLeft--;
                }
            }
        }
        #endregion
       
        #endregion
    }
}