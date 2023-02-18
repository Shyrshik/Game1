using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapAlpha : MonoBehaviour
{
    private Tilemap _tilemap;
    private Vector3Int _positionCollision;
    private Vector3Int _position;
    private Collider2D _collision;
    void Start()
    {
        _tilemap = GetComponent<Tilemap>();
    }
    private void OnInvoke() => OnTriggerEnter2D(_collision);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _positionCollision = _tilemap.WorldToCell(collision.transform.position);
        _position = new Vector3Int();
        for (_position.x = _positionCollision.x - 3; _position.x <= _positionCollision.x; _position.x++)
        {
            for (_position.y = _positionCollision.y - 3; _position.y <= _positionCollision.y; _position.y++)
            {
                //_tilemap.SetTileFlags(_position, TileFlags.None);
                _tilemap.SetColor(_position, new Color(1f, 1f, 1f, 0.5f));
            }
        }
        _collision = collision;
        Invoke(nameof(OnInvoke), 0.3f);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsInvoking(nameof(OnTriggerEnter2D)))
            CancelInvoke(nameof(OnTriggerEnter2D));
        //_tilemap.ClearAllEditorPreviewTiles();
    }
}
