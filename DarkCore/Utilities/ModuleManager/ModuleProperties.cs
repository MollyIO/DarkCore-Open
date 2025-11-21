using System.IO;
using DarkCore.Utilities.ModuleManager.Configuration;
using LabApi.Loader;

namespace DarkCore.Utilities.ModuleManager
{
    public class ModuleProperties
    {
        public static Properties Get(string moduleName)
        {
            Directory.CreateDirectory(Path.Combine(DarkCore.PluginInstance.GetConfigDirectory().FullName, "modules", moduleName));
            return DarkCore.PluginInstance.LoadConfig<Properties>(Path.Combine("modules", moduleName, "properties.yml"));
        }

        public static void Save(Properties properties, string moduleName)
        {
            Directory.CreateDirectory(Path.Combine(DarkCore.PluginInstance.GetConfigDirectory().FullName, "modules", moduleName));
            DarkCore.PluginInstance.SaveConfig(properties, Path.Combine("modules", moduleName, "properties.yml"));
        }
    }
}