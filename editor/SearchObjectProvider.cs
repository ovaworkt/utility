using Object = UnityEngine.Object;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;

namespace OVAWORKT
{
    public sealed class SearchObjectProvider : ScriptableObject, ISearchWindowProvider
    {
        public Type AssetType { private get; set; }
        public SerializedProperty SerializedProperty { private get; set; }

        public SearchObjectProvider(Type assetType, SerializedProperty serializedProperty)
        {
            AssetType = assetType;
            SerializedProperty = serializedProperty;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> list = new List<SearchTreeEntry>();
            string[] assetGuids = AssetDatabase.FindAssets($"t:{AssetType.Name}");
            List<string> paths = new List<string>();
            foreach (string guid in assetGuids) paths.Add(AssetDatabase.GUIDToAssetPath(guid));

            paths.Sort((a, b) =>
            {
                string[] aSplits = a.Split('/'), bSplits = b.Split('/');
                for (int i = 0; i < aSplits.Length; i++)
                {
                    if (i >= bSplits.Length) return 1;
                    int value = aSplits[i].CompareTo(bSplits[i]);
                    if(value != 0)
                    {
                        if(aSplits.Length != bSplits.Length && (i == aSplits.Length - 1 || i == bSplits.Length - 1)) return aSplits.Length < bSplits.Length ? 1 : -1;
                        return value;
                    }
                }

                return 0;
            });

            List<string> groups = new List<string>();
            foreach (string item in paths)
            {
                string[] entryTitle = item.Split('/');
                string groupName = string.Empty;
                for (int i = 0; i < entryTitle.Length - 1; i++)
                {
                    groupName += entryTitle[i];
                    if(!groups.Contains(groupName))
                    {
                        list.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]), i + 1));
                        groups.Add(groupName);
                    }

                    groupName += '/';
                }

                Object obj = AssetDatabase.LoadAssetAtPath<Object>(item);
                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(entryTitle.Last().Split('.')[0], EditorGUIUtility.ObjectContent(obj, obj.GetType()).image));
                entry.level = entryTitle.Length;
                entry.userData = obj;
                list.Add(entry);
            }

            return list;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            SerializedProperty.objectReferenceValue = (Object)SearchTreeEntry.userData;
            SerializedProperty.serializedObject.ApplyModifiedProperties();
            return true;
        }
    }
}