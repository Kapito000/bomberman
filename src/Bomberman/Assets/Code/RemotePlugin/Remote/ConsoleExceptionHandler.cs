using System;
using UnityEngine;

namespace RemotePlugin.Remote {
    public class ConsoleExceptionHandler : IExceptionHandler {
        private readonly bool _allowOnlyDevelopment;

        public ConsoleExceptionHandler(bool allowOnlyDevelopmentMove = true) {
            _allowOnlyDevelopment = allowOnlyDevelopmentMove;
        }
        
        public void HandleException(string message) {
            if (_allowOnlyDevelopment && !Debug.isDebugBuild) {
                return;
            }
            
            Debug.LogError(message);
        }

        public void HandleException(Exception exception) {
            if (_allowOnlyDevelopment && !Debug.isDebugBuild) {
                return;
            }

            Debug.LogError($"{exception.Message} (at {exception.StackTrace})");
        }
    }
}
