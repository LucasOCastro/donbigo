using System.Reflection;
using DonBigo;
using DonBigo.Rooms;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor
{
    private const string BoundsName = "bounds";
    private const string TilesBlockName = "tilesBlock";
    private const string TileTypesName = "tileTypes";

    public override void OnInspectorGUI()
    {
        bool alreadyFilled = serializedObject.FindProperty(TilesBlockName).arraySize > 0;
        
        Tilemap tm = EditorGUILayout.ObjectField(null, typeof(Tilemap), allowSceneObjects: true) as Tilemap;
        
        if (alreadyFilled)
        {
            if (GUILayout.Button("Instantiate"))
            {
                Instantiate();
            }
            
            EditorGUI.BeginDisabledGroup(true);
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();
        }
        
        if (tm == null) return;

        if (alreadyFilled)
        {
            bool overrideConfirm = EditorUtility.DisplayDialog("Sobreescrever Cômodo",
                "Deseja mesmo sobreescrever o conteúdo desse cômodo?",
                "Sim", "Não");
            if (!overrideConfirm) return;
        }
        
        tm.CompressBounds();
        UpdateValues(tm);
        serializedObject.ApplyModifiedProperties();
        AssetDatabase.SaveAssetIfDirty(target);
    }

    private void Instantiate()
    {
        Grid grid = FindObjectOfType<Grid>();
        if (grid == null)
        {
            Debug.LogError("Não tem nenhuma grid na cena!");
            return;
        }

        GameObject go = new GameObject(target.name, typeof(Tilemap), typeof(TilemapRenderer));
        go.transform.parent = grid.transform;
        
        TilemapRenderer tmRenderer = go.GetComponent<TilemapRenderer>();
        tmRenderer.mode = TilemapRenderer.Mode.Individual;
        
        Tilemap tm = go.GetComponent<Tilemap>();
        (target as Room).FillTilemap(tm, Vector2Int.zero);
    }

    private void UpdateValues(Tilemap tm)
    {
        var bounds = tm.cellBounds;
        serializedObject.FindProperty(BoundsName).vector3IntValue = bounds.size;
        
        var block = tm.GetTilesBlock(bounds);
        serializedObject.FindProperty(TilesBlockName).SetArrayValue(block);

        var tileTypesProp = serializedObject.FindProperty(TileTypesName);
        tileTypesProp.arraySize = bounds.size.x * bounds.size.y;
        for (int z = bounds.zMin; z <= bounds.zMax; z++)
        {
            for (int x = bounds.xMin; x <= bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y <= bounds.yMax; y++)
                {
                    TileType tile = tm.GetTile<TileType>(new Vector3Int(x, y, z));
                    if (tile != null)
                    {
                        int xyToI = (bounds.size.x * (y - bounds.yMin)) + (x - bounds.xMin);
                        tileTypesProp.GetArrayElementAtIndex(xyToI).objectReferenceValue = tile;
                    }
                }
            }    
        }
        
        
    }
}
