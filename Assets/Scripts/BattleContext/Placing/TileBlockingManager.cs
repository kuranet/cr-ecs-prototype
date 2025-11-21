using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBlockingManager : MonoBehaviour
{
    public static TileBlockingManager Instance;

    private Tilemap tileMap;

    public Dictionary<Vector3Int, int> cellOwnerShip { get; private set; } = new Dictionary<Vector3Int, int>();
    public Dictionary<Vector3Int, int> cellOwnersCount { get; private set; } = new Dictionary<Vector3Int, int>();

    private void Awake()
    {
        var tilemapGO = GameObject.Find("Tilemap");
        if (tilemapGO != null)
            tileMap = tilemapGO.GetComponent<Tilemap>();

        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void AddBuilding(int ownerId, int blockedLength, int blockedWidth, Vector3 worldCenterPos)
    {
        var centerCell = tileMap.WorldToCell(worldCenterPos);
        int halfLength = blockedLength / 2;
        int halfWidth = blockedWidth / 2;
        for (int i = centerCell.x - halfLength; i <= centerCell.x + halfLength; i++)
        {
            for (int j = centerCell.y - halfWidth; j <= centerCell.y + halfWidth; j++)
            {
                Vector3Int cell = new Vector3Int(i, j, 0);
                var walkingTile = tileMap.GetTile(cell) as WalkableTile;

                if (walkingTile != null)
                {
                    if (cellOwnerShip.ContainsKey(cell) == false)
                    {
                        cellOwnerShip.Add(cell, 0);
                    }
                    if (cellOwnersCount.ContainsKey(cell) == false)
                    {
                        cellOwnersCount.Add(cell, 0);
                    }

                    cellOwnerShip[cell] = ownerId;
                    cellOwnersCount[cell]++;
                }
            }
        }
    }

    public void RemoveBuilding(int blockedLength, int blockedWidth, Vector3 worldCenterPos)
    {
        var centerCell = tileMap.WorldToCell(worldCenterPos);
        int halfLength = blockedLength / 2;
        int halfWidth = blockedWidth / 2;
        for (int i = centerCell.x - halfLength; i <= centerCell.x + halfLength; i++)
        {
            for (int j = centerCell.y - halfWidth; j <= centerCell.y + halfWidth; j++)
            {
                Vector3Int cell = new Vector3Int(i, j, 0);
                var walkingTile = tileMap.GetTile(cell) as WalkableTile;

                if (walkingTile != null)
                {
                    if (cellOwnerShip.ContainsKey(cell) == false)
                    {
                        UnityEngine.Debug.LogError($"building removing error at {cell}, cellOwnerShip is empty");
                        continue;
                    }
                    if (cellOwnersCount.ContainsKey(cell) == false)
                    {
                        UnityEngine.Debug.LogError($"building removing error at {cell}, cellOwnersCount is empty");
                        continue;
                    }

                    cellOwnersCount[cell]--;
                    if (cellOwnersCount[cell] <= 0)
                    {
                        cellOwnersCount.Remove(cell);
                        cellOwnerShip.Remove(cell);
                    }
                }
            }
        }
    }
}