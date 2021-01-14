using UnityEngine;

namespace CrystalAlchemist
{
    public class ExceptionHandler : MonoBehaviour
    {
        [SerializeField]
        private DebugLog log;

        void OnEnable()
        {
            this.log.Initialize();
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable() => Application.logMessageReceived -= HandleLog;

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Exception) log.WriteLog(logString, stackTrace, type);
        }
    }
}