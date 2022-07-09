using UnityEditor;

namespace OVAWORKT
{
    public static class InspectorUtility
    {
        private const string SCRIPT_NAME = "m_Script";

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
                        EditorGUILayout.PropertyField(property);
                    }
                    while (iterator.NextVisible(enterChildren: false));
                }
            }

            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
        }
    }
}