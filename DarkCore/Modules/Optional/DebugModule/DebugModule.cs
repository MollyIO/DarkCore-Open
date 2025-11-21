using System;
using DarkCore.Utilities.ModuleManager.Abstracts;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using NetworkManagerUtils.Dummies;
using PlayerRoles;

namespace DarkCore.Modules.Optional.DebugModule
{
    public class DebugModule : Module
    {
        public override string Name => "Debug Module";
        public override string Description => "A module for debugging purposes that spawns dummies and assigns roles.";
        public override string Author => "MollyIO";
        public override Version Version => new Version(1, 1, 0);

        public override void Enable()
        {
            ServerEvents.WaitingForPlayers += OnWaitingForPlayers;
            PlayerEvents.Joined += OnJoined;
        }

        public override void Disable()
        {
            ServerEvents.WaitingForPlayers -= OnWaitingForPlayers;
            PlayerEvents.Joined -= OnJoined;
        }

        private static void OnWaitingForPlayers()
        {
            Round.Start();
            Round.IsLocked = true;
            Server.FriendlyFire = true;

            for (var i = 0; i < 15; i++)
            {
                var dummy = Player.Get(DummyUtils.SpawnDummy("DebugDummy_" + i));
                dummy?.SetRole(RoleTypeId.ChaosRifleman);
            }
        }

        private static void OnJoined(PlayerJoinedEventArgs ev)
        {
            ev.Player.SetRole(RoleTypeId.ChaosRepressor);
            ev.Player.IsNoclipEnabled = true;
        }
    }
}
