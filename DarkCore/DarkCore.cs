using System;
using System.IO;
using System.Reflection;
using DarkCore.Utilities.ModuleManager;
using HarmonyLib;
using LabApi.Features;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
using LiteDB;

namespace DarkCore
{
    public class DarkCore : Plugin
    {
        public override string Name => "DarkCore";
        public override string Description => "A core plugin for Darkness Project SCP:SL servers.";
        public override string Author => "MollyIO";
        public override Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;
        public override Version RequiredApiVersion { get; } = new Version(LabApiProperties.CompiledVersion);
        
        public static DarkCore PluginInstance { get; private set; }
        public static Harmony HarmonyInstance { get; private set; }
        public static LiteDatabase DatabaseInstance { get; private set; }
        
        public override void Enable()
        {
            PluginInstance = this;
            HarmonyInstance = new Harmony($"{Author.ToLower()}.{Name.ToLower()}");
            DatabaseInstance = new LiteDatabase(Path.Combine(this.GetConfigDirectory().FullName, "data.db"));
            
            ModuleManager.LoadModules();
        }

        public override void Disable()
        {
            ModuleManager.UnloadModules();
            DatabaseInstance?.Dispose();
            
            DatabaseInstance = null;
            HarmonyInstance = null;
            PluginInstance = null;
        }   
    }
}