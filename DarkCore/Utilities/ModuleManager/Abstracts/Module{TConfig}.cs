namespace DarkCore.Utilities.ModuleManager.Abstracts
{
    /// <summary>
    /// Represents a module that can be enabled or disabled.
    /// </summary>
    public abstract class Module<TConfig> : Module where TConfig : class, new()
    {
        /// <summary>
        /// The configuration of the <see cref="Module"/>.
        /// </summary>
        public TConfig Config
        {
            get => ModuleConfig.Get<TConfig>(Name);
            set => ModuleConfig.Save(value, Name);
        }
        
        /// <summary>
        /// Generates the configuration file for the <see cref="Module"/>.
        /// </summary>
        public override void GenerateConfig()
        {
            base.GenerateConfig();
            ModuleConfig.Get<TConfig>(Name);
        }
    }
}