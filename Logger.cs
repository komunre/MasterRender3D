using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterRender3D
{
    public static class Logger
    {
        public static FileStream? _logFile;

        public static void Init()
        {
            _logFile = File.OpenWrite("log.txt");
        }

        public static void Log(string message)
        {
            _logFile.Write(Encoding.ASCII.GetBytes(message));
        }

        public static void LogLine(string message)
        {
            _logFile.Write(Encoding.ASCII.GetBytes(message + "\n"));
        }
    }
}
