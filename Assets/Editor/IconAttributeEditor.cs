using System;
using System.Reflection;
using DonBigo;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Object), true)]
[CanEditMultipleObjects]
public class IconAttributeEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        Texture2D tex = GetTexFor(target);
        if (tex == null) return base.RenderStaticPreview(assetPath, subAssets, width, height);
        
        Texture2D texCopy = new Texture2D(width, height);
        EditorUtility.CopySerialized(tex, texCopy);
        return texCopy;
    }

    Texture2D GetTexFor(Object obj)
    {
        if (obj == null) return null;
        
        var type = obj.GetType();
        var attribute = type.GetCustomAttribute<ScriptableObjectIconAttribute>();
        if (attribute == null) return null;

        var prop = FindProperty(type, attribute.PropName);
        var value = prop.GetValue(obj);
        if (prop.PropertyType == typeof(Texture2D))
        {
            return value as Texture2D;;
        }

        if (prop.PropertyType == typeof(Sprite))
        {
            return (value is Sprite sprite) ? sprite.texture : null;
        }

        Debug.LogError("Tipo inadequado pra variavel de icone! Deve ser Sprite ou Texture2D.");
        return null;
    }

    private PropertyInfo FindProperty(Type type, string propName)
    {
        while (type != null && type != typeof(Object))
        {
            var prop = type.GetProperty(propName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null)
            {
                return prop;
            }
            type = type.BaseType;
        }

        return null;
    }
}
