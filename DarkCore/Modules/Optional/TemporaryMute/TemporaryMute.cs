using System;
using System.Collections.Generic;
using CommandSystem;
using DarkCore.Modules.Optional.TemporaryMute.Commands;
using DarkCore.Utilities.CommandManager;
using DarkCore.Utilities.ModuleManager.Abstracts;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using MEC;

namespace DarkCore.Modules.Optional.TemporaryMute
{
    public class TemporaryMute : Module<TemporaryMuteConfig>
    {
        public override string Name => "Temporary Mute";
        public override string Description => "Provides a command to temporarily mute players.";
        public override string Author => "MollyIO";
        public override Version Version => new Version(1, 0, 0);
        
        public static TemporaryMute Instance { get; private set; }
        private CoroutineHandle TemporaryMuteCheckCoroutineHandle { get; set; }
        
        public override void Enable()
        {
            DarkCore.DatabaseInstance.GetCollection<Models.TemporaryMute>().EnsureIndex(x => x.UserId);
            
            Instance = this;
            CommandManager.RegisterCommand(typeof(TemporaryMuteCommand), typeof(RemoteAdminCommandHandler));
            CommandManager.RegisterCommand(typeof(UnmuteCommand), typeof(RemoteAdminCommandHandler));
            
            PlayerEvents.Joined += OnJoined;
            TemporaryMuteCheckCoroutineHandle = Timing.RunCoroutine(TemporaryMuteCheckCoroutine());
        }
        
        public override void Disable()
        {
            CommandManager.UnregisterCommand(typeof(TemporaryMuteCommand));
            CommandManager.UnregisterCommand(typeof(UnmuteCommand));
            Instance = null;
            
            PlayerEvents.Joined -= OnJoined;
            Timing.KillCoroutines(TemporaryMuteCheckCoroutineHandle);
            TemporaryMuteCheckCoroutineHandle = default;
        }
        
        private void OnJoined(PlayerJoinedEventArgs ev)
        {
            var collection = DarkCore.DatabaseInstance.GetCollection<Models.TemporaryMute>();
            var muteRecord = collection.FindOne(x => x.UserId == ev.Player.UserId);
            if (muteRecord != null)
            {
                if (DateTime.Now < muteRecord.MuteEndTime)
                {
                    ev.Player.Mute();
                    ev.Player.SendBroadcast(Config.MuteBroadcast
                        .Replace("%duration%", FormatTimeSpan(muteRecord.MuteEndTime - DateTime.Now))
                        .Replace("%reason%", muteRecord.Reason), 10);
                }
                else
                {
                    collection.DeleteMany(x => x.UserId == ev.Player.UserId);
                }
            }
        }
        
        private IEnumerator<float> TemporaryMuteCheckCoroutine()
        {
            var collection = DarkCore.DatabaseInstance.GetCollection<Models.TemporaryMute>();

            while (true)
            {
                foreach (var muteRecord in collection.FindAll())
                {
                    if (DateTime.Now >= muteRecord.MuteEndTime)
                    {
                        if (Player.TryGet(muteRecord.UserId, out var player) && player.IsMuted)
                        {
                            player.Unmute(false);
                            player.SendBroadcast(Config.MuteEndBroadcast, 10);
                        }

                        collection.DeleteMany(x => x.UserId == muteRecord.UserId);
                    }
                }

                yield return Timing.WaitForSeconds(5f);
            }
        }
        
        public static string FormatTimeSpan(TimeSpan time)
        {
            if (time < TimeSpan.Zero)
                time = TimeSpan.Zero;

            var days = time.Days;
            var hours = time.Hours;
            var minutes = time.Minutes;
            var seconds = time.Seconds;

            return $"{days} діб. {hours} год. {minutes} хв. {seconds} с.";
        }
    }
}