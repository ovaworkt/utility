using Object = UnityEngine.Object;
using System;
using UnityEditor;
using UnityEngine;

namespace OVAWORKT
{
    [InitializeOnLoad]
    public static class HierarchyScriptDragging
    {
        static HierarchyScriptDragging() => EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemCallback;

        private static void HierarchyWindowItemCallback(int pID, Rect pRect)
        {
            if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragExited || Event.current.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                Object[] draggedObjects = DragAndDrop.objectReferences;
                if (draggedObjects != null)
                {
                    if (draggedObjects.Length == 1)
                    {
                        if (draggedObjects[0] is MonoScript)
                        {
                            if (Event.current.type == EventType.DragUpdated) DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                            else if (Event.current.type == EventType.DragPerform)
                            {
                                Type componentType = (draggedObjects[0] as MonoScript).GetClass();
                                if (!componentType.IsSubclassOf(typeof(Component))) return;

                                GameObject gameObject = new GameObject(draggedObjects[0].name);
                                Component component = gameObject.AddComponent(componentType);

                                Selection.SetActiveObjectWithContext(gameObject, component);
                            }

                            Event.current.Use();
                        }
                    }
                }
            }
        }
    }
}