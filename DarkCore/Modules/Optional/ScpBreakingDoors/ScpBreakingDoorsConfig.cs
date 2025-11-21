using System.ComponentModel;

namespace DarkCore.Modules.Optional.ScpBreakingDoors
{
    public class ScpBreakingDoorsConfig
    {
        [Description("Міцність дверей.")]
        public int DoorHealth { get; set; } = 50;
        
        [Description("Текст підказки, що показується гравцю при атаці дверей.")]
        public string DoorHealthHint { get; set; } = "<color=%player-role-color%>🚪 Міцність двері:</color> %health%";
    }
}