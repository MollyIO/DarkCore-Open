using System;
using System.Linq;

namespace DarkCore.Utilities.ModuleManager.Abstracts
{
    /// <summary>
    /// Represents a module that can be enabled or disabled.
    /// </summary>
    public abstract class Module
    {
        /// <summary>
        /// The name of the <see cref="Module"/>.
        /// </summary>
        public abstract string Name { get; }
        
        /// <summary>
        /// A description of the <see cref="Module"/>.
        /// </summary>
        public abstract string Description { get; }
        
        /// <summary>
        /// The author of the <see cref="Module"/>.
        /// </summary>
        public abstract string Author { get; }
        
        /// <summary>
        /// The <see cref="Version"/> of the <see cref="Module"/>.
        /// </summary>
        public abstract Version Version { get; }

        /// <summary>
        /// The <see cref="IsEnabled"/> of the <see cref="Module"/>.
        /// </summary>
        public bool IsEnabled
        {
            get => ModuleProperties.Get(Name).IsEnabled;
            set
            {
                var prop = ModuleProperties.Get(Name);
                prop.IsEnabled = value;
                ModuleProperties.Save(prop, Name);
            }
        }

        /// <summary>
        /// Whether the <see cref="Module"/> is mandatory.
        /// Mandatory modules cannot be disabled.
        /// </summary>
        public bool IsMandatory
        {
            get
            {
                var fullName = GetType().FullName;
                return fullName != null && fullName.Contains(".Mandatory.");
            }
        }

        /// <summary>
        /// Whether the <see cref="Module"/> is currently loaded.
        /// </summary>
        public bool IsLoaded => ModuleManager.Modules.Any(m => m.GetType() == GetType());

        /// <summary>
        /// Called when the <see cref="Module"/> is enabled.
        /// Should be used to register events, etc.
        /// </summary>
        public abstract void Enable();

        /// <summary>
        /// Called when the <see cref="Module"/> is disabled.
        /// Should be used to unregister events, etc.
        /// </summary>
        public abstract void Disable();

        /// <summary>
        /// Generates the default configuration for the <see cref="Module"/>.
        /// </summary>
        public virtual void GenerateConfig()
        {
            ModuleProperties.Get(Name);
        }

        public override string ToString()
        {
            return $"'{Name}' v{Version} by {Author}";
        }
    }
}