using System.Collections.Generic;
using System.ComponentModel;
using PlayerRoles;

namespace DarkCore.Modules.Optional.GroupLimits
{
    public class GroupLimitsConfig
    {
        [Description("A list of give limits for different user groups.")]
        public List<GiveLimits> GiveLimitsByGroup { get; set; } = new List<GiveLimits>
        {
            new GiveLimits()
        };
        
        [Description("The message displayed to the player when they exceed their item give limit. %used% with the number of items given, and %limit% with the maximum allowed.")]
        public string GiveLimitExceededMessage { get; set; } = "<color=red>Ви вичерпали ліміт видачі предметів в цьому раунді! (%used%/%limit%)</color>";
        
        [Description("The message displayed to the player when they attempt to give an item they are not allowed to give. %item% with the name of the item.")]
        public string GiveItemNotAllowedMessage { get; set; } = "<color=red>Вам не дозволено видавати %item%!</color>";
        
        public class GiveLimits
        {
            [Description("The name of the group to which these give limits apply.")]
            public string GroupName { get; set; } = "donator";
            
            [Description("If true, players in this group can only give items to themselves.")]
            public bool CanOnlyGiveToThemselves { get; set; } = true;
            
            [Description("The maximum number of items a player in this group can give themselves per round.")]
            public int RoundLimit { get; set; } = 5;
            
            [Description("If true, only items in the AllowedItems list can be given. If false, all items except those in the AllowedItems list can be given.")]
            public bool WhitelistMode { get; set; } = true;
            
            [Description("The list of items that this give limit applies to.")]
            public List<ItemType> AllowedItems { get; set; } = new List<ItemType>
            {
                ItemType.Medkit,
                ItemType.Adrenaline
            };
        }
        
        [Description("A list of role limits for different user groups.")]
        public List<RoleLimits> RoleLimitsByGroup { get; set; } = new List<RoleLimits>
        {
            new RoleLimits()
        };
        
        [Description("The message displayed to the player when they exceed their role limit. %used% with the number of roles given, and %limit% with the maximum allowed.")]
        public string RoleLimitExceededMessage { get; set; } = "<color=red>Ви вичерпали ліміт видачі ролей в цьому раунді! (%used%/%limit%)</color>";
        
        [Description("The message displayed to the player when they attempt to give role they are not allowed to give. %role% with the name of the role.")]
        public string GiveRoleNotAllowedMessage { get; set; } = "<color=red>Вам не дозволено видавати %role%!</color>";
        
        public class RoleLimits
        {
            [Description("The name of the group to which these role limits apply.")]
            public string GroupName { get; set; } = "donator";
            
            [Description("The maximum number of roles a player in this group can give themselves per round.")]
            public int RoundLimit { get; set; } = 5;
            
            [Description("If true, only roles in the AllowedRoles list can be given. If false, all roles except those in the AllowedRoles list can be given.")]
            public bool WhitelistMode { get; set; } = true;
            
            [Description("The list of roles that this give limit applies to.")]
            public List<RoleTypeId> AllowedRoles { get; set; } = new List<RoleTypeId>
            {
                RoleTypeId.Scp049,
                RoleTypeId.Scientist,
                RoleTypeId.ClassD
            };
        }
        
        [Description("A list of command restrictions for different user groups.")]
        public List<CommandRestrictions> CommandRestrictionsByGroup { get; set; } = new List<CommandRestrictions>
        {
            new CommandRestrictions()
        };
        
        [Description("Message displayed to the player when they attempt to use a restricted command.")]
        public string CommandRestrictedMessage { get; set; } = "<color=red>Вам не дозволено використовувати цю команду!</color>";
        
        public class CommandRestrictions
        {
            [Description("The name of the group to which these command restrictions apply.")]
            public string GroupName { get; set; } = "donator";
            
            [Description("The list of commands that are disabled for this group.")]
            public List<string> DisabledCommands { get; set; } = new List<string>
            {
                "grantloadout",
                
                "removeitem",
                "clear",
                "disarm",
                "free",
                "forceceequip"
            };
        }
    }
}