using System.Reflection;
using DonBigo;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Object), true)]
[CanEditMultipleObjects]
public class IconAttributeEditor : Editor
{
    private static BindingFlags memberBinding = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        Texture2D tex = GetIconFor(target);
        if (tex == null) return base.RenderStaticPreview(assetPath, subAssets, width, height);
        
        Texture2D texCopy = new Texture2D(width, height);
        EditorUtility.CopySerialized(tex, texCopy);
        return texCopy;
    }

    Texture2D GetIconFor(Object obj)
    {
        if (obj == null) return null;
        
        var type = obj.GetType();
        var attribute = type.GetCustomAttribute<ScriptableObjectIconAttribute>();
        if (attribute == null) return null;

        var prop = type.FindMemberInParentTypes(attribute.PropName, memberBinding);
        if (prop == null)
        {
            Debug.LogError("Property de icone nao foi encontrada");
            return null;
        }
        
        var value = prop.GetValue(obj);
        return ExtractTexFromObj(value);
    }

    private Texture2D ExtractTexFromObj(object obj)
    {
        if (obj is MonoBehaviour mb) obj = mb.gameObject;
        
        switch (obj)
        {
            case null:
                return null;
            case Texture2D tex:
                return tex;
            case Sprite sprite:
                return (sprite == null) ? null : sprite.texture;
            case GameObject go:
                if (go == null) return null;
                if (!go.TryGetComponent<SpriteRenderer>(out var renderer)) return null;
                return (renderer.sprite != null) ? renderer.sprite.texture : null;
        }
        Debug.LogError("Tipo inadequado pra variavel de icone! Deve ser Sprite, Texture2D ou um prefab com um sprite.");
        return null;
    }
    
}
