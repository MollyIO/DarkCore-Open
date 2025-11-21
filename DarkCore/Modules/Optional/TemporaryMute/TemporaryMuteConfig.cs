using System.ComponentModel;

namespace DarkCore.Modules.Optional.TemporaryMute
{
    public class TemporaryMuteConfig
    {
        [Description("Повідомлення, яке надсилається гравцю при тимчасовому забороні голосового чату.")]
        public string MuteBroadcast { get; set; } = "<color=red>Вам тимчасово заборонений доступ до голосового чату на </color>%duration%<color=red> Причина: </color>%reason%";
        
        [Description("Повідомлення, яке надсилається гравцю, коли закінчується тимчасова заборона голосового чату.")]
        public string MuteEndBroadcast { get; set; } = "<color=green>Ваша тимчасова заборона на голосовий чат закінчилась.</color>";
        
        [Description("Повідомлення, яке надсилається гравцю, коли адміністратор знімає тимчасову заборону голосового чату.")]
        public string MuteRevokeBroadcast { get; set; } = "<color=green>Ваша тимчасова заборона на голосовий чат була знята адміністратором.</color>";
    }
}