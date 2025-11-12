using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom Tiles/Walkable Tile")]
public class WalkableTile : Tile
{
    public bool isWalkable = true;
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        // For example, tint unwalkable tiles red in the editor
        if (!isWalkable)
            tileData.color = Color.red;
    }
}
