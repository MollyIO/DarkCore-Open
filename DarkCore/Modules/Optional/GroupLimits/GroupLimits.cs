using System;
using System.Collections.Generic;
using System.Linq;
using DarkCore.Utilities.ModuleManager.Abstracts;
using InventorySystem.Items;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace DarkCore.Modules.Optional.GroupLimits
{
    public class GroupLimits : Module<GroupLimitsConfig>
    {
        private static readonly Dictionary<string, int> GiveLimitCount = new Dictionary<string, int>();
        private static readonly Dictionary<string, int> RoleLimitCount = new Dictionary<string, int>();

        public override string Name => "Group Limits";
        public override string Description => "Implements command usage and argument limits based on user groups.";
        public override string Author => "MollyIO";
        public override Version Version => new Version(1, 1, 0);

        public override void Enable()
        {
            ServerEvents.CommandExecuting += OnCommandExecuting;
            ServerEvents.WaitingForPlayers += OnWaitingForPlayers;
        }

        public override void Disable()
        {
            ServerEvents.CommandExecuting -= OnCommandExecuting;
            ServerEvents.WaitingForPlayers -= OnWaitingForPlayers;
        }

        private void OnCommandExecuting(CommandExecutingEventArgs ev)
        {
            var player = Player.Get(ev.Sender);
            if (player == null)
                return;
            
            if (ev.CommandName == "give")
            {
                if (ev.Arguments.Count < 2) 
                    return;
                
                // Получаем лимит для группы игрока
                var groupLimit = Config.GiveLimitsByGroup.FirstOrDefault(x => player.UserGroup != null && x.GroupName == player.UserGroup.Name);
                if (groupLimit == null)
                    return;
                
                // Проверяем счетчик выданных предметов
                if (!GiveLimitCount.ContainsKey(player.UserId))
                {
                    GiveLimitCount[player.UserId] = 0;
                }
                
                if (GiveLimitCount[player.UserId] >= groupLimit.RoundLimit)
                {
                    var message = Config.GiveLimitExceededMessage
                        .Replace("%used%", GiveLimitCount[player.UserId].ToString())
                        .Replace("%limit%", groupLimit.RoundLimit.ToString());
                    player.SendBroadcast(message, 3);
                    ev.IsAllowed = false;
                    return;
                }
                
                // Получаем ID игрока и проверяем, можно ли выдавать предметы другим игрокам
                var playerId = int.Parse(ev.Arguments.At(0).Replace(".", ""));
                if (player.PlayerId != playerId && groupLimit.CanOnlyGiveToThemselves)
                {
                    ev.IsAllowed = false;
                    return;
                }
                
                // Проверяем, разрешен ли выдаваемый предмет
                var itemId = int.Parse(ev.Arguments.At(1).Replace(".", ""));
                var itemType = (ItemType)itemId;
                var isItemAllowed = groupLimit.WhitelistMode
                    ? groupLimit.AllowedItems.Contains(itemType)
                    : !groupLimit.AllowedItems.Contains(itemType);
                
                // Обрабатываем результат проверки
                if (!isItemAllowed)
                {
                    var message = Config.GiveItemNotAllowedMessage
                        .Replace("%item%", itemType.GetName());
                    player.SendBroadcast(message, 3);
                    ev.IsAllowed = false;
                    return;
                }
                
                GiveLimitCount[player.UserId]++;
            } 
            
            else if (ev.CommandName == "forceclass")
            {
                if (ev.Arguments.Count < 2) 
                    return;
                
                // Получаем лимит для группы игрока
                var groupLimit = Config.RoleLimitsByGroup.FirstOrDefault(x => player.UserGroup != null && x.GroupName == player.UserGroup.Name);
                if (groupLimit == null)
                    return;
                
                // Проверяем счетчик выданных ролей
                if (!RoleLimitCount.ContainsKey(player.UserId))
                {
                    RoleLimitCount[player.UserId] = 0;
                }
                
                if (RoleLimitCount[player.UserId] >= groupLimit.RoundLimit)
                {
                    var message = Config.RoleLimitExceededMessage
                        .Replace("%used%", RoleLimitCount[player.UserId].ToString())
                        .Replace("%limit%", groupLimit.RoundLimit.ToString());
                    player.SendBroadcast(message, 3);
                    ev.IsAllowed = false;
                    return;
                }
                
                // Проверяем, разрешена ли выдаваемая роль
                if (!Enum.TryParse<RoleTypeId>(ev.Arguments.At(1), true, out var roleTypeId))
                    return;
                var isRoleAllowed = groupLimit.WhitelistMode
                    ? groupLimit.AllowedRoles.Contains(roleTypeId)
                    : !groupLimit.AllowedRoles.Contains(roleTypeId);
                
                // Обрабатываем результат проверки
                if (!isRoleAllowed)
                {
                    var message = Config.GiveRoleNotAllowedMessage
                        .Replace("%role%", roleTypeId.GetAbbreviatedRoleName());
                    player.SendBroadcast(message, 3);
                    ev.IsAllowed = false;
                    return;
                }
                
                RoleLimitCount[player.UserId]++;
            }

            // Проверяем ограничения команд по группам
            else if (Config.CommandRestrictionsByGroup.Any(x => player.UserGroup != null && x.GroupName == player.UserGroup.Name))
            {
                var groupRestriction = Config.CommandRestrictionsByGroup.First(x => player.UserGroup != null && x.GroupName == player.UserGroup.Name);
                if (groupRestriction.DisabledCommands.Contains(ev.CommandName))
                {
                    player.SendBroadcast(Config.CommandRestrictedMessage, 3);
                    ev.IsAllowed = false;
                }
            }
        }

        private static void OnWaitingForPlayers()
        {
            GiveLimitCount.Clear();
            RoleLimitCount.Clear();
        }
    }
}