using Unity.Entities;
using UnityEngine;
using UnityEngine.Tilemaps;

public partial class TileSelectingSystem : SystemBase
{
    private Tilemap _tilemap;
    protected override void OnCreate()
    {
        var tilemapGO = GameObject.Find("Tilemap");
        if (tilemapGO != null)
            _tilemap = tilemapGO.GetComponent<Tilemap>();

        RequireForUpdate<BattleState>();

        base.OnCreate();
    }

    protected override void OnUpdate()
    {
        if (!Application.isFocused)
            return;

        if (Input.GetMouseButton(0) == false || canProcessInput() == false)
        {
            TileMapSingleton.HasSelectedCell = false;
            return;
        }

        Vector3 worldPos = CameraManager.GetCameraOrientedPos();

        Vector3Int cell = _tilemap.WorldToCell(worldPos);
        var tile = _tilemap.GetTile(cell);

        if (!(tile is WalkableTile))
        { 
            TileMapSingleton.HasSelectedCell = false; 
            return; 
        }

        if (TileBlockingManager.Instance.cellOwnerShip.ContainsKey(cell) && TileBlockingManager.Instance.cellOwnerShip[cell] != 0)
        {
            TileMapSingleton.HasSelectedCell = false;
            return;
        }

        TileMapSingleton.HasSelectedCell = true;
        TileMapSingleton.SelectedCell = cell;
    }

    private bool canProcessInput()
    {
        return SystemAPI.GetSingleton<BattleState>().isStateValidForPlacingUnits();
    }
}
