using System;
using System.Collections.Generic;
using DarkCore.Utilities.ModuleManager.Abstracts;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using MEC;

namespace DarkCore.Modules.Optional.AutoBroadcast
{
    public class AutoBroadcast : Module<AutoBroadcastConfig>
    {
        public override string Name => "Auto Broadcast";
        public override string Description => "Automatically sends broadcasts to players based on configured events.";
        public override string Author => "MollyIO";
        public override Version Version => new Version(1, 1, 0);

        public override void Enable()
        {
            PlayerEvents.Joined += OnJoined;
            ServerEvents.RoundEnded += OnRoundEnded;

            foreach (var message in Config.ScheduledMessages)
            {
                Timing.RunCoroutine(BroadcastScheduled(message), $"AutoBroadcast_Scheduled_{message.Key}");
            }
            
            foreach (var message in Config.PeriodicMessages)
            {
                Timing.RunCoroutine(BroadcastPeriodically(message), $"AutoBroadcast_Periodic_{message.Key}");
            }
        }
        
        public override void Disable()
        {
            PlayerEvents.Joined -= OnJoined;
            ServerEvents.RoundEnded -= OnRoundEnded;
            
            foreach (var message in Config.ScheduledMessages)
            {
                Timing.KillCoroutines($"AutoBroadcast_Scheduled_{message.Key}");
            }
            
            foreach (var message in Config.PeriodicMessages)
            {
                Timing.KillCoroutines($"AutoBroadcast_Periodic_{message.Key}");
            }
        }
        
        private void OnJoined(PlayerJoinedEventArgs ev)
        {
            var message = Config.WelcomeMessages[new Random().Next(0, Config.WelcomeMessages.Count)];
            ev.Player.SendBroadcast(message.Text, (ushort)message.Time);
        }
        
        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            var message = Config.EndRoundBroadcast[new Random().Next(0, Config.EndRoundBroadcast.Count)];
            foreach (var player in Player.List)
            {
                player.SendBroadcast(message.Text, (ushort)message.Time);
            }
        }
        
        private static IEnumerator<float> BroadcastScheduled(KeyValuePair<int, List<AutoBroadcastConfig.BroadcastMessage>> messageConfig)
        {
            yield return Timing.WaitForSeconds(messageConfig.Key);
            
            var message = messageConfig.Value[new Random().Next(0, messageConfig.Value.Count)];
            foreach (var player in Player.List)
            {
                player.SendBroadcast(message.Text, (ushort)message.Time);
            }
        }
        
        private static IEnumerator<float> BroadcastPeriodically(KeyValuePair<int, List<AutoBroadcastConfig.BroadcastMessage>> messageConfig)
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(messageConfig.Key);
                
                var message = messageConfig.Value[new Random().Next(0, messageConfig.Value.Count)];
                foreach (var player in Player.List)
                {
                    player.SendBroadcast(message.Text, (ushort)message.Time);
                }
            }
        }
    }
}