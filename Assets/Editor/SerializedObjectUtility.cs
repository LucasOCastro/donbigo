using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class SerializedObjectUtility
{
    public static IEnumerable<SerializedProperty> IterateProperties(this SerializedObject obj,
        bool enterChildren)
    {
        SerializedProperty prop = obj.GetIterator();
        yield return prop;
        if (!prop.NextVisible(true)) yield break;
        
        while (prop.NextVisible(enterChildren)) 
        {
            yield return prop; 
        }
    }

    public static SerializedProperty GetPropertyOfType(this SerializedObject obj, Type type)
    {
        foreach (var prop in obj.IterateProperties(enterChildren: false))
        {
            if (prop.type == nameof(type))
            {
                return prop;
            }
        }
        return null;
    }

    public static void SetArrayValue<T>(this SerializedProperty prop, T[] arr) where T: Object
    {
        prop.arraySize = arr.Length;
        for (int i = 0; i < arr.Length; i++)
        {
            prop.GetArrayElementAtIndex(i).objectReferenceValue = arr[i];
        }
    }

    public static string CurrentOpenDir()
    {
        MethodInfo getActiveFolderPath = typeof(ProjectWindowUtil).GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
        if (getActiveFolderPath == null)
        {
            Debug.LogError("Método GetActiveFolderPath não encontrado");
            return null;
        }
        return getActiveFolderPath.Invoke(null, new object[0]) as string;
    }
}