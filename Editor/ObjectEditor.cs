using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OVAWORKT
{
    [CanEditMultipleObjects, CustomEditor(typeof(Object), editorForChildClasses: true)]
    public sealed class ObjectEditor : Editor
    {
        private static class Styles
        {
            public static readonly GUIStyle Foldout;

            static Styles()
            {
                Foldout = new(EditorStyles.foldout)
                {

                };
            }
        }

        private int _propertyIndex;
        private List<bool> _foldouts;

        public override void OnInspectorGUI() => serializedObject.OnInspectorGUI();
    }
}