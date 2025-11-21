using System;
using System.Linq;
using CommandSystem;
using GameCore;
using PlayerRoles;
using PlayerRoles.RoleAssign;
using Utils.NonAllocLINQ;

namespace DarkCore.Modules.Optional.AssignRoles.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class AssignRolesCommand : ICommand
    {
        public string Command => "dc.assign-roles";
        public string[] Aliases => new[] { "dc.ar" };
        public string Description => "Redistributes roles even if the round has already started.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            foreach (var hub in ReferenceHub.AllHubs.Where(hub => hub.IsAlive()))
            {
                hub.roleManager.ServerSetRole(RoleTypeId.Spectator, RoleChangeReason.RemoteAdmin);
            }

            var queue = ConfigFile.ServerConfig.GetString("team_respawn_queue", "4014314031441404134041434414");
            var totalPlayers = ReferenceHub.AllHubs.Count(RoleAssigner.CheckPlayer);

            var totalQueueSize = queue.Length;
            var scpCount = 0;
            var maxScps = ScpSpawner.MaxSpawnableScps;
            var allowOverflow = ConfigFile.ServerConfig.GetBool("allow_scp_overflow");

            for (var i = 0; i < totalPlayers; i++)
            {
                if ((Team)(queue[i % totalQueueSize] - 48U) == Team.SCPs)
                {
                    scpCount++;
                    if (scpCount > maxScps && !allowOverflow)
                        break;
                }
            }

            ScpSpawner.SpawnScps(scpCount);

            var humanQueue = queue
                .Select(c => (Team)(c - 48U))
                .Where(t => t != Team.SCPs)
                .ToArray();
            HumanSpawner.SpawnHumans(humanQueue, humanQueue.Length);

            response = $"Reassigned all roles successfully ({totalPlayers} players, {scpCount} SCPs).";
            return true;
        }
    }
}
