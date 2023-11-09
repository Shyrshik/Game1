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
    [SerializeField] private int _tilesLeft;
    private void Awake()
    {
        _tilesLeft = _countTiles;
        SimpleRandom();
    }
    private void MazeWalkers()
    {
        //caching
        int i,x,y;
        int sizeMap = _sizeMap / 2;
        Vector3Int startPosition = new(0, 0, 0);
        Vector3Int positionOld = startPosition;
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

            _tilesLeft--;
        }
        for (x = 0; x < 5; x++)
        {
            positionWalker = new(0, 0, 0);
            for (y = 0; y < _countTiles / 4; y++)
            {
                float random = UnityEngine.Random.Range(0f, 4f);
                positionOld = positionWalker;

                if (random < 1)
                {
                    positionWalker += Vector3Int.left;
                }
                else if (random < 2)
                {
                    positionWalker += Vector3Int.up;
                }
                else if (random < 3)
                {
                    positionWalker += Vector3Int.right;
                }
                else
                {
                    positionWalker += Vector3Int.down;
                }

                if (math.abs(positionWalker.x) < sizeMap && math.abs(positionWalker.y) < sizeMap)
                    _tilemapGround.SetTile(positionWalker, _tileGround);
                else
                    positionWalker = positionOld;
            }
        }
        TileBase tileFind;
        for (positionWalker.x = -sizeMap - 1; positionWalker.x < sizeMap + 1; positionWalker.x++)
        {
            for (positionWalker.y = -sizeMap - 1; positionWalker.y < sizeMap + 1; positionWalker.y++)
            {
                tileFind = _tilemapGround.GetTile(positionWalker);
                if (tileFind == null)
                {
                    continue;
                }
                else
                    _tilemapCollider.SetTile(positionWalker, _tileMapGround);
                positionOld = positionWalker;
                int x1 = positionWalker.x + 2;
                int y1 = positionWalker.y + 2;
                for (positionWalker.x = positionOld.x - 1; positionWalker.x < x1; positionWalker.x++)
                {

                    for (positionWalker.y = positionOld.y - 1; positionWalker.y < y1; positionWalker.y++)
                    {
                        if (_tilemapGround.GetTile(positionWalker) == null)
                        {
                            _tilemapWall.SetTile(positionWalker, _tileWall);
                            _tilemapWallTransparent.SetTile(positionWalker, _tileWall);
                            _tilemapCollider.SetTile(positionWalker, _tileMapWall);
                        }
                    }
                }
                positionWalker = positionOld;
            }
        }
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
