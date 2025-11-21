using System;
using System.Linq;
using System.Reflection;
using System.Text;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Permissions;
using Module = DarkCore.Utilities.ModuleManager.Abstracts.Module;

namespace DarkCore.Modules.Mandatory.Watermark
{
    public class Watermark : Module
    {
        public override string Name => "Watermark";
        public override string Description => "Displays a watermark in the console when a player joins.";
        public override string Author => "MollyIO";
        public override Version Version => new Version(1, 1, 0);

        public override void Enable()
        {
            PlayerEvents.Joined += OnJoined;
        }

        public override void Disable()
        {
            PlayerEvents.Joined -= OnJoined;
        }

        private static void OnJoined(PlayerJoinedEventArgs ev)
        {
            ev.Player.SendConsoleMessage(@"
:::::::::      :::     :::::::::  :::    ::: ::::::::   ::::::::  :::::::::  :::::::::: 
:+:    :+:   :+: :+:   :+:    :+: :+:   :+: :+:    :+: :+:    :+: :+:    :+: :+:        
+:+    +:+  +:+   +:+  +:+    +:+ +:+  +:+  +:+        +:+    +:+ +:+    +:+ +:+        
+#+    +:+ +#++:++#++: +#++:++#:  +#++:++   +#+        +#+    +:+ +#++:++#:  +#++:++#   
+#+    +#+ +#+     +#+ +#+    +#+ +#+  +#+  +#+        +#+    +#+ +#+    +#+ +#+        
#+#    #+# #+#     #+# #+#    #+# #+#   #+# #+#    #+# #+#    #+# #+#    #+# #+#        
#########  ###     ### ###    ### ###    ### ########   ########  ###    ### ##########                               

This server is running <color=white>DarkCore v" + DarkCore.PluginInstance.Version + @"</color>.
Made with <3 by <color=purple>MollyIO (Discord: @molly.io)</color>.
");
            
            if (ev.Player.UserGroup == null)
                return;
            
            var moduleTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(Module).IsAssignableFrom(t))
                .ToList();
            
            var stringBuilder = new StringBuilder("\n================ Modules ================\n");
            foreach (var type in moduleTypes)
            {
                if (Activator.CreateInstance(type) is Module module)
                {
                    stringBuilder.Append($"<color={(module.IsLoaded ? "green" : "red")}>");
                    stringBuilder.Append($"{module}");
                    stringBuilder.Append($" - {(module.IsLoaded ? "✅" : "❌")}");
    
                    if (module.IsMandatory)
                        stringBuilder.Append(" <color=red>(Mandatory)</color>");

                    stringBuilder.Append("</color>\n");
                }
            }
            stringBuilder.Append("================ Modules ================");

            ev.Player.SendConsoleMessage(stringBuilder.ToString());
        }
    }
}