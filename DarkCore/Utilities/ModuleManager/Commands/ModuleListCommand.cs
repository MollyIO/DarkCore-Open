using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using CommandSystem;
using LabApi.Features.Permissions;
using RemoteAdmin;
using Module = DarkCore.Utilities.ModuleManager.Abstracts.Module;

namespace DarkCore.Utilities.ModuleManager.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ModuleListCommand : ICommand
    {
        public string Command => "dcm.list";
        public string[] Aliases { get; } = { "dcm.l" };
        public string Description => "Lists all DarkCore modules and their statuses.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender && !sender.HasPermissions($"{DarkCore.PluginInstance.Name.ToLower()}.{Command.ToLower()}"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }
            
            var moduleTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(Module).IsAssignableFrom(t))
                .ToList();
            
            var stringBuilder = new StringBuilder("Modules:\n");
            foreach (var type in moduleTypes)
            {
                if (Activator.CreateInstance(type) is Module module)
                {
                    stringBuilder.Append($"<color={(module.IsLoaded ? "green" : "red")}>");
                    stringBuilder.Append($"- {module}");
                    stringBuilder.Append($" - {(module.IsLoaded ? "✅" : "❌")}");
    
                    if (module.IsMandatory)
                        stringBuilder.Append(" <color=red>(Mandatory)</color>");

                    stringBuilder.Append("</color>\n");
                }
            }
            
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"<color=green>This server is running <color=white>DarkCore v{DarkCore.PluginInstance.Version}</color></color>");
            stringBuilder.AppendLine("<color=green>Made with <3 by <color=purple>MollyIO (Discord: @molly.io)</color></color>.");

            response = stringBuilder.ToString();
            if (sender is ServerConsoleSender)
            {
                response = Regex.Replace(response, "<.*?>", string.Empty);
            }
            return true;
        }
    }
}