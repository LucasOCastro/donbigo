using System;
using System.Reflection;

public static class ReflectionUtility
{
    public static object GetValue(this MemberInfo member, object obj)
    {
        if (member is FieldInfo field) return field.GetValue(obj);
        if (member is PropertyInfo prop) return prop.GetValue(obj);
        return null;
    }
    
    public static MemberInfo FindMemberInParentTypes(this Type type, string name, BindingFlags bind)
    {
        while (type != null && type != typeof(Object))
        {
            var prop = type.GetProperty(name, bind)  as MemberInfo ?? type.GetField(name, bind) as MemberInfo;
            if (prop != null)
            {
                return prop;
            }
            type = type.BaseType;
        }
        return null;
    }
}
