using EFT.Interactive;
using EFT;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OldTarkovMovement.Patches
{
    public class NostalgiaPatrolFixEnterPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(SprintStateClass).GetMethod(nameof(SprintStateClass.Enter), BindingFlags.Public | BindingFlags.Instance);

        [PatchPostfix]
        private static void PostFix(SprintStateClass __instance)
        {
            try
            {
                __instance.MovementContext.SetPatrol(true);
            }
            catch (Exception ex)
            {
                Logger.LogError($"GenericPatch Prefix failed: {ex}");
            }
        }
    }

    public class NostalgiaPatrolFixExitPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(SprintStateClass).GetMethod(nameof(SprintStateClass.Exit), BindingFlags.Public | BindingFlags.Instance);

        [PatchPostfix]
        private static void PostFix(SprintStateClass __instance)
        {
            try
            {
                __instance.MovementContext.SetPatrol(false);
            }
            catch (Exception ex)
            {
                Logger.LogError($"GenericPatch Prefix failed: {ex}");
            }
        }
    }
}
