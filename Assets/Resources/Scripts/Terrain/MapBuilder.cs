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
    [SerializeField] private int _CountWalkers;
    private int _tilesLeft;
    private int i,j, x,y;
    private Vector3Int[] tablePositions = { Vector3Int.left , Vector3Int.up , Vector3Int.right , Vector3Int.down };
    private List<Vector3Int> EmptyPositions = new List<Vector3Int>(4);
    private int random;
    private Vector3Int position;
    int sizeMap;
    private void Awake()
    {
        _tilesLeft = _countTiles;
        int sizeMap = _sizeMap / 2;
        SimpleRandom();
    }
    private void MazeWalkers()
    {
        //caching
        Vector3Int startPosition = new(0, 0, 0);
        Vector3Int positionNew = startPosition;
        int walkerNumber = 0;
        List< Vector3Int> positionWalker = new(_CountWalkers);
        for (i = 0; i < _CountWalkers; i++)
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
            _tilesLeft--;
        }
    }
    private void SetWallArroundGround() 
    {
        position = new();
        Vector3Int positionNew;
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
                positionNew = position;
                int x1 = position.x + 2;
                int y1 = position.y + 2;
                for (position.x = positionNew.x - 1; position.x < x1; position.x++)
                {

                    for (position.y = positionNew.y - 1; position.y < y1; position.y++)
                    {
                        if (_tilemapGround.GetTile(position) == null)
                        {
                            _tilemapWall.SetTile(position, _tileWall);
                            _tilemapWallTransparent.SetTile(position, _tileWall);
                            _tilemapCollider.SetTile(position, _tileMapWall);
                        }
                    }
                }
                position = positionNew;
            }
        }
    }
    
    private Vector3Int GetEmptyTitleSquareMethod(Vector3Int point)
    {
        EmptyPositions.Clear();
        for (i = 0; i < 4; i++)
        {
            position = tablePositions[i] + point;
            if (_tilemapGround.GetTile(position) is null)
            {
                EmptyPositions.Add(position);
            }
        }
        if (EmptyPositions.Count == 1)
        {
            return EmptyPositions[0];
        }
        else if (EmptyPositions.Count > 1)
        {
            random = UnityEngine.Random.Range(0, EmptyPositions.Count);
            return EmptyPositions[random];
        }
        for (j = 1; j < _countTiles; j++)
        {
            for (i = -j; i < j; i++)
            {
                position = point + Vector3Int.left * j;
                position.y = i;
                if (_tilemapGround.GetTile(position) is null)
                {
                    return position;
                }
                position = -position;
                if (_tilemapGround.GetTile(position) is null)
                {
                    return position;
                }
                position = point + Vector3Int.down * j;
                position.x = i;
                if (_tilemapGround.GetTile(position) is null)
                {
                    return position;
                }
                position = -position;
                if (_tilemapGround.GetTile(position) is null)
                {
                    return position;
                }
            }
        }
        return new();
    }


    private void SimpleRandom()
    {

        int sizeMap = _sizeMap / 2;
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
}
