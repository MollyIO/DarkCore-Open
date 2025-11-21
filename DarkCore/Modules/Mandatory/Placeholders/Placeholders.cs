using System;
using DarkCore.Modules.Mandatory.Placeholders.Patches;
using DarkCore.Utilities.ModuleManager.Abstracts;
using HarmonyLib;
using Hints;
using LabApi.Features.Wrappers;
using PlayerRoles;
using Respawning.Objectives;

namespace DarkCore.Modules.Mandatory.Placeholders
{
    public class Placeholders : Module
    {
        public override string Name => "Placeholders";
        public override string Description => "Provides placeholders for use in other modules' messages.";
        public override string Author => "MollyIO";
        public override Version Version => new Version(1, 0, 0);
        
        public override void Enable()
        {
            DarkCore.HarmonyInstance.Patch(
                AccessTools.Method(
                    typeof(Player),
                    nameof(Player.SendHint),
                    new[]
                    {
                        typeof(string),
                        typeof(HintParameter[]),
                        typeof(HintEffect[]),
                        typeof(float)
                    }
                ),
                new HarmonyMethod(typeof(PlayerPatch), nameof(PlayerPatch.Prefix))
            );
        }

        public override void Disable()
        {
            DarkCore.HarmonyInstance.Unpatch(
                AccessTools.Method(
                    typeof(Player),
                    nameof(Player.SendHint),
                    new[]
                    {
                        typeof(string),
                        typeof(HintParameter[]),
                        typeof(HintEffect[]),
                        typeof(float)
                    }
                ),
                AccessTools.Method(typeof(PlayerPatch), nameof(PlayerPatch.Prefix))
            );
        }

        public static string ProcessPlaceholders(Player player, string text)
        {
            text = text.Replace("%player-id%", player.PlayerId.ToString());
            text = text.Replace("%player-userid%", player.UserId);
            text = text.Replace("%player-nickname%", player.Nickname);
            text = text.Replace("%player-role%", player.Role.GetAbbreviatedRoleName());
            text = text.Replace("%player-role-color%", player.Role.GetRoleColor().ToHex());

            return text;
        }
    }
}