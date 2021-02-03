using System;
using System.IO;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Debugging/DebugLog")]
    public class DebugLog : ScriptableObject
    {
        private StreamWriter m_Writer;

        [SerializeField]
        private int maxFileSize = 5;

        public string lastError;

        public int errorCount;

        public void Initialize()
        {
            try
            {
                if (m_Writer == null)
                {
                    string folder = Path.Combine(Application.dataPath, "Fehlerprotokoll");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    long bytes = this.maxFileSize * 1000000;
                    string path = Path.Combine(folder, "log.txt");
                    if (File.Exists(path) && new FileInfo(path).Length > bytes) File.Delete(path);

                    m_Writer = new StreamWriter(path, true);
                    m_Writer.AutoFlush = true;

                    lastError = null;
                    errorCount = 0;
                }
            }
            catch
            {

            }
        }

        public void WriteLog(string logString, string stackTrace, LogType type)
        {
            if (this.m_Writer == null) return;
            string date = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            m_Writer.WriteLine("{0} {1}: {2}\r\n{3}", type, date, logString, stackTrace.Replace("\n", Environment.NewLine));

            string[] array = { type.ToString(), date, logString, stackTrace };
            this.lastError = logString;
            this.errorCount++;
        }
    }
}
