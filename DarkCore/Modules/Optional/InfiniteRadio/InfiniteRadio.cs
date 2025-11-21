using System;
using DarkCore.Utilities.ModuleManager.Abstracts;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;

namespace DarkCore.Modules.Optional.InfiniteRadio
{
    public class InfiniteRadio : Module
    {
        public override string Name => "Infinite Radio";
        public override string Description => "Prevents radios from draining battery.";
        public override string Author => "MollyIO";
        public override Version Version => new Version(1, 0, 0);

        public override void Enable()
        {
            PlayerEvents.UsingRadio += OnUsingRadio;
            PlayerEvents.PickedUpItem += OnPickedUpItem;
        }

        public override void Disable()
        {
            PlayerEvents.UsingRadio -= OnUsingRadio;
            PlayerEvents.PickedUpItem -= OnPickedUpItem;
        }
        
        private static void OnUsingRadio(PlayerUsingRadioEventArgs ev)
        {
            ev.Drain = 0;
        }
        
        private static void OnPickedUpItem(PlayerPickedUpItemEventArgs ev)
        {
            if (ev.Item.Type != ItemType.Radio) return;
            
            if (ev.Item is RadioItem radio)
            {
                radio.BatteryPercent = 100;
            }
        }
    }
}