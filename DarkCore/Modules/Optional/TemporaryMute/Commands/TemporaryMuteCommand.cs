using System;
using System.Linq;
using CommandSystem;
using DarkCore.Utilities.Parser;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using RemoteAdmin;

namespace DarkCore.Modules.Optional.TemporaryMute.Commands
{
    public class TemporaryMuteCommand : ICommand
    {
        public string Command => "dc.temporary-mute";
        public string[] Aliases => new[] { "dc.temp-mute", "dc.tm" };
        public string Description => "Temporarily mutes a player for a specified duration.";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender && !sender.HasPermissions($"{DarkCore.PluginInstance.Name.ToLower()}.{Command.ToLower()}"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }
            
            if (arguments.Count < 3)
            {
                response = "Usage: dc.temporary-mute <UserId or PlayerId> <time> <reason>";
                return false;
            }

            try
            {
                var playerId = PlayerParser.ParseUserId(arguments.At(0));
                var time = TimeParser.ParseTimeSpan(arguments.At(1));
                var reason = string.Join(" ", arguments.Skip(2).ToArray());

                var collection = DarkCore.DatabaseInstance.GetCollection<Models.TemporaryMute>();
                collection.Upsert(new Models.TemporaryMute
                {
                    UserId = playerId,
                    MuteEndTime = DateTime.Now.Add(time),
                    Reason = reason
                });

                if (Player.TryGet(playerId, out var player))
                {
                    player.Mute();
                    player.SendBroadcast(TemporaryMute.Instance.Config.MuteBroadcast
                        .Replace("%duration%", TemporaryMute.FormatTimeSpan(time))
                        .Replace("%reason%", reason), 10);
                }
                
                response = $"Гравець з UserId {playerId} був тимчасово замьючений на {TemporaryMute.FormatTimeSpan(time)}. Причина: {reason}";
                return true;
            }
            catch (Exception e)
            {
                response = e.ToString();
                return false;
            }
        }
    }
}