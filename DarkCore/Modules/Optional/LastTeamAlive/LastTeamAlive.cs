using System;
using System.Linq;
using DarkCore.Utilities.ModuleManager.Abstracts;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace DarkCore.Modules.Optional.LastTeamAlive
{
    public class LastTeamAlive : Module<LastTeamAliveConfig>
    {
        public override string Name => "Last Team Alive";
        public override string Description => "Sends a hint to the last player alive on a team when all other members of their team have died.";
        public override string Author => "MollyIO";
        public override Version Version => new Version(1, 0, 0);

        public override void Enable()
        {
            PlayerEvents.Death += OnDeath;
        }

        public override void Disable()
        {
            PlayerEvents.Death -= OnDeath;
        }
        
        private void OnDeath(PlayerDeathEventArgs ev)
        {
            var team = ev.OldRole.GetTeam();
            var alivePlayers = Player.List.Where(x => x.Team == team).ToList();

            if (alivePlayers.Count > 1)
                return;

            if (!Config.ApplicableTeams.ContainsKey(team))
            {
                var config = Config;
                config.ApplicableTeams.Add(team, true);
                Config = config;
            }

            if (!Config.ApplicableTeams[team]) return;
            if (alivePlayers.Count == 0) return;

            var lastPlayer = alivePlayers.First();
            var message = Config.HintText.Replace("<role-color>", $"<color={lastPlayer.RoleBase.RoleColor.ToHex()}>");
            lastPlayer.SendHint(message, Config.HintDuration);

#if DEBUG
            Logger.Debug($"[LastTeamAlive] Sent hint to {lastPlayer.DisplayName} ({lastPlayer.Role})");
#endif
        }
    }
}