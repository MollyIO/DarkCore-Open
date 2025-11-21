using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps.Scp939;

namespace DarkCore.Modules.Optional.ScpBreakingDoors.Patches
{
    public class Scp939ClawAbilityPatch
    {
        private static int _callCount; // Костыльчики (Если убрать будет 2 раза вызываться за 1 атаку)

        public static void Postfix(Scp939ClawAbility __instance)
        {
            if (__instance.Owner == null) return;
            if (!Player.TryGet(__instance.Owner.netIdentity, out var player)) return;

            _callCount++;

            if (_callCount == 2)
            {
                ScpBreakingDoors.Instance.OnAttacking(player);
                _callCount = 0;
            }
        }
    }
}