using LabApi.Features.Wrappers;

namespace DarkCore.Modules.Mandatory.Placeholders.Patches
{
    public class PlayerPatch
    {
        public static bool Prefix(Player __instance, ref string text)
        {
            text = Placeholders.ProcessPlaceholders(__instance, text);
            return true;
        }
    }
}