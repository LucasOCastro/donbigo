using DonBigo;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


[CustomEditor(typeof(Bigodon))]
public class EntityEditor : Editor
{
    private static int[] RectIndices = new[]
    {
        0, 1,
        1, 2,
        2, 3,
        3, 0
    };
    
    private void OnSceneGUI()
    {
        if (!Application.isPlaying) return;
        if (target is not Entity entity) return;
        
        var tilemap = FindObjectOfType<Tilemap>();
        if (!tilemap) return;

        var entityPos = entity.Tile.Pos;
        Handles.color = Color.yellow;
        UtilVec2Int.FloodFill(
            entityPos,
            v =>
            {
                Vector2Int difVec = (v - entityPos).Abs() * 10;
                return difVec.x <= entity.VisionRange && difVec.y <= entity.VisionRange;
            },//v.ManhattanDistance(entityPos) <= entity.VisionRange,
            v =>
            {
                if (v == entityPos) Handles.color = Color.blue;
                Handles.DrawLines(GenCellRect(v, tilemap), RectIndices);
                if (v == entityPos) Handles.color = Color.yellow;
            }
        );
        Handles.color = Color.blue;
        Handles.DrawLines(GenCellRect(entityPos, tilemap), RectIndices);
        Handles.color = Color.white;
    }
    
    
    
    private static Vector3[] GenCellRect(Vector2Int tile, Tilemap tilemap)
    {
        Vector3 GetCorner(float x, float y) 
            => tilemap.LocalToWorld(tilemap.CellToLocalInterpolated(tile + new Vector2(x,y)));
        return new[]
        {
            GetCorner(0, 0),
            GetCorner(0, 1),
            GetCorner(1, 1),
            GetCorner(1, 0),
        };
    }
}

