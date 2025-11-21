using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LabApi.Features.Console;
using Module = DarkCore.Utilities.ModuleManager.Abstracts.Module;

namespace DarkCore.Utilities.ModuleManager
{
    public static class ModuleManager
    {
        public static readonly List<Module> Modules = new List<Module>();
        
        public static void LoadModules()
        {
            Logger.Raw("================== [ModuleManager] ==================", ConsoleColor.Magenta);
            Logger.Raw("[ModuleManager] Starting to load modules...", ConsoleColor.Magenta);
            
            var moduleTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(Module).IsAssignableFrom(t))
                .ToList();
            
            Logger.Raw($"[ModuleManager] Found {moduleTypes.Count} module(s) to load.", ConsoleColor.Magenta);

            foreach (var type in moduleTypes)
                LoadModule(type);
            
            Logger.Raw($"[ModuleManager] Finished loading modules. Loaded {Modules.Count} module(s).", ConsoleColor.Magenta);
            Logger.Raw("================== [ModuleManager] ==================", ConsoleColor.Magenta);
        }
        
        public static void UnloadModules()
        {
            Logger.Raw("================== [ModuleManager] ==================", ConsoleColor.Magenta);
            Logger.Raw("[ModuleManager] Starting to unload modules...", ConsoleColor.Magenta);
            
            foreach (var module in Modules)
                UnloadModule(module, false, true);
            
            Modules.Clear();
            Logger.Raw("[ModuleManager] Finished unloading modules.", ConsoleColor.Magenta);
            Logger.Raw("================== [ModuleManager] ==================", ConsoleColor.Magenta);
        }

        public static bool LoadModule(Type type, bool ignoreProperties = false, bool updateProperties = false)
        {
            if (!typeof(Module).IsAssignableFrom(type) || type.IsAbstract)
            {
                Logger.Raw($"[ModuleManager] → Type {type.Name} is not a valid module type.", ConsoleColor.Red);
                return false;
            }

            try
            {
                var module = (Module)Activator.CreateInstance(type);
                
                module.GenerateConfig();
                
                if (!ignoreProperties && !module.IsEnabled)
                {
                    Logger.Raw($"[ModuleManager] → Module {module.Name} is disabled in properties. Skipping load.", ConsoleColor.Yellow);
                    return false;
                }
                
                if (updateProperties)
                    module.IsEnabled = true;
                
                module.Enable();
                Modules.Add(module);
                Logger.Raw($"[ModuleManager] → Loaded module: {module.Name} v{module.Version} by {module.Author}", ConsoleColor.Green);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Raw($"[ModuleManager] → Failed to load module of type {type.Name}: {ex}", ConsoleColor.Red);
                return false;
            }
        }

        public static bool UnloadModule(Module module, bool updateProperties = false, bool ignoreMandatory = false)
        {
            try
            {
                var fullName = module.GetType().FullName;
                if (fullName != null && fullName.Contains(".Mandatory.") && !ignoreMandatory)
                {
                    Logger.Raw($"[ModuleManager] → Module {module.Name} is mandatory and cannot be disabled.", ConsoleColor.Yellow);
                    return false;
                }
                
                if (updateProperties)
                    module.IsEnabled = false;
                
                module.Disable();
                Modules.Remove(module);
                Logger.Raw($"[ModuleManager] → Disabled module: {module.Name}", ConsoleColor.Yellow);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Raw($"[ModuleManager] → Failed to disable module {module.Name}: {ex}", ConsoleColor.Red);
                return false;
            }
        }
    }
}