using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileBase = UnityEngine.Tilemaps.TileBase;
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
    private void Awake()
    {
        int sizeMap = _sizeMap / 2;
        //_tileWall..hideFlags = HideFlags.None

        //// генерируем карту
        Vector3Int position = new(0, 0, 0);
        
        _tilemapGround.SetTile(position, _tileGround);
        // создаем площадку 3х3 вокруг персонажа, может там что то будем ложить при старте игры
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                _tilemapGround.SetTile(new(x, y, 0), _tileGround);
            }
        }

        Vector3Int positionOld;
        // рандомно набрасываем блоки
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
