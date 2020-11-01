using System;
using System.Collections.Generic;
using System.Text;
using Tofunaut.TofuUnity.Interfaces;
using UnityEngine;
using ILogger = Tofunaut.TofuUnity.Interfaces.ILogger;

namespace Tofunaut.TofuUnity
{
    public class UnityGUILogger : MonoBehaviour, ILogger
    {
        public LogLevel logLevel;
        public float onScreenTime;
        
        private string _logText;
        private List<Tuple<float, string>> _logs;
        private GUIStyle _style;

        private void Awake()
        {
            _logText = string.Empty;
            _logs = new List<Tuple<float, string>>();
            _style = new GUIStyle
            {
                richText = true,
            };
        }

        private void Update()
        {
            if (_logs.Count <= 0)
                return;

            if (!(Time.time - _logs[0].Item1 >= onScreenTime)) 
                return;
            
            _logs.RemoveAt(0);
            RebuildLogText();
        }

        private void OnGUI()
        {
            if(string.IsNullOrEmpty(_logText))
                return;
            
            GUILayout.Label(_logText, _style);
        }

        public void Info(string s)
        {
            if (logLevel > LogLevel.Info)
                return;
            
            Log(s);
        }

        public void Warn(string s)
        {
            if (logLevel > LogLevel.Warn)
                return;
            
            Log($"<color=yellow>{s}</color>");
        }

        public void Error(string s)
        {
            if (logLevel > LogLevel.Error)
                return;
            
            Log($"<color=red>{s}</color>");
        }

        private void Log(string message)
        {
            _logs.Add(new Tuple<float, string>(Time.time, message));
            RebuildLogText();
        }

        private void RebuildLogText()
        {
            _logText = string.Empty;
            var sb = new StringBuilder();
            for (var i = _logs.Count - 1; i >= 0; i--)
                sb.AppendLine(_logs[i].Item2);
            
            _logText = sb.ToString();
        }
    }
}