using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace GameJamCat
{
    [CustomEditor(typeof(DialogueOptions))]
    public class DialogueOptionsEditor : Editor
    {
        SerializedProperty _path;

        public void OnEnable()
        {
            _path = serializedObject.FindProperty("_pathToCSV");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            serializedObject.Update();
            var dialogueOptions = target as DialogueOptions;

            if(GUILayout.Button("Select CSV"))
            {
                SelectCSV();
            }

            if(GUILayout.Button("Load CSV"))
            {
                LoadCSV(dialogueOptions);
            }

            serializedObject.ApplyModifiedProperties();

        }

        private void SelectCSV()
        {
            var path = EditorUtility.OpenFilePanel("Select CSV", Application.dataPath, "csv");
            if(!string.IsNullOrEmpty(path))
                _path.stringValue = "Assets" + path.Substring(Application.dataPath.Length);
        }

        private void LoadCSV(DialogueOptions options)
        {
            if (string.IsNullOrEmpty(_path.stringValue)) return;
            var pathToLoad = Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - 7),_path.stringValue);
            var list = Sinbad.CsvUtil.LoadObjects<CatCustomisation>(pathToLoad);
            Undo.RecordObject(options, "Load options");
            options._catCustomizationOptions = list;
            Debug.Log($"Loaded {list.Count} options");
        }
    }
}
