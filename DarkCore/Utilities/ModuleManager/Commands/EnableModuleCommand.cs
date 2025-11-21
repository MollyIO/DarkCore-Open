using System;
using System.Linq;
using System.Reflection;
using CommandSystem;
using LabApi.Features.Permissions;
using RemoteAdmin;
using Module = DarkCore.Utilities.ModuleManager.Abstracts.Module;

namespace DarkCore.Utilities.ModuleManager.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class EnableModuleCommand : ICommand
    {
        public string Command => "dcm.enable";
        public string[] Aliases { get; } = { "dcm.e" };
        public string Description => "Enables a DarkCore module by name.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender && !sender.HasPermissions($"{DarkCore.PluginInstance.Name.ToLower()}.{Command.ToLower()}"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }
            
            if (arguments.Count != 1)
            {
                response = "Usage: dc.enable-module <module_name>";
                return false;
            }

            var moduleType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.IsClass && !t.IsAbstract && typeof(Module).IsAssignableFrom(t) && t.Name.ToLower().Contains(arguments.At(0).ToLower()));
            
            if (moduleType == null)
            {
                response = "Module type not found.";
                return false;
            }
            
            if (!ModuleManager.LoadModule(moduleType, true))
            {
                response = $"Failed to enable module '{moduleType.Name}'.";
                return false;
            }

            response = $"Module '{moduleType.Name}' enabled successfully.";
            return true;
        }
    }
}