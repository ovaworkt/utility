using UnityEditor;
using UnityEngine;

namespace OVAWORKT
{
    [CustomEditor(typeof(Object), editorForChildClasses: true)]
    public sealed class ObjectEditor : Editor
    {
        public override void OnInspectorGUI() => serializedObject.OnInspectorGUI();
    }
}