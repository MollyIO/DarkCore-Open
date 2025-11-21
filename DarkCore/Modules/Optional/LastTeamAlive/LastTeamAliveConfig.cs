using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PlayerRoles;

namespace DarkCore.Modules.Optional.LastTeamAlive
{
    public class LastTeamAliveConfig
    {
        [Description("Хинт который приходит последнему выжившему из команды.")]
        public string HintText { get; set; } = "<role-color><b>Ви залишились останнім зі своєї команди!</b></color>";
            
        [Description("Время на которое показывается хинт.")]
        public float HintDuration { get; set; } = 7f;

        [Description("На какие команды распространяется модуль.")]
        public Dictionary<Team, bool> ApplicableTeams { get; set; } = Enum.GetValues(typeof(Team)).Cast<Team>().ToDictionary(t => t, _ => true);
    }
}