using System.Linq;
using DonBigo;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridEditor : Editor
{
    private const string MapSizeName = "mapSize";

    private (Vector3Int[] points, int []segments) GetSquare(int size)
    {
        Vector3Int[] points = {
            Vector3Int.zero,
            new(0, size),
            new(size, size),
            new(size, 0)
        };

        int[] segments = {
            0, 1,
            1, 2,
            2, 3,
            3, 0
        };

        return (points, segments);
    }
    
    private void OnSceneGUI()
    {
        GridManager manager = target as GridManager;
        if (manager == null) return;
        
        Grid grid = manager.GetComponent<Grid>();
        if (grid == null) return;
        
        var sizeProp = serializedObject.FindProperty(MapSizeName);
        if (sizeProp == null) return;
        
        int size = sizeProp.intValue;
        var pointSegs = GetSquare(size);

        Vector3[] actualPoints = pointSegs.points.Select(p => grid.CellToWorld(p)).ToArray();
        
        Handles.color = Color.red;
        Handles.DrawLines(actualPoints, pointSegs.segments);
        Handles.color = Color.white;
    }
}
