using Object = UnityEngine.Object;
using System;
using UnityEditor;
using UnityEngine;

namespace OVAWORKT
{
    public static class InspectorUtility
    {
        private static class Styles
        {
            public static readonly GUIStyle Foldout;

            static Styles()
            {
                Foldout = new GUIStyle(EditorStyles.foldoutHeader)
                {

                };
            }
        }

        private const string SCRIPT_NAME = "m_Script";

        private static void DrawGeneric(this SerializedProperty property)
        {
            void SetObjectValue(SerializedProperty property, Object value) => property.objectReferenceValue = value;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(property.displayName);
            if (EditorGUILayout.DropdownButton(new(property.displayName), FocusType.Keyboard))
            {
                GenericMenu typeMenu = new();
                TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom(property.GetUnderlyingType());
                if (types.IsNullOrEmpty())
                {
                    for (int i = 0; i < types.Count; i++)
                    {
                        if (types[i].IsGenericType) continue;
                        typeMenu.AddItem(new(types[i].Name), false, () => SetObjectValue(property, Activator.CreateInstance(types[i]) as Object));
                    }
                }

                typeMenu.DropDown(new(Event.current.mousePosition, Vector2.zero));
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws properties for an editor, omitting the <i>'m_Script'</i> field.
        /// </summary>
        public static void OnInspectorGUI(this SerializedObject serializedObject)
        {
            EditorGUI.BeginChangeCheck();
            SerializedProperty property;

            using (var iterator = serializedObject.GetIterator())
            {
                if (iterator.NextVisible(enterChildren: true))
                {
                    do
                    {
                        if (iterator.name.Equals(SCRIPT_NAME)) continue;
                        property = serializedObject.FindProperty(iterator.name);
                        if (property.propertyType == SerializedPropertyType.ObjectReference) ObjectFieldDrawer.OnGUI(property);
                        else if (property.propertyType == SerializedPropertyType.Generic) EditorGUILayout.PropertyField(property);
                        else EditorGUILayout.PropertyField(property);
                    }
                    while (iterator.NextVisible(enterChildren: false));
                }
            }

            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
        }
    }
}