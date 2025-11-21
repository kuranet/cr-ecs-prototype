using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom Tiles/Walkable Tile")]
public class WalkableTile : Tile
{
    public bool isWalkable = true;

    public bool isBlockedByPlayer = false;
    public int blokerPlayerId = 0;
    public int buildingsOnThisTile = 0;
}
