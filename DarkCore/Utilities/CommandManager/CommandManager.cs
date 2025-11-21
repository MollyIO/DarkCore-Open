using System;
using System.Linq;
using LabApi.Features.Console;
using LabApi.Loader;

namespace DarkCore.Utilities.CommandManager
{
    public class CommandManager
    {
        public static void RegisterCommand(Type commandType, Type commandHandlerType)
        {
            if (CommandLoader.TryRegisterCommand(commandType, commandHandlerType, out var command, commandType.Name))
            {
                Logger.Info("Registered command: " + command.Command);
            }
        }

        public static void UnregisterCommand(Type commandType)
        {
            var command = CommandLoader.RegisteredCommands[DarkCore.PluginInstance].ToList().FirstOrDefault(c => c.GetType() == commandType);
            if (command != null)
            {
                CommandLoader.UnregisterCommand(command);
                Logger.Info("Unregistered command: " + command.Command);
            } 
        }
    }
}