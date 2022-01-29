using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace dpOra2Pg
{
    public class Utils
    {
        private static bool _counterInitialized;

        #region ReadSqlFromResource
        public static string ReadSqlFromResource(string name, string dbType)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourcePath = name;

            if (!name.StartsWith(nameof(dpOra2Pg)))
                resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name) && str.Contains($".{dbType}."));

            using Stream stream = assembly.GetManifestResourceStream(resourcePath);
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd().Replace("@", ":");
        }
        #endregion

        #region ConsoleWriteStats
        public static void ConsoleWriteStats(string str, bool final = false)
        {
            str = str.PadRight(100);
            if (_counterInitialized) // Dont erase on first iteration
                Console.Write(new string('\b', 100));
            else
            {
                _counterInitialized = true;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Console.CursorVisible = false;
            }

            Console.Write(str);

            if (final)
            {
         
                Console.WriteLine();
                _counterInitialized = false;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Console.CursorVisible = true;
            }

            Console.ResetColor();
        } 
        #endregion
    }
}
