using DonBigo;
using DonBigo.Rooms;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor
{
    private const string RoomNameName = "roomName";
    
    private const string BoundsName = "size";
    private const string TilesBlockName = "tilesBlock";
    private const string TileTypesName = "tileTypes";
    
    private const string DoorsName = "doors";
    private const string DoorPosName = "localPos";
    private const string DoorDirectionName = "direction";
    private const string DoorMarkerName = "marker";

    private const string StructuresName = "structureTiles";
    private const string StructurePosName = "pos";
    private const string StructureTileName = "structure";

    private const string TransformOverridesName = "transformOverrides";
    private const string OverridePosName = "pos";
    private const string OverrideMatrixName = "matrix";
    
    public override void OnInspectorGUI()
    {
        var nameProp = serializedObject.FindProperty(RoomNameName);
        string newName = nameProp.stringValue;
        if (nameProp.stringValue.NullIfEmpty() == null && target != null) {
            newName = target.name;
        }
        newName = EditorGUILayout.TextField(newName);
        if (newName != nameProp.stringValue) {
            nameProp.stringValue  =newName;
            serializedObject.ApplyModifiedProperties();
        }
        
        bool alreadyFilled = serializedObject.FindProperty(TilesBlockName).arraySize > 0;
        
        Tilemap tm = EditorGUILayout.ObjectField(null, typeof(Tilemap), allowSceneObjects: true) as Tilemap;
        
        //Se já temos um tilemap armazenado, mostrar informações extras
        if (alreadyFilled)
        {
            if (GUILayout.Button("Instantiate"))
            {
                Instantiate();
            }

            //Informações de visualização do tilemap armazenado no comodo
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

        GameObject go = new(target.name, typeof(Tilemap), typeof(TilemapRenderer));
        go.transform.parent = grid.transform;
        
        TilemapRenderer tmRenderer = go.GetComponent<TilemapRenderer>();
        tmRenderer.mode = TilemapRenderer.Mode.Individual;
        
        Tilemap tm = go.GetComponent<Tilemap>();
        (target as Room).FillTilemap(tm, Vector2Int.zero);
    }

    private void UpdateValues(Tilemap tm)
    {
        tm.CompressBounds();
        var bounds = tm.cellBounds;
        serializedObject.FindProperty(BoundsName).vector3IntValue = bounds.size;
         
        var block = tm.GetTilesBlock(bounds);
        serializedObject.FindProperty(TilesBlockName).SetArrayValue(block);

        var tileTypesProp = serializedObject.FindProperty(TileTypesName);
        var doorsProp = serializedObject.FindProperty(DoorsName);
        var transformsProp = serializedObject.FindProperty(TransformOverridesName);
        var structuresProp = serializedObject.FindProperty(StructuresName);
        tileTypesProp.arraySize = bounds.size.x * bounds.size.y;
        doorsProp.arraySize = 0;
        transformsProp.arraySize = 0;
        structuresProp.arraySize = 0;
        //Percorrendo as tiles de baixo pra cima, garantimos que a parede será armazenada na lista ao invés do chão abaixo dela.
        for (int z = bounds.zMin; z < bounds.zMax; z++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, z);
                    var tile = tm.GetTile<UnityEngine.Tilemaps.Tile>(tilePos);
                    if (tile == null) continue;

                    switch (tile)
                    {
                        case TileType:
                        {
                            int xyToI = (bounds.size.x * (y - bounds.yMin)) + (x - bounds.xMin);
                            tileTypesProp.GetArrayElementAtIndex(xyToI).objectReferenceValue = tile;
                            break;
                        }
                        case StructureTileType structure:
                        {
                            structuresProp.arraySize++;
                            var arrayItem = structuresProp.GetArrayElementAtIndex(structuresProp.arraySize - 1);
                            arrayItem.FindPropertyRelative(StructurePosName).vector2IntValue = new Vector2Int(x, y);
                            arrayItem.FindPropertyRelative(StructureTileName).objectReferenceValue = structure;
                            break;
                        }
                    }

                    RoomExit.Direction? dir;
                    if (tile is IRoomEntranceMarker && (dir = GetExitDirectionAt(x, y, bounds)) != null)
                    {
                        doorsProp.arraySize++;
                        var arrayItem = doorsProp.GetArrayElementAtIndex(doorsProp.arraySize - 1);
                        arrayItem.FindPropertyRelative(DoorPosName).vector2IntValue = new Vector2Int(x, y);
                        arrayItem.FindPropertyRelative(DoorDirectionName).enumValueFlag = (int)dir.Value;
                        arrayItem.FindPropertyRelative(DoorMarkerName).objectReferenceValue = tile;
                    }

                    var tileMatrix = tm.GetTransformMatrix(tilePos); 
                    if (tileMatrix != tile.transform)
                    {
                        transformsProp.arraySize++;
                        var arrayItem = transformsProp.GetArrayElementAtIndex(transformsProp.arraySize - 1);
                        arrayItem.FindPropertyRelative(OverridePosName).vector3IntValue = tilePos;
                        arrayItem.FindPropertyRelative(OverrideMatrixName).SetMatrixValue(tileMatrix);
                    }
                }
            }    
        }
    }

    //Esse método só retornará uma direção se a porta estiver na borda de uma sala. Portas no interior do comodo serão ignoradas.
    private static RoomExit.Direction? GetExitDirectionAt(int x, int y, BoundsInt bounds)
    {
        if (y == bounds.yMin) return RoomExit.Direction.Down;
        if (y == bounds.yMax - 1) return RoomExit.Direction.Up;
        if (x == bounds.xMin) return RoomExit.Direction.Left;
        if (x == bounds.xMax - 1) return RoomExit.Direction.Right;
        return null;
    }
}
