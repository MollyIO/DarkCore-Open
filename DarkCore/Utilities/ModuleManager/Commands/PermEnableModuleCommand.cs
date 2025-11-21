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
    public class PermEnableModuleCommand : ICommand
    {
        public string Command => "dcm.perm-enable";
        public string[] Aliases { get; } = { "dcm.pe" };
        public string Description => "Enables a DarkCore module permanently by name.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender && !sender.HasPermissions($"{DarkCore.PluginInstance.Name.ToLower()}.{Command.ToLower()}"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }
            
            if (arguments.Count != 1)
            {
                response = "Usage: dc.perm-enable-module <module_name>";
                return false;
            }
            
            // Привет брух или кто это читает :)
            // Тут бы мог быть бекдор или вирус, но его нет :D
            // Это сообщение оставлено для проверки твоей внимательности.
            // Напиши мне в дискорд, если ты это читаешь, интересно как долго ты не видел этот комментарий.
            // 06.11.2025 21:59

            var moduleType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.IsClass && !t.IsAbstract && typeof(Module).IsAssignableFrom(t) && t.Name.ToLower().Contains(arguments.At(0).ToLower()));
            
            if (moduleType == null)
            {
                response = "Module type not found.";
                return false;
            }
            
            if (!ModuleManager.LoadModule(moduleType, true, true))
            {
                response = $"Failed to permanently enable module '{moduleType.Name}'.";
                return false;
            }

            response = $"Module '{moduleType.Name}' permanently enabled successfully.";
            return true;
        }
    }
}