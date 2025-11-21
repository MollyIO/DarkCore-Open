using System;
using System.Collections.Generic;
using DarkCore.Modules.Optional.ScpBreakingDoors.Patches;
using DarkCore.Utilities.ModuleManager.Abstracts;
using HarmonyLib;
using Interactables.Interobjects.DoorUtils;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp939;
using UnityEngine;

namespace DarkCore.Modules.Optional.ScpBreakingDoors
{
    public class ScpBreakingDoors : Module<ScpBreakingDoorsConfig>
    {
        public override string Name => "SCP Breaking Doors";
        public override string Description => "Allows SCPs to break doors.";
        public override string Author => "MollyIO";
        public override Version Version => new Version(1, 0, 0);
        
        public static ScpBreakingDoors Instance { get; private set; }
        private static readonly Dictionary<Door, int> DamagedDoors = new Dictionary<Door, int>();
        
        public override void Enable()
        {
            Instance = this;
            DarkCore.HarmonyInstance.Patch(
                AccessTools.Method(typeof(Scp939ClawAbility), nameof(Scp939ClawAbility.ServerProcessCmd)),
                null,
                AccessTools.Method(typeof(Scp939ClawAbilityPatch), nameof(Scp939ClawAbilityPatch.Postfix))
            );
        }
        
        public override void Disable()
        {
            DarkCore.HarmonyInstance.Unpatch(
                AccessTools.Method(typeof(Scp939ClawAbility), nameof(Scp939ClawAbility.ServerProcessCmd)),
                AccessTools.Method(typeof(Scp939ClawAbilityPatch), nameof(Scp939ClawAbilityPatch.Postfix))
            );
            Instance = null;
            DamagedDoors.Clear();
        }
        
        public void OnAttacking(Player player)
        {
            if (!Physics.Raycast(player.Camera.position, player.Camera.forward, out var hit, 1, ~(1 << 1 | 1 << 13 | 1 << 16 | 1 << 28)))
                return;

            var door = Door.Get(hit.collider.gameObject.GetComponentInParent<DoorVariant>());
            if (door == null || door.IsOpened || !(door is BreakableDoor breakableDoor))
                return;

            if (!DamagedDoors.ContainsKey(door))
                DamagedDoors[door] = 0;

            DamagedDoors[door]++;

            player.SendHint(Config.DoorHealthHint.Replace("%health%", Mathf.Clamp(Config.DoorHealth - DamagedDoors[door], 0, Config.DoorHealth).ToString()), 2.5f);
            player.SendHitMarker();
            door.PlayLockBypassDeniedSound();
            door.PlayPermissionDeniedAnimation();

            if (DamagedDoors[door] >= Config.DoorHealth)
            {
                breakableDoor.TryBreak();
                DamagedDoors.Remove(door);
            }
        }
    }
}