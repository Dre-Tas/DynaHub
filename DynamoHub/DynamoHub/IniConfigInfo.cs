using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;

namespace DynaHub
{
    class IniConfigInfo
    {
        // Config file location
        private static readonly string configLoaction = @"C:\ProgramData\DynaHub";
        private static readonly string configName = "DynaHub_config.ini";
        internal static string configFilePath = Path.Combine(configLoaction, configName);

        private static IniData GetConfig()
        {
            // Start parser
            FileIniDataParser parser = new FileIniDataParser();
            // Read ini file with dll info
            try
            {
                return parser.ReadFile(configFilePath);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        private static string GetDllLocation()
        {
            IniData config = GetConfig();

            if (config == null)
                return null;

            return config["decryption"]["dll_location"];
        }

        private static string GetDllName()
        {
            IniData config = GetConfig();

            if (config == null)
                return null;

            return config["decryption"]["dll_name"];
        }

        internal static string GetDllPath()
        {
            string dllLocation = GetDllLocation();
            string dllName = GetDllName();

            IniData config = GetConfig();

            if (config == null || dllLocation == null || dllName == null)
                return null;

            return Path.Combine(dllLocation, dllName);
        }

        internal static string GetDllClass()
        {
            IniData config = GetConfig();
            
            if (config == null)
                return null;

            return config["decryption"]["class_name"];
        }

        internal static string GetDllMethod()
        {
            IniData config = GetConfig();

            if (config == null)
                return null;

            return config["decryption"]["method_name"];
        }
    }
}
