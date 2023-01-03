using System;
using DonBigo;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DirectionalSpriteSet))]
public class DirectionalSpriteEditor : PropertyDrawer
{
    private const string SpritesPropertyName = "sprites";
    
    private const float BoxWidth = 50;
    private const float BoxHeight = 50;
    private const float BoxSpacing = 10;
    
    //private void UpdateAsset(obj)
    
    private void DrawBox(Vector2Int dir, SerializedProperty arrayProp)
    {
        arrayProp.arraySize = DirectionalSpriteSet.SpriteOrder.Length;
        int index = Array.IndexOf(DirectionalSpriteSet.SpriteOrder, dir);
        if (index < 0)
        {
            Debug.LogError("Tentou fazer caixa de seleção para sprite direcional com direção inadequada: "+dir);
            return;
        }

        var prop = arrayProp.GetArrayElementAtIndex(index);

        Rect boxRect = GUILayoutUtility.GetRect(BoxWidth, BoxHeight);
        prop.objectReferenceValue = EditorGUI.ObjectField(boxRect, prop.objectReferenceValue, typeof(Sprite), false);
    }

    private bool _foldout;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var prop = property.FindPropertyRelative(SpritesPropertyName);
        float rowWidth = 3 * BoxWidth + 2 * BoxSpacing;
        float rowHeight = BoxHeight + BoxSpacing;

        _foldout = EditorGUILayout.Foldout(_foldout, label);
        if (!_foldout) return;

        for (int y = 0; y < 3; y++)
        {
            Rect rowRect = EditorGUILayout.BeginHorizontal(GUILayout.Width(rowWidth), GUILayout.Height(rowHeight));
            for (int x = 0; x < 3; x++)
            {
                if (y == 1 && x == 1) GUILayout.Space(BoxWidth);
                else DrawBox(new Vector2Int(x - 1, -(y - 1)), prop);
                
                if (x < 2) GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    /*public override void OnInspectorGUI()
    {
        var prop = serializedObject.FindProperty(SpritesPropertyName);
        float rowWidth = 3 * BoxWidth + 2 * BoxSpacing;
        float rowHeight = BoxHeight + BoxSpacing;
                         
        for (int y = 0; y < 3; y++)
        {
            Rect rowRect = EditorGUILayout.BeginHorizontal(GUILayout.Width(rowWidth), GUILayout.Height(rowHeight));
            for (int x = 0; x < 3; x++)
            {
                if (y == 1 && x == 1) continue;
                DrawBox(new Vector2Int(x, y));
            }
            EditorGUILayout.EndHorizontal();
        }
    }*/
}