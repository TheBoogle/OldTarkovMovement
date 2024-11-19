using System.Reflection;
using SPT.Reflection.Patching;
using EFT;
using System.Runtime.Remoting.Contexts;

namespace AnimationReplacer
{
    public class EnableSprintPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // Target the EnableSprint(bool) method in MovementContext
            return typeof(MovementContext).GetMethod("EnableSprint", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool PatchPrefix(bool enable, MovementContext __instance)
        {
            __instance.BlockFirearms = enable;

            return true;
        }
    }
}
