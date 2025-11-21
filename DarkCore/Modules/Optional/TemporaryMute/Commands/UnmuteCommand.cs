using System;
using CommandSystem;
using DarkCore.Utilities.Parser;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using RemoteAdmin;

namespace DarkCore.Modules.Optional.TemporaryMute.Commands
{
    public class UnmuteCommand : ICommand
    {
        public string Command => "dc.unmute";
        public string[] Aliases => new[] { "dc.um" };
        public string Description => "Removes a temporary mute from a player.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is PlayerCommandSender && !sender.HasPermissions($"{DarkCore.PluginInstance.Name.ToLower()}.{Command.ToLower()}"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage: dc.unmute <UserId or PlayerId>";
                return false;
            }

            try
            {
                var playerId = PlayerParser.ParseUserId(arguments.At(0));

                var collection = DarkCore.DatabaseInstance.GetCollection<Models.TemporaryMute>();
                collection.EnsureIndex(x => x.UserId);

                var muteEntry = collection.FindOne(x => x.UserId == playerId);
                if (muteEntry == null)
                {
                    response = $"Гравець з UserId {playerId} не має активного мута.";
                    return false;
                }

                collection.DeleteMany(x => x.UserId == playerId);
                if (Player.TryGet(playerId, out var player))
                {
                    player.Unmute(false);
                    player.SendBroadcast(TemporaryMute.Instance.Config.MuteRevokeBroadcast, 10);
                }

                response = $"Мут гравця {playerId} було успішно знято.";
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
