using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Tofunaut.TofuUnity.Editor
{
    public class TextImportWindow : EditorWindow
    {
        private Action<string> _onComplete;
        private Vector2 _scrollPos;
        private string _text;
        
        public static void Init(string title, Action<string> onComplete)
        {
            var window = (TextImportWindow) EditorWindow.GetWindow(typeof(TextImportWindow));
            window.titleContent = new GUIContent(title);
            window._onComplete = onComplete;
            window._text = string.Empty;
        }

        public void OnGUI()
        {
            if (GUILayout.Button("Open File"))
                ImportFromFile();
            
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.ExpandHeight(true));
            _text = EditorGUILayout.TextArea(_text, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
            
            if (GUILayout.Button("Import"))
                _onComplete?.Invoke(_text);
        }

        private void ImportFromFile()
        {
            var path = EditorUtility.OpenFilePanel("Open", "", "txt,json,csv");
            if (path.Length != 0)
                _text =  File.ReadAllText(path);
        }
    }
}