using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _camera;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private GameObject _prefabEnemy;
    private float _timePrefab;
    public float _timeBotSpawn = 1f;
    private InputСontroller _inputController;
    private GameObject _allEnemies;
    private void Awake()
    {
        // переносим персонажа с камерой в начало координат, игра всегда будет начинаться отсюда
        Vector3 startPosition = new(0f, 0f, 0f);
        _player.transform.position = startPosition;
        startPosition.y -= 10;
        startPosition.z -= 6;
        _camera.transform.position = startPosition;

        _allEnemies = new GameObject("AllEnemies");
        Invoke(nameof(PostEnemyInvoke), _timeBotSpawn);
        _allEnemies.AddComponent<EnemiesSpawner>();
    }
    private void PostEnemyInvoke()
    {

        PostEnemy();
        Invoke(nameof(PostEnemyInvoke), _timeBotSpawn);
    }
    public void PostEnemy(GameObject postObject = null)
    {
        if (postObject != null)
        {
            TileBase tileForSpawn = null;
            while (tileForSpawn == null)
            {
                float randomX = Random.Range(-5f, 5f);
                float randomY = Random.Range(-5f, 5f);
                randomX += _player.transform.position.x;
                randomY += _player.transform.position.y;
                Vector3 vectorInstantiate = new(randomX, randomY, 0f);
                Vector3Int vectorInstantiateInt = _tilemap.WorldToCell(vectorInstantiate);
                tileForSpawn = _tilemap.GetTile(vectorInstantiateInt);
                if (tileForSpawn != null)
                    postObject.transform.position = vectorInstantiate;
            }
        }
        if (_timePrefab < Time.time)
        {

            _timeBotSpawn = _timeBotSpawn < 0.1f ? 0.1f : _timeBotSpawn;
            _timePrefab = Time.time + _timeBotSpawn;
            if (_timeBotSpawn > 0.2)
                _timeBotSpawn -= 0.01f;
            float randomX = Random.Range(-5f, 5f);
            float randomY = Random.Range(-5f, 5f);
            randomX += _player.transform.position.x;
            randomY += _player.transform.position.y;
            Vector3 vectorInstantiate = new(randomX, randomY, 0f);
            Vector3Int vectorInstantiateInt = _tilemap.WorldToCell(vectorInstantiate);
            TileBase tileForSpawn = _tilemap.GetTile(vectorInstantiateInt);
            if (tileForSpawn != null)
            {
                Instantiate(_prefabEnemy, vectorInstantiate, _player.transform.rotation, _allEnemies.transform);
                EventManager.SendEnemyCount(1f);
            }
        }
    }
    
    public void LoadScene(int number)
    {
        SceneManager.LoadScene(number);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
