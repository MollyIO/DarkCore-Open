using System.Collections.Generic;
using System.IO;
using LabApi.Loader;

namespace DarkCore.Utilities.ModuleManager
{
    public class ModuleConfig
    {
        public Dictionary<string, bool> EnabledModules { get; set; } = new Dictionary<string, bool>();
        
        public static T Get<T>(string moduleName) where T : class, new()
        {
            Directory.CreateDirectory(Path.Combine(DarkCore.PluginInstance.GetConfigDirectory().FullName, "modules", moduleName));
            return DarkCore.PluginInstance.LoadConfig<T>(Path.Combine("modules", moduleName, "config.yml"));
        }

        public static void Save<T>(T config, string moduleName) where T : class, new()
        {
            Directory.CreateDirectory(Path.Combine(DarkCore.PluginInstance.GetConfigDirectory().FullName, "modules", moduleName));
            DarkCore.PluginInstance.SaveConfig(config, Path.Combine("modules", moduleName, "config.yml"));
        }
    }
}