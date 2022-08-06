using Object = UnityEngine.Object;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;

namespace OVAWORKT
{
    public static class ObjectFieldDrawer
    {
        private static class Styles
        {
            public static readonly GUIStyle ObjectField, ObjectThumbnail, SearchButton, NullButton;
            public static readonly GUIContent SearchIcon, NullIcon;

            static Styles()
            {
                ObjectField = new("ObjectField");
                ObjectThumbnail = new()
                {
                    fontSize = 0,
                    stretchWidth = false,
                    stretchHeight = false,
                    fixedWidth = EditorGUIUtility.singleLineHeight,
                    fixedHeight = EditorGUIUtility.singleLineHeight,
                    padding = new(1,1,1,1),
                    margin = new(0,0,0,0),
                };

                SearchButton = new(EditorStyles.miniButtonLeft)
                {
                    fixedWidth = EditorGUIUtility.singleLineHeight,
                    fixedHeight = EditorGUIUtility.singleLineHeight,
                    padding = new(2,2,2,2)
                };

                NullButton = new(EditorStyles.miniButtonRight)
                {
                    fixedWidth = EditorGUIUtility.singleLineHeight,
                    fixedHeight = EditorGUIUtility.singleLineHeight,
                    padding = new(1,1,1,1)
                };

                SearchIcon = new(EditorGUIUtility.IconContent("d_Search Icon"));
                NullIcon = new(EditorGUIUtility.IconContent("winbtn_win_close"));
            }
        }

        public static void OnGUI(SerializedProperty property)
        {
            Rect position = EditorGUILayout.GetControlRect();
            position = EditorGUI.PrefixLabel(position, new(property.displayName));

            float width = position.width, iconWidth = EditorGUIUtility.singleLineHeight;
            position.width -= iconWidth * 2 + 1;
            GUI.Box(position, GUIContent.none, Styles.ObjectField);

            if (position.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragExited || Event.current.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    Object[] draggedObjects = DragAndDrop.objectReferences;
                    if (!draggedObjects.IsNullOrEmpty())
                    {
                        if (draggedObjects.Length == 1)
                        {
                            if (draggedObjects[0].GetType().Equals(property.GetUnderlyingType()))
                            {
                                if (Event.current.type == EventType.DragUpdated) DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                                else if (Event.current.type == EventType.DragPerform) property.SetUnderlyingValue(draggedObjects[0]);
                            }
                        }
                    }
                }
            }

            position.x += iconWidth - 2;
            GUI.enabled = property.objectReferenceValue != null;
            GUI.Label(position, property.objectReferenceValue != null ? property.objectReferenceValue.name : "None");
            GUI.enabled = true;

            position.x -= iconWidth - 2;
            position.width = position.height;
            GUI.enabled = property.objectReferenceValue != null;
            GUI.Box(position, new GUIContent(EditorGUIUtility.ObjectContent(property.objectReferenceValue, property.GetUnderlyingType()).image), Styles.ObjectThumbnail);
            GUI.enabled = true;

            position.x += width - EditorGUIUtility.singleLineHeight;
            position.x -= iconWidth - 1;
            if (GUI.Button(position, Styles.SearchIcon, Styles.SearchButton))
            {
                Type type = property.GetUnderlyingType();
                SearchObjectProvider searchObjectProvider = ScriptableObject.CreateInstance<SearchObjectProvider>();
                searchObjectProvider.AssetType = type;
                searchObjectProvider.SerializedProperty = property;
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), searchObjectProvider);
            }

            position.x += iconWidth + 1;
            GUI.enabled = property.objectReferenceValue != null;
            if (GUI.Button(position, Styles.NullIcon, Styles.NullButton)) property.objectReferenceValue = null;
            GUI.enabled = true;
        }
    }
}