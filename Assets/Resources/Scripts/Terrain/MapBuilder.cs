using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileBase = UnityEngine.Tilemaps.TileBase;
[DisallowMultipleComponent]
public class MapBuilder : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemapGround;
    [SerializeField] private Tilemap _tilemapWall;
    [SerializeField] private Tilemap _tilemapWallTransparent;
    [SerializeField] private Tilemap _tilemapCollider;
    [SerializeField] private TileBase _tileGround;
    [SerializeField] private TileBase _tileWall;
    //[SerializeField] private TileBase _tileWallTransparent;
    [SerializeField] private TileBase _tileMapGround;
    [SerializeField] private TileBase _tileMapWall;
    [SerializeField] private int _sizeMap = 10;
    [SerializeField] private int _countTiles = 100;
    [SerializeField] private int _countWalkers=1;
    private int _tilesLeft;
    private int _i,_j;
    private readonly Vector3Int[] _tablePositions = { Vector3Int.left , Vector3Int.up , Vector3Int.right , Vector3Int.down };
    private readonly List<Vector3Int> _emptyPositions = new(4);
    private int _random;
    private Vector3Int _position;
    private int _sizeSelfMap;
    private void Awake()
    {
        _tilesLeft = _countTiles;
        _sizeSelfMap = _sizeMap / 2;
        RandomPoint();
        SetWallAroundGround();
    }
    private void RandomPoint()
    {
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
            _random = UnityEngine.Random.Range(0, _emptyPositions.Count);
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
    #region Legasi
    private void SimpleRandom()
    {

        int sizeMap = _sizeSelfMap / 2;
        //_tileWall..hideFlags = HideFlags.None

        //// генерируем карту
        Vector3Int position = new(0, 0, 0);

        _tilemapGround.SetTile(position, _tileGround);
        // создаем площадку 3х3 вокруг персонажа, может там что то будем класть при старте игры
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                _tilemapGround.SetTile(new(x, y, 0), _tileGround);
            }
        }

        Vector3Int positionOld;
        // Случайно набрасываем блоки
        for (int x = 0; x < 5; x++)
        {
            position = new(0, 0, 0);
            for (int y = 0; y < _countTiles / 4; y++)
            {
                float random = UnityEngine.Random.Range(0f, 4f);
                positionOld = position;

                if (random < 1)
                {
                    position += Vector3Int.left;
                }
                else if (random < 2)
                {
                    position += Vector3Int.up;
                }
                else if (random < 3)
                {
                    position += Vector3Int.right;
                }
                else
                {
                    position += Vector3Int.down;
                }

                if (math.abs(position.x) < sizeMap && math.abs(position.y) < sizeMap)
                    _tilemapGround.SetTile(position, _tileGround);
                else
                    position = positionOld;
            }
        }
        TileBase tileFind;
        for (position.x = -sizeMap - 1; position.x < sizeMap + 1; position.x++)
        {
            for (position.y = -sizeMap - 1; position.y < sizeMap + 1; position.y++)
            {
                tileFind = _tilemapGround.GetTile(position);
                if (tileFind == null)
                {
                    continue;
                }
                else
                    _tilemapCollider.SetTile(position, _tileMapGround);
                positionOld = position;
                int x1 = position.x + 2;
                int y1 = position.y + 2;
                for (position.x = positionOld.x - 1; position.x < x1; position.x++)
                {

                    for (position.y = positionOld.y - 1; position.y < y1; position.y++)
                    {
                        if (_tilemapGround.GetTile(position) == null)
                        {
                            _tilemapWall.SetTile(position, _tileWall);
                            _tilemapWallTransparent.SetTile(position, _tileWall);
                            _tilemapCollider.SetTile(position, _tileMapWall);
                        }
                    }
                }
                position = positionOld;
            }
        }
    }
    #endregion
}
