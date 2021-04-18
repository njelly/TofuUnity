using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tofunaut.TofuUnity
{
    [RequireComponent(typeof(PlayerInput))]
    public class DebugOverlay : MonoBehaviour
    {
        public BuildInfo buildInfo;
        public int maxLogs = 200;
        public TextMeshProUGUI buildNumberLabel;
        public TextMeshProUGUI fpsLabel;
        public float fpsUpdateInterval;
        public TextMeshProUGUI errorCountLabel;
        public Transform logScrollPanel;
        public Transform logMessageContainer;
        public Transform stackTracePanel;
        public TextMeshProUGUI stackTraceLabel;
        public DebugOverlayLog logMessagePrefab;
        
        private Queue<(string, string, LogType)> _logQueue;
        private Queue<DebugOverlayLog> _logMessages;
        private PlayerInput _playerInput;
        private int _fpsCounter;
        private float _fpsUpdateTimer;
        private int _errorCount;
        
        private void Awake()
        {
            _logQueue = new Queue<(string, string, LogType)>();
            _logMessages = new Queue<DebugOverlayLog>();
            Application.logMessageReceived += OnLogMessageReceived;
            Debug.Log($"build {buildInfo.buildNumber} created on {buildInfo.buildDate} by {buildInfo.buildMachineName}");
            DontDestroyOnLoad(gameObject);

            buildNumberLabel.text = $"Build: {buildInfo.buildNumber}";
            _fpsUpdateTimer = fpsUpdateInterval;
            SetFPSLabelText(0);
            SetErrorCountText();
            
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.actions["Toggle"].Enable();
            _playerInput.actions["Toggle"].started += OnToggle;
            
            ShowLogScroll(false);
            ShowStackTrace(false);
        }

        private void Update()
        {
            UpdateFPSCounter();
            
            if(logScrollPanel.gameObject.activeInHierarchy)
                InstantiateAllLogMessages();
        }
        
        private void OnDestroy()
        {
            if(_playerInput)
                _playerInput.actions["Toggle"].started -= OnToggle;
        }

        private void OnToggle(InputAction.CallbackContext context)
        {
            if (!context.started)
                return;
            
            ShowLogScroll(!logScrollPanel.gameObject.activeInHierarchy);
        }

        private void ShowLogScroll(bool doShow)
        {
            logScrollPanel.gameObject.SetActive(doShow);
            
            if(!doShow)
                ShowStackTrace(false);
        }

        private void ShowStackTrace(bool doShow) => ShowStackTrace(doShow, string.Empty);
        private void ShowStackTrace(bool doShow, string message)
        {
            stackTracePanel.gameObject.SetActive(doShow);
            
            if (doShow)
                stackTraceLabel.text = message;
        }

        private void OnLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            var message = $"{DateTime.Now:HH:mm:ss}: {condition}";
            _logQueue.Enqueue((message, stacktrace, type));

            while (_logQueue.Count > maxLogs)
                _logQueue.Dequeue();
        }

        public void CloseStackTrace() => ShowStackTrace(false);

        private void SetFPSLabelText(int fps) => fpsLabel.text = $"FPS: {fps}";

        private void SetErrorCountText()
        {
            var s = string.Empty;
            if (_errorCount == 1)
                s = "Errors: 1";
            else if(_errorCount > 1)
                s = $"Errors: {_errorCount}";

            errorCountLabel.text = s;
        }

        private void UpdateFPSCounter()
        {
            _fpsUpdateTimer -= Time.deltaTime;
            _fpsCounter++;
            
            if (_fpsUpdateTimer > 0) 
                return;
            
            var fps = Mathf.RoundToInt(_fpsCounter / fpsUpdateInterval);
            SetFPSLabelText(fps);
            _fpsCounter = 0;
            _fpsUpdateTimer += fpsUpdateInterval;
        }

        private void InstantiateAllLogMessages()
        {
            while (_logQueue.Count > 0)
            {
                var (message, stackTrace, logType) = _logQueue.Dequeue();
                var logMessage = Instantiate(logMessagePrefab, logMessageContainer, false);
                logMessage.messageLabel.text = message;
                switch (logType)
                {
                    case LogType.Warning:
                        logMessage.messageLabel.color = Color.yellow;
                        break;
                    case LogType.Error:
                        logMessage.messageLabel.color = Color.red;
                        break;
                    case LogType.Exception:
                        logMessage.messageLabel.color = Color.magenta;
                        break;
                    default:
                        logMessage.messageLabel.color = Color.white;
                        break;
                }
                    
                logMessage.button.onClick.RemoveAllListeners();
                logMessage.button.onClick.AddListener(() => ShowStackTrace(true, $"{message}\n{stackTrace}"));
                    
                _logMessages.Enqueue(logMessage);
            }

            while (_logMessages.Count > maxLogs)
            {
                var oldMessage = _logMessages.Dequeue();
                Destroy(oldMessage.gameObject);
            }
        }
    }
}