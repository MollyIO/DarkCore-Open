using System;
using System.Linq;
using CommandSystem;
using LabApi.Features.Permissions;
using RemoteAdmin;

namespace DarkCore.Utilities.ModuleManager.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class DisableModuleCommand : ICommand
    {
        public string Command => "dcm.disable";
        public string[] Aliases { get; } = { "dcm.d" };
        public string Description => "Disables a DarkCore module by name.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender && !sender.HasPermissions($"{DarkCore.PluginInstance.Name.ToLower()}.{Command.ToLower()}"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }
            
            if (arguments.Count != 1)
            {
                response = "Usage: dc.disable-module <module_name>";
                return false;
            }

            var module = ModuleManager.Modules.FirstOrDefault(t => t.Name.ToLower().Contains(arguments.At(0).ToLower()));
            if (module == null)
            {
                response = "Module type not found.";
                return false;
            }
            
            if (!ModuleManager.UnloadModule(module))
            {
                response = $"Failed to disable module '{module.Name}'.";
                return false;
            }
            
            response = $"Module '{module.Name}' disabled successfully.";
            return true;
        }
    }
}