using System;
using DonBigo.Rooms;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = UnityEngine.Tilemaps.Tile;

public class ReplaceTilesWindow : EditorWindow
{
    [MenuItem ("Window/Replace Tiles")]
    public static void ShowWindow() 
    {
        EditorWindow.GetWindow(typeof(ReplaceTilesWindow));
    }

    private static Type TileType = typeof(Tile);
    private Tile _a, _b;
    private void OnGUI()
    {
        GUILayout.Label("Replace:");
        _a = EditorGUILayout.ObjectField(_a, TileType, false) as Tile;
        GUILayout.Label("With:");
        _b = EditorGUILayout.ObjectField(_b, TileType, false) as Tile;

        bool disabled = _a == null || _b == null;
        EditorGUI.BeginDisabledGroup(disabled);
        bool replace = GUILayout.Button("Replace");
        EditorGUI.EndDisabledGroup();

        if (replace)
        {
            ReplaceAllRooms();
        }
    }

    private void ReplaceAllRooms()
    {
        RoomEditor.UpdateRooms(ReplaceInRoom);
        /*foreach (string path in AssetDatabase.GetAllAssetPaths())
        {
            Room room = AssetDatabase.LoadAssetAtPath<Room>(path);
            if (room == null) continue;
            ReplaceInRoom(room, a, b);
        }*/
    }

    private void ReplaceInRoom(Tilemap tm)
    {
        for (int x = 0; x < tm.size.x; x++)
        {
            for (int y = 0; y < tm.size.y; y++)
            {
                for (int z = 0; z < tm.size.z; z++)
                {
                    Vector3Int pos = new Vector3Int(x, y, z);
                    if (tm.GetTile(pos) == _a)
                    {
                        tm.SetTile(pos, _b);
                    }
                }
            }
        }
    }
}