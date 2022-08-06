using System.Reflection;
using System;
using UnityEditor;
using UnityEngine;

namespace OVAWORKT
{
    public static partial class ExtensionMethods
    {
        public static T GetPropertyAttribute<T>(this SerializedProperty property, bool inherit) where T : PropertyAttribute
        {
            if (property == null) return null;
            Type type = property.serializedObject.targetObject.GetType();

            FieldInfo fieldInfo = null;
            PropertyInfo propertyInfo = null;
            foreach (var name in property.propertyPath.Split('.'))
            {
                fieldInfo = type.GetField(name, (BindingFlags)(-1));
                if (fieldInfo == null)
                {
                    propertyInfo = type.GetProperty(name, (BindingFlags)(-1));
                    if (propertyInfo == null) return null;
                    else type = propertyInfo.PropertyType;
                }
                else type = fieldInfo.FieldType;
            }

            T[] attributes;
            if (fieldInfo != null) attributes = fieldInfo.GetCustomAttributes(typeof(T), inherit) as T[];
            else if (propertyInfo != null) attributes = propertyInfo.GetCustomAttributes(typeof(T), inherit) as T[];
            else return null;

            return attributes.Length > 0 ? attributes[0] : null;
        }
    }
}