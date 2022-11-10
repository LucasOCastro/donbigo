using System;
using System.Linq;
using DonBigo;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileCreator
{
    private static string ProcessTileTypeName(Type type)
    {
        if (type == typeof(TileType)) return "Floor";
        return type.Name.Replace("Type", "").Replace("Tile", "");
    }
    
    private static Type GetTypeDialog(Type[] types, int i, Sprite sprite)
    {
        if (i < 0 || i >= types.Length) return null;
        
        string title = "Tipo de Tile";
        string descr = $"Escolha o tipo de tile para {sprite}";
        if (i == types.Length - 1)
        {
            bool doLast = EditorUtility.DisplayDialog(title, descr, ProcessTileTypeName(types[i]), "Cancel");
            if (doLast) return types[i];
            return null;
        }
        
        int choice = EditorUtility.DisplayDialogComplex(
            title,
            descr,
            ProcessTileTypeName(types[i]),
            (i == types.Length - 2) ? "Cancel" : "Next",
            ProcessTileTypeName(types[i + 1])
        );
        return choice switch
        {
            0 => types[i],
            1 => GetTypeDialog(types, i + 2, sprite),
            2 => types[i + 1],
            _ => null
        };
    }

    private static Type GetTileBaseType()
    {
        int choice = EditorUtility.DisplayDialogComplex(
            "Tipo de Tile",
            "Escolha o tipo base de tile",
            "BaseTile",
            "Cancel",
            "StructureTile"
        );
        return choice switch
        {
            0 => typeof(TileType),
            1 => null,
            2 => typeof(StructureTileType),
            _ => null
        };
    }
    
    [CreateTileFromPalette]
    private static TileBase CreateTile(Sprite sprite)
    {
        Type baseType = GetTileBaseType();
        if (baseType == null) return null;
        
        Type[] tileTypes = ReflectionUtility.TypesThatInherit(baseType, true).Where(t => !t.IsAbstract).ToArray();

        
        
        Type type = GetTypeDialog(tileTypes, 0, sprite);
        if (type == null) return null;

        var tile = ScriptableObject.CreateInstance(type) as UnityEngine.Tilemaps.Tile;
        tile.sprite = sprite;
        tile.name = sprite.name; 
        return tile;
    }
}
