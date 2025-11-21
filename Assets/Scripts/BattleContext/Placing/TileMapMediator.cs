using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapMediator : MonoBehaviour
{
    [SerializeField] private GameObject _tileSelectingPrefab;
    [SerializeField] private Tilemap _tilemap;

    private Vector3 _tileOffset = new Vector3(0.5f, 0, 0.5f);

    void Update()
    {
        if (TileMapSingleton.HasSelectedCell == false)
        {
            _tileSelectingPrefab.SetActive(false);
            return;
        }

        _tileSelectingPrefab.SetActive(true);
        _tileSelectingPrefab.transform.position = _tilemap.CellToWorld(TileMapSingleton.SelectedCell) + _tileOffset;
    }
}
