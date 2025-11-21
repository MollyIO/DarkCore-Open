using System.Text.RegularExpressions;
using LabApi.Features.Wrappers;

namespace DarkCore.Utilities.Parser
{
    public class PlayerParser
    {
        public static string ParseUserId(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new System.ArgumentException("Input cannot be null or empty.");

            var match = Regex.Match(input, @"\b(7656119[0-9]{10})\b");
            if (match.Success)
            {
                return match.Groups[1].Value + "@steam";
            }
            
            if (int.TryParse(input, out var playerId) && Player.TryGet(playerId, out var playerById))
            {
                return playerById.UserId;
            }
            
            if (Player.TryGet(input, out var player))
            {
                return player.UserId;
            }

            throw new System.FormatException("Player UserId could not be parsed.");
        }
    }
}