using System.ComponentModel;

namespace DarkCore.Utilities.ModuleManager.Configuration
{
    public class Properties
    {
        [Description("Whether the module is enabled or disabled.")]
        public bool IsEnabled { get; set; } = true;
    }
}