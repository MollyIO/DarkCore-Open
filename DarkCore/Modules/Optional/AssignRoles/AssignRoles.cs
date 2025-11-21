using System;
using CommandSystem;
using DarkCore.Modules.Optional.AssignRoles.Commands;
using DarkCore.Utilities.CommandManager;
using DarkCore.Utilities.ModuleManager.Abstracts;

namespace DarkCore.Modules.Optional.AssignRoles
{
    public class AssignRoles : Module
    {
        public override string Name => "Assign Roles";
        public override string Description => "Provides command to redistribute roles even if the round has already started.";
        public override string Author => "MollyIO";
        public override Version Version => new Version(1, 0, 0);
        
        public override void Enable()
        {
            CommandManager.RegisterCommand(typeof(AssignRolesCommand), typeof(RemoteAdminCommandHandler));
        }

        public override void Disable()
        {
            CommandManager.UnregisterCommand(typeof(AssignRolesCommand));
        }
    }
}